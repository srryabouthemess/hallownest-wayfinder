using System.Collections.Generic;

namespace HallownestWayfinder
{
    public static class SaveCompletionDefinition
    {
        public const string Name = "Completar save";

        public static readonly IReadOnlyList<RouteStep> Steps = BuildSteps();

        private static IReadOnlyList<RouteStep> BuildSteps()
        {
            List<RouteStep> result = new List<RouteStep>();
            foreach (RouteStep step in RouteDefinition.Steps)
            {
                if (step.Id == "c25_grubfather")
                    result.AddRange(GrubRouteDefinition.Steps);

                if (ShouldInclude(step)) result.Add(step);
            }
            return result;
        }

        private static bool ShouldInclude(RouteStep step)
        {
            if (step == null || step.Optional) return false;

            // These values can become false again after using an item and are
            // therefore not reliable evidence when inspecting an existing save.
            if (step.RequiredPlayerBool == "atBench" ||
                step.RequiredPlayerInt == "trinket1" ||
                step.RequiredPlayerInt == "ore" ||
                step.RequiredPlayerInt == "grubsCollected")
            {
                return false;
            }

            // Individual grub records provide a precise answer and replace the
            // aggregate grub milestones from the original walkthrough.
            if (!string.IsNullOrEmpty(step.RequiredGrubScene)) return false;

            // Entering a room is useful during a guided run but is not permanent
            // save progress. Visited-scene checks remain eligible.
            bool hasPersistentCondition =
                !string.IsNullOrEmpty(step.RequiredPlayerBool) ||
                HasValues(step.RequiredAllPlayerBools) ||
                HasValues(step.RequiredAnyPlayerBools) ||
                !string.IsNullOrEmpty(step.RequiredPlayerInt) ||
                HasValues(step.RequiredPlayerIntSum) ||
                !string.IsNullOrEmpty(step.RequiredVisitedScene) ||
                !string.IsNullOrEmpty(step.RequiredBenchScene) ||
                step.RequireNoRelics ||
                step.RequiredPantheonCount > 0;

            return hasPersistentCondition;
        }

        private static bool HasValues(string[] values) => values != null && values.Length > 0;
    }
}
