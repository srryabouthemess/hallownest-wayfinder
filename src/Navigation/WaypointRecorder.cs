using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace HallownestWayfinder
{
    public sealed class WaypointDoorCandidate
    {
        public string Name { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
    }

    public sealed class WaypointCapture
    {
        public bool Success { get; set; }
        public string Scene { get; set; } = string.Empty;
        public string StepId { get; set; } = string.Empty;
        public string Json { get; set; } = string.Empty;
        public string? TargetObjectName { get; set; }
        public int PointCount { get; set; }
    }

    public static class WaypointRecorder
    {
        private const float DoorDetectionRadius = 6f;
        private static readonly Dictionary<string, List<NavigationWaypoint>> Recordings =
            new Dictionary<string, List<NavigationWaypoint>>(StringComparer.Ordinal);

        public static void Reset() => Recordings.Clear();

        public static WaypointCapture Capture(RoutePlan? route, RouteStep? step)
        {
            GameManager? game = GameManager.instance;
            HeroController? hero = HeroController.instance;
            if (route == null || step == null || game == null || hero == null)
                return new WaypointCapture();

            string scene = game.sceneName ?? string.Empty;
            if (scene.Length == 0) return new WaypointCapture();

            Vector3 position = hero.transform.position;
            string? door = RouteNavigation.FindNearestDoorName(
                scene, position, DoorDetectionRadius);
            string key = route.Id + "\n" + step.Id + "\n" + scene;
            if (!Recordings.TryGetValue(key, out List<NavigationWaypoint>? points))
                Recordings[key] = points = new List<NavigationWaypoint>();

            NavigationWaypoint waypoint = CreateWaypoint(scene, position.x, position.y,
                points.Count, door, 2.5f);
            points.Add(waypoint);
            string json = SerializeSnippet(points);
            GUIUtility.systemCopyBuffer = json;

            return new WaypointCapture
            {
                Success = true,
                Scene = scene,
                StepId = step.Id,
                Json = json,
                TargetObjectName = door,
                PointCount = points.Count
            };
        }

        public static NavigationWaypoint CreateWaypoint(string scene, float x, float y,
            int order, string? targetObjectName, float arrivalRadius) =>
            new NavigationWaypoint
            {
                Scene = scene,
                X = Round(x),
                Y = Round(y),
                Order = Math.Max(0, order),
                ArrivalRadius = Math.Max(0.5f, Round(arrivalRadius)),
                TargetObjectName = string.IsNullOrEmpty(targetObjectName)
                    ? null
                    : targetObjectName
            };

        public static string SerializeSnippet(IReadOnlyList<NavigationWaypoint> waypoints)
        {
            JArray navigation = new JArray();
            if (waypoints != null)
            {
                foreach (NavigationWaypoint waypoint in waypoints)
                {
                    if (waypoint == null) continue;
                    JObject point = new JObject
                    {
                        ["Scene"] = waypoint.Scene
                    };
                    if (string.IsNullOrEmpty(waypoint.TargetObjectName))
                    {
                        point["X"] = Round(waypoint.X);
                        point["Y"] = Round(waypoint.Y);
                    }
                    else
                    {
                        point["TargetObjectName"] = waypoint.TargetObjectName;
                    }
                    point["Order"] = waypoint.Order;
                    point["ArrivalRadius"] = Round(waypoint.ArrivalRadius);
                    if (!string.IsNullOrEmpty(waypoint.Label)) point["Label"] = waypoint.Label;
                    navigation.Add(point);
                }
            }

            return new JObject { ["Navigation"] = navigation }
                .ToString(Formatting.Indented);
        }

        public static string? SelectNearestDoor(IEnumerable<WaypointDoorCandidate> candidates,
            float x, float y, float maximumDistance)
        {
            if (candidates == null || maximumDistance <= 0f) return null;
            string? closest = null;
            float closestSquared = maximumDistance * maximumDistance;
            foreach (WaypointDoorCandidate candidate in candidates)
            {
                if (candidate == null || string.IsNullOrEmpty(candidate.Name)) continue;
                float deltaX = candidate.X - x;
                float deltaY = candidate.Y - y;
                float squared = deltaX * deltaX + deltaY * deltaY;
                if (squared > closestSquared) continue;
                closest = candidate.Name;
                closestSquared = squared;
            }
            return closest;
        }

        private static float Round(float value) =>
            (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
