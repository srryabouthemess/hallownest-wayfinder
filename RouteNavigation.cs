using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HallownestWayfinder
{
    public enum NavigationKind
    {
        Precise,
        General,
        Transport,
        Arrived,
        Unmapped
    }

    public sealed class NavigationResult
    {
        public NavigationKind Kind { get; set; }
        public float Degrees { get; set; }
        public string Label { get; set; } = string.Empty;
        public bool ShowArrow => Kind == NavigationKind.Precise || Kind == NavigationKind.General;
    }

    public static class RouteNavigation
    {
        private static string? _objectiveCacheKey;
        private static Transform? _objectiveTarget;
        private static float _nextObjectiveSearch;

        public static NavigationResult Resolve(RouteStep? step, bool intelligent)
        {
            if (step == null)
                return General(0f);

            if (!intelligent)
                return General(step.ArrowDegrees);

            string? scene = GameManager.instance == null ? null : GameManager.instance.sceneName;
            Vector3 hero = HeroController.instance == null
                ? Vector3.zero
                : HeroController.instance.transform.position;

            NavigationWaypoint[] points = step.Navigation == null
                ? Array.Empty<NavigationWaypoint>()
                : step.Navigation.Where(point => point != null && point.Scene == scene)
                    .OrderBy(point => point.Order).ToArray();

            foreach (NavigationWaypoint point in points)
            {
                Vector2 position = new Vector2(point.X, point.Y);
                if (point.TargetObjectName != null)
                {
                    Transform? target = FindTransition(point.TargetObjectName);
                    if (target == null) continue;
                    position = target.position;
                }
                Vector2 delta = position - (Vector2)hero;
                if (delta.magnitude <= Mathf.Max(0.5f, point.ArrivalRadius))
                    continue;

                // A textura original da seta aponta para cima (0 graus).
                float degrees = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;
                return new NavigationResult
                {
                    Kind = NavigationKind.Precise,
                    Degrees = degrees,
                    Label = point.Label == null || point.Label.Length == 0
                        ? LocalizationService.Text("next_exit", "Próxima saída")
                        : point.Label
                };
            }

            if (points.Length > 0)
            {
                return new NavigationResult
                {
                    Kind = NavigationKind.Arrived,
                    Label = LocalizationService.Text("point_reached", "Ponto alcançado — siga a instrução")
                };
            }

            string? destinationScene = step.GetTargetScene();
            string? transportInstruction = LocalizationService.StepTransport(step);
            if (destinationScene != null && destinationScene.Length > 0)
            {
                if (scene == destinationScene)
                {
                    if (!string.IsNullOrEmpty(step.TransportScene) &&
                        scene == step.TransportScene &&
                        transportInstruction != null && transportInstruction.Length > 0)
                    {
                        return new NavigationResult
                        {
                            Kind = NavigationKind.Transport,
                            Label = transportInstruction
                        };
                    }

                    NavigationWaypoint? objective = FindObjectiveWaypoint(step, destinationScene, hero);
                    if (objective != null)
                    {
                        Vector2 delta = new Vector2(objective.X - hero.x, objective.Y - hero.y);
                        if (delta.magnitude > Mathf.Max(0.5f, objective.ArrivalRadius))
                        {
                            return new NavigationResult
                            {
                                Kind = NavigationKind.Precise,
                                Degrees = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg,
                                Label = LocalizationService.Text("objective_here", "Objetivo nesta sala")
                            };
                        }
                    }

                    if (transportInstruction != null && transportInstruction.Length > 0)
                    {
                        return new NavigationResult
                        {
                            Kind = NavigationKind.Transport,
                            Label = transportInstruction
                        };
                    }
                    return new NavigationResult
                    {
                        Kind = NavigationKind.Arrived,
                            Label = LocalizationService.Text("objective_here", "Objetivo nesta sala")
                    };
                }

                if (VanillaRouteGraph.TryGetNextDoor(scene, destinationScene, out string? door) &&
                    door != null)
                {
                    Transform? exit = FindTransition(door);
                    if (exit != null)
                    {
                        Vector2 delta = (Vector2)exit.position - (Vector2)hero;
                        return new NavigationResult
                        {
                            Kind = NavigationKind.Precise,
                            Degrees = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg,
                            Label = LocalizationService.Text("next_exit", "Próxima saída")
                        };
                    }

                    return new NavigationResult
                    {
                        Kind = NavigationKind.Unmapped,
                        Label = LocalizationService.Text("exit_pending", "Saída calculada, aguardando ponto da sala")
                    };
                }
            }

            if (transportInstruction != null && transportInstruction.Length > 0)
            {
                return new NavigationResult
                {
                    Kind = NavigationKind.Transport,
                    Label = transportInstruction
                };
            }

            return General(step.ArrowDegrees);
        }

        private static NavigationWaypoint? FindObjectiveWaypoint(RouteStep step, string scene, Vector3 hero)
        {
            string cacheKey = scene + "\n" + step.Id;
            if (_objectiveCacheKey != cacheKey)
            {
                _objectiveCacheKey = cacheKey;
                _objectiveTarget = null;
                _nextObjectiveSearch = 0f;
            }

            if (_objectiveTarget != null) return WaypointAt(_objectiveTarget, scene);
            if (Time.unscaledTime < _nextObjectiveSearch) return null;
            _nextObjectiveSearch = Time.unscaledTime + 2f;

            HashSet<string> keys = TrackingKeys(step);
            Transform? closest = null;
            float closestDistance = float.MaxValue;
            Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (keys.Count > 0)
            {
                foreach (GameObject root in activeScene.GetRootGameObjects())
                {
                    PlayMakerFSM[] fsms = root.GetComponentsInChildren<PlayMakerFSM>(true);
                    foreach (PlayMakerFSM fsm in fsms)
                    {
                        if (!WritesAnyPlayerDataKey(fsm, keys)) continue;
                        float distance = ((Vector2)fsm.transform.position - (Vector2)hero).sqrMagnitude;
                        if (distance >= closestDistance) continue;
                        closest = fsm.transform;
                        closestDistance = distance;
                    }
                }
            }

            if (closest == null)
                closest = FindNamedObjective(activeScene, step, hero);

            if (closest == null) return null;
            _objectiveTarget = closest;
            return WaypointAt(closest, scene);
        }

        private static NavigationWaypoint WaypointAt(Transform target, string scene) =>
            new NavigationWaypoint
            {
                Scene = scene,
                X = target.position.x,
                Y = target.position.y,
                ArrivalRadius = 2.5f,
                Label = LocalizationService.Text("objective_here", "Objetivo nesta sala")
            };

        private static Transform? FindNamedObjective(Scene scene, RouteStep step, Vector3 hero)
        {
            string? token = !string.IsNullOrEmpty(step.Completion.GrubScene)
                ? "Grub"
                : !string.IsNullOrEmpty(step.Completion.BenchScene) ? "Bench" : null;
            if (token == null) return null;

            Transform? closest = null;
            float closestDistance = float.MaxValue;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
                foreach (Transform candidate in transforms)
                {
                    if (candidate.name.IndexOf(token, StringComparison.OrdinalIgnoreCase) < 0) continue;
                    float distance = ((Vector2)candidate.position - (Vector2)hero).sqrMagnitude;
                    if (distance >= closestDistance) continue;
                    closest = candidate;
                    closestDistance = distance;
                }
            }
            return closest;
        }

        private static HashSet<string> TrackingKeys(RouteStep step)
        {
            HashSet<string> keys = new HashSet<string>(StringComparer.Ordinal);
            AddKey(keys, step.Completion.PlayerBool);
            AddKey(keys, step.Completion.PlayerInt);
            AddKeys(keys, step.Completion.AllPlayerBools);
            AddKeys(keys, step.Completion.AnyPlayerBools);
            AddKeys(keys, step.Completion.PlayerIntSum);
            return keys;
        }

        private static void AddKey(HashSet<string> keys, string? key)
        {
            if (key != null && key.Length > 0) keys.Add(key);
        }

        private static void AddKeys(HashSet<string> keys, string[]? values)
        {
            if (values == null) return;
            foreach (string value in values) AddKey(keys, value);
        }

        private static bool WritesAnyPlayerDataKey(PlayMakerFSM fsm, HashSet<string> keys)
        {
            FsmState[]? states;
            try
            {
                states = fsm.FsmStates;
            }
            catch
            {
                return false;
            }

            if (states == null) return false;
            foreach (FsmState state in states)
            {
                if (state?.Actions == null) continue;
                foreach (FsmStateAction action in state.Actions)
                {
                    if (action == null || !MutatesPlayerData(action.GetType().Name)) continue;
                    FieldInfo[] fields = action.GetType().GetFields(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    foreach (FieldInfo field in fields)
                    {
                        if (field.FieldType != typeof(FsmString)) continue;
                        FsmString? value = field.GetValue(action) as FsmString;
                        if (value != null && keys.Contains(value.Value)) return true;
                    }
                }
            }
            return false;
        }

        private static bool MutatesPlayerData(string actionName) =>
            actionName.StartsWith("SetPlayerData", StringComparison.Ordinal) ||
            actionName.StartsWith("IncrementPlayerData", StringComparison.Ordinal) ||
            actionName.StartsWith("DecrementPlayerData", StringComparison.Ordinal);

        private static NavigationResult General(float degrees) => new NavigationResult
        {
            Kind = NavigationKind.General,
            Degrees = degrees,
            Label = LocalizationService.Text("general_direction", "Direção geral")
        };

        private static Transform? FindTransition(string doorName)
        {
            Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            foreach (GameObject root in activeScene.GetRootGameObjects())
            {
                Transform[] children = root.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in children)
                    if (child.name == doorName) return child;
            }
            return null;
        }
    }
}

