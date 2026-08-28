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

        [Newtonsoft.Json.JsonIgnore]
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
            return PlayerDataGameState.TryCapture(out PlayerDataGameState? state) &&
                state != null && IsComplete(state);
        }

        public bool IsComplete(IGameState state)
        {
            string? playerBool = PlayerBool;
            if (!string.IsNullOrEmpty(playerBool) && !state.GetBool(playerBool!))
            {
                return false;
            }

            string[]? allPlayerBools = AllPlayerBools;
            if (allPlayerBools != null && allPlayerBools.Length > 0)
            {
                foreach (string field in allPlayerBools)
                    if (!state.GetBool(field)) return false;
            }

            string[]? anyPlayerBools = AnyPlayerBools;
            if (anyPlayerBools != null && anyPlayerBools.Length > 0)
            {
                bool any = false;
                foreach (string field in anyPlayerBools)
                    if (state.GetBool(field)) any = true;
                if (!any) return false;
            }

            string? scene = Scene;
            if (!string.IsNullOrEmpty(scene) &&
                state.SceneName != scene && !state.HasVisitedScene(scene!))
            {
                return false;
            }

            string[]? playerIntSum = PlayerIntSum;
            if (playerIntSum != null && playerIntSum.Length > 0)
            {
                int sum = 0;
                foreach (string field in playerIntSum)
                    sum += state.GetInt(field);
                if (sum < PlayerIntSumMinimum) return false;
            }

            string? visitedScene = VisitedScene;
            if (!string.IsNullOrEmpty(visitedScene) &&
                state.SceneName != visitedScene && !state.HasVisitedScene(visitedScene!))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(BenchScene))
            {
                bool sittingThere = state.SceneName == BenchScene && state.AtBench;
                bool savedThere = state.RespawnScene == BenchScene;
                if (!sittingThere && !savedThere) return false;
            }

            if (NoRelics && state.RelicCount > 0)
            {
                return false;
            }

            if (PantheonCount > 0 && state.CompletedPantheons < PantheonCount)
                return false;

            string? playerInt = PlayerInt;
            if (!string.IsNullOrEmpty(playerInt) && state.GetInt(playerInt!) < Minimum)
            {
                return false;
            }

            string? grubScene = GrubScene;
            if (!string.IsNullOrEmpty(grubScene) && !IsGrubRescued(state))
                return false;

            return IsTracked;
        }

        public bool IsGrubRescued()
        {
            return PlayerDataGameState.TryCapture(out PlayerDataGameState? state) &&
                state != null && IsGrubRescued(state);
        }

        public bool IsGrubRescued(IGameState state)
        {
            string? grubScene = GrubScene;
            if (string.IsNullOrEmpty(grubScene) || !state.HasRescuedGrub(grubScene!)) return false;

            if (GrubCountInScene <= 1) return true;

            int rescuedInSharedScenes = state.GrubsCollected - state.GrubSceneCount + 1;
            return rescuedInSharedScenes >= GrubCountInScene;
        }

        private static bool HasValues(string[]? values) => values != null && values.Length > 0;
    }
}
