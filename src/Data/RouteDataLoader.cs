using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace HallownestWayfinder
{
    public static class RouteDataLoader
    {
        private const string ResourceName = "HallownestWayfinder.Assets.routes.json";

        public static IReadOnlyList<RoutePlan> Load()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException("Route resource not found: " + ResourceName);
                using (StreamReader reader = new StreamReader(stream))
                    return LoadFromJson(reader.ReadToEnd());
            }
        }

        public static IReadOnlyList<RoutePlan> LoadFromJson(string json)
        {
            List<RoutePlan>? routes = JsonConvert.DeserializeObject<List<RoutePlan>>(json);
            if (routes == null || routes.Count == 0)
                throw new InvalidDataException("The route file does not contain any routes.");
            return routes;
        }
    }
}
