using System;
using System.Collections.Generic;

namespace HallownestWayfinder
{
    public sealed class RoutePlan
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public IReadOnlyList<RouteStep> Steps { get; set; } = Array.Empty<RouteStep>();
        public bool IsSaveCompletion { get; set; }
    }
}
