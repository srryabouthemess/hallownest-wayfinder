namespace HallownestWayfinder
{
    public sealed class RouteProgress
    {
        public int DataVersion { get; set; } = 3;
        public int CurrentStep { get; set; }
        public int SpeedrunCurrentStep { get; set; }
        public bool Visible { get; set; } = true;
    }
}

