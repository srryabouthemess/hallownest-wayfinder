namespace HallownestWayfinder
{
    public sealed class RouteProgress
    {
        public int DataVersion { get; set; } = 2;
        public int CurrentStep { get; set; }
        public bool Visible { get; set; } = true;
    }
}

