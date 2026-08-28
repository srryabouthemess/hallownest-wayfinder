namespace HallownestWayfinder
{
    public sealed class RouteCompletion
    {
        public string? PlayerBool { get; set; }
        public string[]? AllPlayerBools { get; set; }
        public string[]? AnyPlayerBools { get; set; }
        public string? PlayerInt { get; set; }
        public int Minimum { get; set; }
        public string[]? PlayerIntSum { get; set; }
        public int PlayerIntSumMinimum { get; set; }
        public string? Scene { get; set; }
        public string? VisitedScene { get; set; }
        public string? BenchScene { get; set; }
        public bool NoRelics { get; set; }
        public int PantheonCount { get; set; }
        public string? GrubScene { get; set; }
        public int GrubCountInScene { get; set; } = 1;

        public bool IsTracked =>
            !string.IsNullOrEmpty(PlayerBool) ||
            HasValues(AllPlayerBools) ||
            HasValues(AnyPlayerBools) ||
            !string.IsNullOrEmpty(PlayerInt) ||
            HasValues(PlayerIntSum) ||
            !string.IsNullOrEmpty(Scene) ||
            !string.IsNullOrEmpty(VisitedScene) ||
            !string.IsNullOrEmpty(BenchScene) ||
            NoRelics ||
            PantheonCount > 0 ||
            !string.IsNullOrEmpty(GrubScene);

        public bool IsComplete()
        {
            if (!string.IsNullOrEmpty(PlayerBool) &&
                !PlayerData.instance.GetBool(PlayerBool))
            {
                return false;
            }

            string[]? allPlayerBools = AllPlayerBools;
            if (allPlayerBools != null && allPlayerBools.Length > 0)
            {
                foreach (string field in allPlayerBools)
                    if (!PlayerData.instance.GetBool(field)) return false;
            }

            string[]? anyPlayerBools = AnyPlayerBools;
            if (anyPlayerBools != null && anyPlayerBools.Length > 0)
            {
                bool any = false;
                foreach (string field in anyPlayerBools)
                    if (PlayerData.instance.GetBool(field)) any = true;
                if (!any) return false;
            }

            if (!string.IsNullOrEmpty(Scene) &&
                GameManager.instance.sceneName != Scene &&
                (PlayerData.instance.scenesVisited == null ||
                 !PlayerData.instance.scenesVisited.Contains(Scene)))
            {
                return false;
            }

            string[]? playerIntSum = PlayerIntSum;
            if (playerIntSum != null && playerIntSum.Length > 0)
            {
                int sum = 0;
                foreach (string field in playerIntSum)
                    sum += PlayerData.instance.GetInt(field);
                if (sum < PlayerIntSumMinimum) return false;
            }

            if (!string.IsNullOrEmpty(VisitedScene) &&
                GameManager.instance.sceneName != VisitedScene &&
                (PlayerData.instance.scenesVisited == null ||
                 !PlayerData.instance.scenesVisited.Contains(VisitedScene)))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(BenchScene))
            {
                bool sittingThere = GameManager.instance.sceneName == BenchScene &&
                    PlayerData.instance.atBench;
                bool savedThere = PlayerData.instance.respawnScene == BenchScene;
                if (!sittingThere && !savedThere) return false;
            }

            if (NoRelics &&
                PlayerData.instance.trinket1 + PlayerData.instance.trinket2 +
                PlayerData.instance.trinket3 + PlayerData.instance.trinket4 > 0)
            {
                return false;
            }

            if (PantheonCount > 0 && CompletedPantheons() < PantheonCount)
                return false;

            if (!string.IsNullOrEmpty(PlayerInt) &&
                PlayerData.instance.GetInt(PlayerInt) < Minimum)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(GrubScene) && !IsGrubRescued())
                return false;

            return IsTracked;
        }

        public bool IsGrubRescued()
        {
            if (PlayerData.instance == null || PlayerData.instance.scenesGrubRescued == null ||
                !PlayerData.instance.scenesGrubRescued.Contains(GrubScene))
            {
                return false;
            }

            if (GrubCountInScene <= 1) return true;

            int rescuedInSharedScenes = PlayerData.instance.grubsCollected -
                PlayerData.instance.scenesGrubRescued.Count + 1;
            return rescuedInSharedScenes >= GrubCountInScene;
        }

        private static bool HasValues(string[]? values) => values != null && values.Length > 0;

        private static int CompletedPantheons()
        {
            int completed = 0;
            if (PlayerData.instance.bossDoorStateTier1.completed) completed++;
            if (PlayerData.instance.bossDoorStateTier2.completed) completed++;
            if (PlayerData.instance.bossDoorStateTier3.completed) completed++;
            if (PlayerData.instance.bossDoorStateTier4.completed) completed++;
            return completed;
        }
    }
}
