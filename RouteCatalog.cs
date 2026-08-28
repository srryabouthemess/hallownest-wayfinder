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
                EnglishName = "112% Route",
                Steps = RouteDefinition.Steps
            },
            new RoutePlan
            {
                Id = "speedrun_5h",
                Name = SpeedrunRouteDefinition.Name,
                EnglishName = "5h Speedrun • Glitchless",
                Steps = SpeedrunRouteDefinition.Steps
            },
            new RoutePlan
            {
                Id = "grubs_46",
                Name = GrubRouteDefinition.Name,
                EnglishName = "Grubs 46/46",
                Steps = GrubRouteDefinition.Steps
            }
        };
    }
}
