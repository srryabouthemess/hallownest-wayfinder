using System.Collections.Generic;

namespace HallownestWayfinder
{
    public sealed class RouteProgress
    {
        public int DataVersion { get; set; } = 5;
        public Dictionary<string, int> StepByRoute { get; set; } =
            new Dictionary<string, int>(System.StringComparer.Ordinal);

        // Kept for one migration cycle so existing version-4 saves retain progress.
        public int CurrentStep { get; set; }
        public int SpeedrunCurrentStep { get; set; }
        public int GrubCurrentStep { get; set; }
        public List<string> SaveCompletionDismissedStepIds { get; set; } = new List<string>();
        public bool Visible { get; set; } = true;

        public void MigrateRouteDictionary()
        {
            if (StepByRoute == null)
                StepByRoute = new Dictionary<string, int>(System.StringComparer.Ordinal);
            if (DataVersion >= 5) return;

            StepByRoute["completion_112"] = CurrentStep;
            StepByRoute["speedrun_5h"] = SpeedrunCurrentStep;
            StepByRoute["grubs_46"] = GrubCurrentStep;
            DataVersion = 5;
        }
    }
}

