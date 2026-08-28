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
        public string[] RequiredAllPlayerBools { get; set; }
        public string[] RequiredAnyPlayerBools { get; set; }
        public string RequiredPlayerInt { get; set; }
        public int RequiredMinimum { get; set; }
        public string[] RequiredPlayerIntSum { get; set; }
        public int RequiredPlayerIntSumMinimum { get; set; }
        public string RequiredScene { get; set; }
        public string RequiredVisitedScene { get; set; }
        public string RequiredBenchScene { get; set; }
        public bool RequireNoRelics { get; set; }
        public int RequiredPantheonCount { get; set; }
        public string RequiredGrubScene { get; set; }
        public int RequiredGrubCountInScene { get; set; } = 1;
        public NavigationWaypoint[] Navigation { get; set; }
        public string TransportInstruction { get; set; }
        public string TargetScene { get; set; }

        public bool IsAutomaticallyTracked =>
            !string.IsNullOrEmpty(RequiredPlayerBool) ||
            HasValues(RequiredAllPlayerBools) ||
            HasValues(RequiredAnyPlayerBools) ||
            !string.IsNullOrEmpty(RequiredPlayerInt) ||
            HasValues(RequiredPlayerIntSum) ||
            !string.IsNullOrEmpty(RequiredScene) ||
            !string.IsNullOrEmpty(RequiredVisitedScene) ||
            !string.IsNullOrEmpty(RequiredBenchScene) ||
            RequireNoRelics ||
            RequiredPantheonCount > 0 ||
            !string.IsNullOrEmpty(RequiredGrubScene);

        public string GetTargetScene()
        {
            if (!string.IsNullOrEmpty(RequiredGrubScene) && !IsGrubRescued())
                return RequiredGrubScene;

            if (!string.IsNullOrEmpty(TargetScene)) return TargetScene;
            if (!string.IsNullOrEmpty(RequiredScene)) return RequiredScene;
            if (!string.IsNullOrEmpty(RequiredBenchScene)) return RequiredBenchScene;
            return RequiredVisitedScene;
        }

        public bool IsComplete()
        {
            if (!string.IsNullOrEmpty(RequiredPlayerBool) &&
                !PlayerData.instance.GetBool(RequiredPlayerBool))
            {
                return false;
            }

            if (HasValues(RequiredAllPlayerBools))
            {
                foreach (string field in RequiredAllPlayerBools)
                    if (!PlayerData.instance.GetBool(field)) return false;
            }

            if (HasValues(RequiredAnyPlayerBools))
            {
                bool any = false;
                foreach (string field in RequiredAnyPlayerBools)
                    if (PlayerData.instance.GetBool(field)) any = true;
                if (!any) return false;
            }

            if (!string.IsNullOrEmpty(RequiredScene) &&
                GameManager.instance.sceneName != RequiredScene)
            {
                return false;
            }

            if (HasValues(RequiredPlayerIntSum))
            {
                int sum = 0;
                foreach (string field in RequiredPlayerIntSum)
                    sum += PlayerData.instance.GetInt(field);
                if (sum < RequiredPlayerIntSumMinimum) return false;
            }

            if (!string.IsNullOrEmpty(RequiredVisitedScene) &&
                GameManager.instance.sceneName != RequiredVisitedScene &&
                (PlayerData.instance.scenesVisited == null ||
                 !PlayerData.instance.scenesVisited.Contains(RequiredVisitedScene)))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(RequiredBenchScene))
            {
                bool recorded = PlayerData.instance.scenesEncounteredBench != null &&
                    PlayerData.instance.scenesEncounteredBench.Contains(RequiredBenchScene);
                bool sittingThere = GameManager.instance.sceneName == RequiredBenchScene &&
                    PlayerData.instance.atBench;
                if (!recorded && !sittingThere) return false;
            }

            if (RequireNoRelics &&
                PlayerData.instance.trinket1 + PlayerData.instance.trinket2 +
                PlayerData.instance.trinket3 + PlayerData.instance.trinket4 > 0)
            {
                return false;
            }

            if (RequiredPantheonCount > 0 && CompletedPantheons() < RequiredPantheonCount)
                return false;

            if (!string.IsNullOrEmpty(RequiredPlayerInt) &&
                PlayerData.instance.GetInt(RequiredPlayerInt) < RequiredMinimum)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(RequiredGrubScene) && !IsGrubRescued())
                return false;

            return IsAutomaticallyTracked;
        }

        private static bool HasValues(string[] values) => values != null && values.Length > 0;

        private static int CompletedPantheons()
        {
            int completed = 0;
            if (PlayerData.instance.bossDoorStateTier1.completed) completed++;
            if (PlayerData.instance.bossDoorStateTier2.completed) completed++;
            if (PlayerData.instance.bossDoorStateTier3.completed) completed++;
            if (PlayerData.instance.bossDoorStateTier4.completed) completed++;
            return completed;
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

