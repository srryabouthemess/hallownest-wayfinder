using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HallownestWayfinder
{
    /// <summary>
    /// Grafo vanilla independente. Ele guarda apenas fatos de conectividade
    /// entre salas; o algoritmo de busca foi escrito para o HallownestWayfinder.
    /// </summary>
    public static class VanillaRouteGraph
    {
        private sealed class Edge
        {
            public string Door = string.Empty;
            public string TargetScene = string.Empty;
            public string? RequiredPlayerBool;
        }

        private static readonly Dictionary<string, List<Edge>> Edges = Load();
        private static readonly string[] RequiredFields = CollectRequiredFields();
        private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();

        public static bool TryGetNextDoor(string? fromScene, string? targetScene, out string? door)
        {
            door = null;
            if (fromScene == null || fromScene.Length == 0 ||
                targetScene == null || targetScene.Length == 0 ||
                fromScene == targetScene)
                return false;

            string cacheKey = fromScene + "\n" + targetScene + "\n" + AccessSignature();
            if (Cache.TryGetValue(cacheKey, out string cachedDoor))
            {
                door = cachedDoor;
                return !string.IsNullOrEmpty(door);
            }

            Queue<string> queue = new Queue<string>();
            HashSet<string> visited = new HashSet<string>(StringComparer.Ordinal) { fromScene };
            Dictionary<string, Tuple<string, string>> previous = new Dictionary<string, Tuple<string, string>>();
            queue.Enqueue(fromScene);

            while (queue.Count > 0)
            {
                string scene = queue.Dequeue();
                if (!Edges.TryGetValue(scene, out List<Edge>? outgoing)) continue;

                foreach (Edge edge in outgoing)
                {
                    if (!IsAvailable(edge)) continue;
                    if (!visited.Add(edge.TargetScene)) continue;
                    previous[edge.TargetScene] = Tuple.Create(scene, edge.Door);
                    if (edge.TargetScene == targetScene)
                    {
                        string cursor = targetScene;
                        string firstDoor = edge.Door;
                        while (previous.TryGetValue(cursor, out Tuple<string, string>? step))
                        {
                            firstDoor = step.Item2;
                            cursor = step.Item1;
                            if (cursor == fromScene) break;
                        }
                        Cache[cacheKey] = firstDoor;
                        door = firstDoor;
                        return true;
                    }
                    queue.Enqueue(edge.TargetScene);
                }
            }

            Cache[cacheKey] = string.Empty;
            return false;
        }

        private static Dictionary<string, List<Edge>> Load()
        {
            Dictionary<string, List<Edge>> result = new Dictionary<string, List<Edge>>(StringComparer.Ordinal);
            Assembly assembly = Assembly.GetExecutingAssembly();
            string? resource = null;
            foreach (string name in assembly.GetManifestResourceNames())
                if (name.EndsWith("vanilla_transitions.txt", StringComparison.OrdinalIgnoreCase)) resource = name;

            if (resource == null) return result;
            using (Stream? stream = assembly.GetManifestResourceStream(resource))
            {
                if (stream == null) return result;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line[0] == '#') continue;
                        string[] parts = line.Split('|');
                        if (parts.Length != 5) continue;
                        if (!result.TryGetValue(parts[0], out List<Edge>? list))
                            result[parts[0]] = list = new List<Edge>();
                        list.Add(new Edge
                        {
                            Door = parts[1],
                            TargetScene = parts[2],
                            RequiredPlayerBool = parts[4].Length > 0
                                ? parts[4]
                                : null
                        });
                    }
                }
            }
            return result;
        }

        public static IReadOnlyList<string> ValidateRequirements()
        {
            List<string> errors = new List<string>();
            foreach (string field in RequiredFields)
            {
                FieldInfo? playerField = typeof(PlayerData).GetField(field,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (playerField?.FieldType != typeof(bool))
                    errors.Add("PlayerData." + field + " não é um campo booleano válido.");
            }
            return errors;
        }

        private static bool IsAvailable(Edge edge)
        {
            string? field = edge.RequiredPlayerBool;
            return field == null ||
                (PlayerData.instance != null && PlayerData.instance.GetBool(field));
        }

        private static string AccessSignature()
        {
            if (PlayerData.instance == null) return "no-save";
            char[] signature = new char[RequiredFields.Length];
            for (int index = 0; index < RequiredFields.Length; index++)
                signature[index] = PlayerData.instance.GetBool(RequiredFields[index]) ? '1' : '0';
            return new string(signature);
        }

        private static string[] CollectRequiredFields()
        {
            HashSet<string> fields = new HashSet<string>(StringComparer.Ordinal);
            foreach (List<Edge> edges in Edges.Values)
                foreach (Edge edge in edges)
                    if (edge.RequiredPlayerBool != null) fields.Add(edge.RequiredPlayerBool);

            string[] result = new string[fields.Count];
            fields.CopyTo(result);
            Array.Sort(result, StringComparer.Ordinal);
            return result;
        }
    }
}

