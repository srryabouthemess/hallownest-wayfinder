using System.Collections.Generic;

namespace HallownestWayfinder
{
    public sealed class RouteProgress
    {
        public int DataVersion { get; set; } = 4;
        public int CurrentStep { get; set; }
        public int SpeedrunCurrentStep { get; set; }
        public int GrubCurrentStep { get; set; }
        public List<string> SaveCompletionDismissedStepIds { get; set; } = new List<string>();
        public bool Visible { get; set; } = true;
    }
}

