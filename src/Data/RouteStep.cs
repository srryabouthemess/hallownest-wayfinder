namespace HallownestWayfinder
{
    public sealed class RouteStep
    {
        public string Id { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public float ArrowDegrees { get; set; }
        public bool SkippableInRoute { get; set; }
        public bool NotRequiredFor112 { get; set; }
        public RouteCompletion Completion { get; set; } = new RouteCompletion();
        // Any complete alternative unlocks the step; every condition inside
        // an alternative must be satisfied.
        public PlayerDataPrerequisite[][]? Prerequisites { get; set; }
        public NavigationWaypoint[]? Navigation { get; set; }
        public string? TransportScene { get; set; }
        public string? TransportInstruction { get; set; }
        public string? TargetScene { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public bool IsAutomaticallyTracked => Completion.IsTracked;

        public string? GetTargetScene()
        {
            if (PlayerDataGameState.TryCapture(out PlayerDataGameState? state) && state != null)
                return GetTargetScene(state);
            return GetStaticTargetScene();
        }

        public string? GetTargetScene(IGameState state)
        {
            if (!string.IsNullOrEmpty(Completion.GrubScene) && !Completion.IsGrubRescued(state))
                return Completion.GrubScene;

            return GetStaticTargetScene();
        }

        private string? GetStaticTargetScene()
        {
            if (!string.IsNullOrEmpty(TransportScene)) return TransportScene;
            if (!string.IsNullOrEmpty(TargetScene)) return TargetScene;
            if (!string.IsNullOrEmpty(Completion.Scene)) return Completion.Scene;
            if (!string.IsNullOrEmpty(Completion.BenchScene)) return Completion.BenchScene;
            return Completion.VisitedScene;
        }

        public bool IsComplete() => Completion.IsComplete();
        public bool IsComplete(IGameState state) => Completion.IsComplete(state);

        public bool ArePrerequisitesSatisfied()
        {
            return PlayerDataGameState.TryCapture(out PlayerDataGameState? state) &&
                state != null && ArePrerequisitesSatisfied(state);
        }

        public bool ArePrerequisitesSatisfied(IGameState state)
        {
            PlayerDataPrerequisite[][]? prerequisites = Prerequisites;
            if (prerequisites == null || prerequisites.Length == 0) return true;

            foreach (PlayerDataPrerequisite[] alternative in prerequisites)
            {
                if (!HasValues(alternative)) continue;
                bool satisfied = true;
                foreach (PlayerDataPrerequisite condition in alternative)
                {
                    if (condition == null || !condition.IsSatisfied(state))
                    {
                        satisfied = false;
                        break;
                    }
                }
                if (satisfied) return true;
            }
            return false;
        }

        private static bool HasValues<T>(T[]? values) => values != null && values.Length > 0;
    }
}

