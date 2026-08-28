namespace HallownestWayfinder
{
    /// <summary>
    /// A waypoint inside a room. Coordinates use HeroController's world space.
    /// Multiple points in one room can be ordered into a short internal path.
    /// </summary>
    public sealed class NavigationWaypoint
    {
        public string Scene { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Order { get; set; }
        public float ArrivalRadius { get; set; } = 2.5f;
        public string? Label { get; set; }
        public string? TargetObjectName { get; set; }
    }
}

