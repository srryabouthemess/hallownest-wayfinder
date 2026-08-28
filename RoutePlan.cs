using System.Collections.Generic;

namespace HallownestWayfinder
{
    public sealed class RoutePlan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IReadOnlyList<RouteStep> Steps { get; set; }
    }
}
