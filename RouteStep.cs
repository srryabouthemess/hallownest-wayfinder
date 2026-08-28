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
        public string RequiredGrubScene { get; set; }
        public int RequiredGrubCountInScene { get; set; } = 1;
        public NavigationWaypoint[] Navigation { get; set; }
        public string TransportInstruction { get; set; }
        public string TargetScene { get; set; }

        public bool IsAutomaticallyTracked =>
            !string.IsNullOrEmpty(RequiredPlayerBool) ||
            !string.IsNullOrEmpty(RequiredPlayerInt) ||
            !string.IsNullOrEmpty(RequiredScene) ||
            !string.IsNullOrEmpty(RequiredGrubScene);

        public string GetTargetScene()
        {
            if (!string.IsNullOrEmpty(RequiredGrubScene) && !IsGrubRescued())
                return RequiredGrubScene;

            return string.IsNullOrEmpty(TargetScene) ? RequiredScene : TargetScene;
        }

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

            if (!string.IsNullOrEmpty(RequiredGrubScene) && !IsGrubRescued())
                return false;

            return IsAutomaticallyTracked;
        }

        private bool IsGrubRescued()
        {
            if (PlayerData.instance == null || PlayerData.instance.scenesGrubRescued == null ||
                !PlayerData.instance.scenesGrubRescued.Contains(RequiredGrubScene))
            {
                return false;
            }

            if (RequiredGrubCountInScene <= 1) return true;

            // The three Collector grubs share one scene entry. The difference
            // between the total and the unique rescued-scene count reveals how
            // many additional grubs were rescued in that same room.
            int rescuedInSharedScenes = PlayerData.instance.grubsCollected -
                PlayerData.instance.scenesGrubRescued.Count + 1;
            return rescuedInSharedScenes >= RequiredGrubCountInScene;
        }
    }
}

