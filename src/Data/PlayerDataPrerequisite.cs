namespace HallownestWayfinder
{
    public sealed class PlayerDataPrerequisite
    {
        public string? PlayerBool { get; set; }
        public string? PlayerInt { get; set; }
        public int Minimum { get; set; }

        public static PlayerDataPrerequisite Bool(string field) =>
            new PlayerDataPrerequisite { PlayerBool = field };

        public static PlayerDataPrerequisite MinimumValue(string field, int minimum) =>
            new PlayerDataPrerequisite { PlayerInt = field, Minimum = minimum };

        public bool IsSatisfied()
        {
            return PlayerDataGameState.TryCapture(out PlayerDataGameState? state) &&
                state != null && IsSatisfied(state);
        }

        public bool IsSatisfied(IGameState state)
        {
            try
            {
                string? playerBool = PlayerBool;
                if (!string.IsNullOrEmpty(playerBool))
                    return state.GetBool(playerBool!);
                string? playerInt = PlayerInt;
                if (!string.IsNullOrEmpty(playerInt))
                    return state.GetInt(playerInt!) >= Minimum;
            }
            catch
            {
                return false;
            }

            // An empty condition is invalid and must never unlock a step.
            return false;
        }
    }
}
