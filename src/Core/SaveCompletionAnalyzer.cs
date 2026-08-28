using System.Collections.Generic;

namespace HallownestWayfinder
{
    public static class SaveCompletionAnalyzer
    {
        public static int FindNextStep(IReadOnlyList<RouteStep> steps, IList<string> dismissed)
        {
            if (!PlayerDataGameState.TryCapture(out PlayerDataGameState? state) || state == null)
                return -1;
            return FindNextStep(steps, dismissed, state);
        }

        public static int FindNextStep(IReadOnlyList<RouteStep> steps, IList<string> dismissed,
            IGameState state)
        {
            if (steps == null) return -1;

            int firstIncomplete = -1;
            int firstDismissedIncomplete = -1;
            for (int index = 0; index < steps.Count; index++)
            {
                RouteStep step = steps[index];
                if (step == null || IsComplete(step, state)) continue;
                if (Contains(dismissed, step.Id))
                {
                    if (firstDismissedIncomplete < 0) firstDismissedIncomplete = index;
                    continue;
                }
                if (firstIncomplete < 0) firstIncomplete = index;
                if (IsAvailable(step, state)) return index;
            }

            // If every remaining item has an unknown prerequisite, still show
            // the earliest one and mark it as blocked instead of hiding the HUD.
            return firstIncomplete >= 0 ? firstIncomplete : firstDismissedIncomplete;
        }

        public static int CountCompleted(IReadOnlyList<RouteStep> steps)
        {
            if (!PlayerDataGameState.TryCapture(out PlayerDataGameState? state) || state == null)
                return 0;
            return CountCompleted(steps, state);
        }

        public static int CountCompleted(IReadOnlyList<RouteStep> steps, IGameState state)
        {
            if (steps == null) return 0;

            int completed = 0;
            foreach (RouteStep step in steps)
                if (step != null && IsComplete(step, state)) completed++;
            return completed;
        }

        public static bool IsAvailable(RouteStep step)
        {
            if (!PlayerDataGameState.TryCapture(out PlayerDataGameState? state) || state == null)
                return false;
            return IsAvailable(step, state);
        }

        public static bool IsAvailable(RouteStep step, IGameState state) =>
            step != null && step.ArePrerequisitesSatisfied(state);

        private static bool IsComplete(RouteStep step, IGameState state)
        {
            try { return step.IsComplete(state); }
            catch { return false; }
        }

        private static bool Contains(IList<string> values, string value)
        {
            if (values == null) return false;
            for (int i = 0; i < values.Count; i++)
                if (values[i] == value) return true;
            return false;
        }

    }
}
