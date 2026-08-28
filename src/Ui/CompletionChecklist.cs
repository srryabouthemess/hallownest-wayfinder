using System;
using System.Globalization;

namespace HallownestWayfinder
{
    public sealed class CompletionChecklistSnapshot
    {
        public int Charms { get; set; }
        public int Masks { get; set; }
        public int Vessels { get; set; }
        public int NailUpgrades { get; set; }
        public int Essence { get; set; }
    }

    public static class CompletionChecklist
    {
        public const int TotalCharms = 40;
        public const int TotalMasks = 9;
        public const int TotalVessels = 3;
        public const int TotalNailUpgrades = 4;
        public const int TotalEssence = 2400;

        public static CompletionChecklistSnapshot? Read()
        {
            if (!PlayerDataGameState.TryCapture(out PlayerDataGameState? state) || state == null)
                return null;
            return Read(state);
        }

        public static CompletionChecklistSnapshot Read(IGameState state) =>
            FromValues(state.CharmsOwned, state.MaxHealthBase,
                state.SoulReserveMaximum, state.NailUpgrades,
                state.CurrentEssence, state.SpentEssence);

        public static CompletionChecklistSnapshot FromValues(int charms, int maxHealthBase,
            int reserveSoulMaximum, int nailUpgrades, int currentEssence, int spentEssence) =>
            new CompletionChecklistSnapshot
            {
                Charms = Clamp(charms, 0, TotalCharms),
                Masks = Clamp(maxHealthBase, 0, TotalMasks),
                Vessels = Clamp(reserveSoulMaximum / 33, 0, TotalVessels),
                NailUpgrades = Clamp(nailUpgrades, 0, TotalNailUpgrades),
                Essence = Math.Max(0, currentEssence + spentEssence)
            };

        public static string? Format()
        {
            CompletionChecklistSnapshot? snapshot = Read();
            if (snapshot == null) return null;

            return string.Format(CultureInfo.InvariantCulture,
                LocalizationService.Text("completion_checklist"),
                snapshot.Charms, snapshot.Masks, snapshot.Vessels,
                snapshot.NailUpgrades, snapshot.Essence);
        }

        public static string Format(IGameState state)
        {
            CompletionChecklistSnapshot snapshot = Read(state);
            return string.Format(CultureInfo.InvariantCulture,
                LocalizationService.Text("completion_checklist"),
                snapshot.Charms, snapshot.Masks, snapshot.Vessels,
                snapshot.NailUpgrades, snapshot.Essence);
        }

        private static int Clamp(int value, int minimum, int maximum) =>
            Math.Max(minimum, Math.Min(value, maximum));
    }
}

