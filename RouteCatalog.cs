using System.Collections.Generic;

namespace HallownestWayfinder
{
    public static class RouteCatalog
    {
        public static readonly IReadOnlyList<RoutePlan> Routes = new List<RoutePlan>
        {
            new RoutePlan
            {
                Id = "completion_112",
                Name = RouteDefinition.Name,
                Steps = RouteDefinition.Steps
            },
            new RoutePlan
            {
                Id = "speedrun_5h",
                Name = SpeedrunRouteDefinition.Name,
                Steps = SpeedrunRouteDefinition.Steps
            },
            new RoutePlan
            {
                Id = "grubs_46",
                Name = GrubRouteDefinition.Name,
                Steps = GrubRouteDefinition.Steps
            }
        };
    }
}
