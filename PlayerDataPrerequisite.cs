namespace HallownestWayfinder
{
    public sealed class PlayerDataPrerequisite
    {
        public string? PlayerBool { get; set; }
        public string? PlayerInt { get; set; }
        public int Minimum { get; set; }

        public static PlayerDataPrerequisite Bool(string field) =>
            new PlayerDataPrerequisite { PlayerBool = field };

        public static PlayerDataPrerequisite Int(string field, int minimum) =>
            new PlayerDataPrerequisite { PlayerInt = field, Minimum = minimum };

        public bool IsSatisfied()
        {
            if (PlayerData.instance == null) return false;

            try
            {
                if (!string.IsNullOrEmpty(PlayerBool))
                    return PlayerData.instance.GetBool(PlayerBool);
                if (!string.IsNullOrEmpty(PlayerInt))
                    return PlayerData.instance.GetInt(PlayerInt) >= Minimum;
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
