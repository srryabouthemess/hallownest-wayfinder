using System;

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
            PlayerData? player = PlayerData.instance;
            if (player == null) return null;

            return FromValues(player.charmsOwned, player.maxHealthBase,
                player.MPReserveMax, player.nailSmithUpgrades,
                player.dreamOrbs, player.dreamOrbsSpent);
        }

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

            return string.Format(
                LocalizationService.Text("completion_checklist",
                    "Amuletos {0}/40  •  Máscaras {1}/9  •  Vasos {2}/3  •  Ferrão {3}/4  •  Essência {4}/2400"),
                snapshot.Charms, snapshot.Masks, snapshot.Vessels,
                snapshot.NailUpgrades, snapshot.Essence);
        }

        private static int Clamp(int value, int minimum, int maximum) =>
            Math.Max(minimum, Math.Min(value, maximum));
    }
}
