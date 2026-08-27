namespace HallownestWayfinder
{
    public sealed class RouteStep
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Hint { get; set; }
        public string Icon { get; set; }
        public float ArrowDegrees { get; set; }
        public bool Optional { get; set; }
        public string RequiredPlayerBool { get; set; }
        public string RequiredPlayerInt { get; set; }
        public int RequiredMinimum { get; set; }
        public string RequiredScene { get; set; }
        public NavigationWaypoint[] Navigation { get; set; }
        public string TransportInstruction { get; set; }
        public string TargetScene { get; set; }

        public bool IsAutomaticallyTracked =>
            !string.IsNullOrEmpty(RequiredPlayerBool) ||
            !string.IsNullOrEmpty(RequiredPlayerInt) ||
            !string.IsNullOrEmpty(RequiredScene);

        public bool IsComplete()
        {
            if (!string.IsNullOrEmpty(RequiredPlayerBool) &&
                !PlayerData.instance.GetBool(RequiredPlayerBool))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(RequiredScene) &&
                GameManager.instance.sceneName != RequiredScene)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(RequiredPlayerInt) &&
                PlayerData.instance.GetInt(RequiredPlayerInt) < RequiredMinimum)
            {
                return false;
            }

            return IsAutomaticallyTracked;
        }
    }
}

