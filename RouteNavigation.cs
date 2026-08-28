using System;
using System.Linq;
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
        public string Label { get; set; }
        public bool ShowArrow => Kind == NavigationKind.Precise || Kind == NavigationKind.General;
    }

    public static class RouteNavigation
    {
        public static NavigationResult Resolve(RouteStep step, bool intelligent)
        {
            if (step == null)
                return General(0f);

            if (!intelligent)
                return General(step.ArrowDegrees);

            string scene = GameManager.instance == null ? null : GameManager.instance.sceneName;
            Vector3 hero = HeroController.instance == null
                ? Vector3.zero
                : HeroController.instance.transform.position;

            NavigationWaypoint[] points = step.Navigation == null
                ? Array.Empty<NavigationWaypoint>()
                : step.Navigation.Where(point => point != null && point.Scene == scene)
                    .OrderBy(point => point.Order).ToArray();

            foreach (NavigationWaypoint point in points)
            {
                Vector2 delta = new Vector2(point.X - hero.x, point.Y - hero.y);
                if (delta.magnitude <= Mathf.Max(0.5f, point.ArrivalRadius))
                    continue;

                // A textura original da seta aponta para cima (0 graus).
                float degrees = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;
                return new NavigationResult
                {
                    Kind = NavigationKind.Precise,
                    Degrees = degrees,
                    Label = string.IsNullOrEmpty(point.Label)
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

            string destinationScene = step.GetTargetScene();
            string transportInstruction = LocalizationService.StepTransport(step);
            if (!string.IsNullOrEmpty(destinationScene))
            {
                if (scene == destinationScene)
                {
                    if (!string.IsNullOrEmpty(transportInstruction))
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

                if (VanillaRouteGraph.TryGetNextDoor(scene, destinationScene, out string door))
                {
                    Transform exit = FindTransition(door);
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

            if (!string.IsNullOrEmpty(transportInstruction))
            {
                return new NavigationResult
                {
                    Kind = NavigationKind.Transport,
                    Label = transportInstruction
                };
            }

            return new NavigationResult
            {
                Kind = NavigationKind.Unmapped,
                Label = LocalizationService.Text("section_unmapped", "Trecho ainda não mapeado")
            };
        }

        private static NavigationResult General(float degrees) => new NavigationResult
        {
            Kind = NavigationKind.General,
            Degrees = degrees,
            Label = LocalizationService.Text("general_direction", "Direção geral")
        };

        private static Transform FindTransition(string doorName)
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

