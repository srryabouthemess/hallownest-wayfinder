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
            if (step == null || step.NotRequiredFor112) return false;

            // These values can become false again after using an item and are
            // therefore not reliable evidence when inspecting an existing save.
            RouteCompletion completion = step.Completion;
            if (completion.PlayerBool == "atBench" ||
                completion.PlayerInt == "trinket1" ||
                completion.PlayerInt == "ore" ||
                completion.PlayerInt == "grubsCollected")
            {
                return false;
            }

            // Individual grub records provide a precise answer and replace the
            // aggregate grub milestones from the original walkthrough.
            if (!string.IsNullOrEmpty(completion.GrubScene)) return false;

            // Entering a room is useful during a guided run but is not permanent
            // save progress. Visited-scene checks remain eligible.
            bool hasPersistentCondition =
                !string.IsNullOrEmpty(completion.PlayerBool) ||
                HasValues(completion.AllPlayerBools) ||
                HasValues(completion.AnyPlayerBools) ||
                !string.IsNullOrEmpty(completion.PlayerInt) ||
                HasValues(completion.PlayerIntSum) ||
                !string.IsNullOrEmpty(completion.VisitedScene) ||
                !string.IsNullOrEmpty(completion.BenchScene) ||
                completion.NoRelics ||
                completion.PantheonCount > 0;

            return hasPersistentCondition;
        }

        private static bool HasValues(string[]? values) => values != null && values.Length > 0;
    }
}
