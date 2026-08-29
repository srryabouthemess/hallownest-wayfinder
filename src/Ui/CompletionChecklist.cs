using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace HallownestWayfinder
{
    public sealed class CompletionChecklistSnapshot
    {
        public int Percentage { get; set; }
        public int Charms { get; set; }
        public int Equipment { get; set; }
        public int Spells { get; set; }
        public int Bosses { get; set; }
        public int WarriorDreams { get; set; }
        public int Dreamers { get; set; }
        public int Masks { get; set; }
        public int MaskUpgrades { get; set; }
        public int Vessels { get; set; }
        public int NailUpgrades { get; set; }
        public int NailArts { get; set; }
        public int DreamNail { get; set; }
        public int Essence { get; set; }
        public int Colosseum { get; set; }
        public int GrimmTroupe { get; set; }
        public int Hive { get; set; }
        public int Godhome { get; set; }
    }

    public static class CompletionChecklist
    {
        public const int TotalPercentage = 112;
        public const int TotalCharms = 40;
        public const int TotalEquipment = 7;
        public const int TotalSpells = 6;
        public const int TotalBosses = 14;
        public const int TotalWarriorDreams = 7;
        public const int TotalDreamers = 3;
        public const int TotalMasks = 9;
        public const int TotalMaskUpgrades = 4;
        public const int TotalVessels = 3;
        public const int TotalNailUpgrades = 4;
        public const int TotalNailArts = 3;
        public const int TotalDreamNail = 3;
        public const int TotalEssence = 2400;
        public const int TotalColosseum = 3;
        public const int TotalGrimmTroupe = 2;
        public const int TotalHive = 1;
        public const int TotalGodhome = 5;

        private static readonly string[] BossFields =
        {
            "killedInfectedKnight", "killedMawlek", "collectorDefeated",
            "killedDungDefender", "killedFalseKnight", "killedBigFly",
            "hasDash", "hornetOutskirtsDefeated", "defeatedMantisLords",
            "killedMimicSpider", "killedTraitorLord", "killedMegaJellyfish",
            "killedBlackKnight"
        };

        private static readonly string[] WarriorDreamFields =
        {
            "elderHuDefeated", "galienDefeated", "aladarSlugDefeated",
            "markothDefeated", "mumCaterpillarDefeated", "noEyesDefeated",
            "xeroDefeated"
        };

        private static readonly string[] EquipmentFields =
        {
            "hasSuperDash", "hasAcidArmour", "hasKingsBrand", "hasWalljump",
            "hasDoubleJump", "hasDash", "hasShadowDash"
        };

        private static readonly string[] NailArtFields =
        {
            "hasCyclone", "hasDashSlash", "hasUpwardSlash"
        };

        private static readonly string[] DreamerFields =
        {
            "hegemolDefeated", "lurienDefeated", "monomonDefeated"
        };

        private static readonly string[] ColosseumFields =
        {
            "colosseumBronzeCompleted", "colosseumSilverCompleted",
            "colosseumGoldCompleted"
        };

        private static readonly string[] GrimmTroupeFields = { "killedGrimm" };
        private static readonly string[] AdditionalBoolFields =
        {
            "hasSpell", "hasDreamNail", "dreamNailUpgraded", "dreamReward9",
            "defeatedNightmareGrimm", "destroyedNightmareLantern",
            "killedHiveKnight", "hasGodfinder"
        };
        private static readonly string[] ReferencedBools = BuildReferencedBools();
        private static readonly string[] ReferencedInts =
        {
            "fireballLevel", "quakeLevel", "screamLevel",
            "elderHuDefeated", "galienDefeated", "aladarSlugDefeated",
            "markothDefeated", "mumCaterpillarDefeated", "noEyesDefeated",
            "xeroDefeated"
        };

        public static IReadOnlyList<string> ReferencedPlayerBools { get; } =
            new ReadOnlyCollection<string>(ReferencedBools);
        public static IReadOnlyList<string> ReferencedPlayerInts { get; } =
            new ReadOnlyCollection<string>(ReferencedInts);

        public static IReadOnlyList<string> ValidatePlayerDataReferences() =>
            RouteDataValidator.ValidatePlayerDataFields("112% checklist",
                ReferencedPlayerBools, ReferencedPlayerInts);

        public static CompletionChecklistSnapshot? Read()
        {
            if (!PlayerDataGameState.TryCapture(out PlayerDataGameState? state) || state == null)
                return null;
            return Read(state);
        }

        public static CompletionChecklistSnapshot Read(IGameState state)
        {
            CompletionChecklistSnapshot snapshot = FromValues(state.CharmsOwned,
                state.MaxHealthBase, state.SoulReserveMaximum, state.NailUpgrades,
                state.CurrentEssence, state.SpentEssence);

            snapshot.Bosses = CountBools(state, BossFields) +
                (state.GetInt("quakeLevel") >= 1 ? 1 : 0);
            snapshot.WarriorDreams = CountPositiveInts(state, WarriorDreamFields);
            snapshot.Equipment = CountBools(state, EquipmentFields);
            snapshot.Spells = SpellCount(state);
            snapshot.NailArts = CountBools(state, NailArtFields);
            snapshot.Dreamers = CountBools(state, DreamerFields);
            snapshot.Colosseum = CountBools(state, ColosseumFields);
            snapshot.DreamNail =
                (state.GetBool("hasDreamNail") ? 1 : 0) +
                (state.GetBool("dreamNailUpgraded") ? 1 : 0) +
                (state.GetBool("dreamReward9") ? 1 : 0);
            snapshot.GrimmTroupe = CountBools(state, GrimmTroupeFields) +
                (state.GetBool("defeatedNightmareGrimm") ||
                 state.GetBool("destroyedNightmareLantern") ? 1 : 0);
            snapshot.Hive = state.GetBool("killedHiveKnight") ? 1 : 0;
            snapshot.Godhome = (state.GetBool("hasGodfinder") ? 1 : 0) +
                Clamp(state.CompletedPantheons, 0, 4);

            snapshot.Percentage = Clamp(
                snapshot.Charms + snapshot.Bosses + snapshot.WarriorDreams +
                snapshot.Colosseum + snapshot.Equipment * 2 + snapshot.Spells +
                snapshot.NailArts + snapshot.MaskUpgrades + snapshot.Vessels +
                snapshot.NailUpgrades + snapshot.DreamNail + snapshot.Dreamers +
                snapshot.GrimmTroupe + snapshot.Hive + snapshot.Godhome,
                0, TotalPercentage);
            return snapshot;
        }

        public static CompletionChecklistSnapshot FromValues(int charms, int maxHealthBase,
            int reserveSoulMaximum, int nailUpgrades, int currentEssence, int spentEssence)
        {
            int masks = Clamp(maxHealthBase, 0, TotalMasks);
            return new CompletionChecklistSnapshot
            {
                Charms = Clamp(charms, 0, TotalCharms),
                Masks = masks,
                MaskUpgrades = Clamp(masks - 5, 0, TotalMaskUpgrades),
                Vessels = Clamp(reserveSoulMaximum / 33, 0, TotalVessels),
                NailUpgrades = Clamp(nailUpgrades, 0, TotalNailUpgrades),
                Essence = Clamp(currentEssence + spentEssence, 0, TotalEssence)
            };
        }

        public static string? Format(bool detailed)
        {
            CompletionChecklistSnapshot? snapshot = Read();
            return snapshot == null ? null : Format(snapshot, detailed);
        }

        public static string Format(IGameState state, bool detailed) =>
            Format(Read(state), detailed);

        public static string Format(CompletionChecklistSnapshot snapshot, bool detailed)
        {
            if (!detailed)
            {
                return Text("completion_checklist", snapshot.Charms, snapshot.Masks,
                    snapshot.Vessels, snapshot.NailUpgrades, snapshot.Essence);
            }

            return string.Join(Environment.NewLine, new[]
            {
                Text("completion_checklist_total", snapshot.Percentage),
                Text("completion_checklist_collection", snapshot.Charms,
                    snapshot.Equipment, snapshot.Spells),
                Text("completion_checklist_combat", snapshot.Bosses,
                    snapshot.WarriorDreams, snapshot.Dreamers),
                Text("completion_checklist_upgrades", snapshot.Masks,
                    snapshot.Vessels, snapshot.NailUpgrades, snapshot.NailArts),
                Text("completion_checklist_world", snapshot.DreamNail,
                    snapshot.Essence, snapshot.Colosseum),
                Text("completion_checklist_content", snapshot.GrimmTroupe,
                    snapshot.Hive, snapshot.Godhome)
            });
        }

        private static int SpellCount(IGameState state) =>
            SpellLevel(state.GetInt("fireballLevel"), state.GetBool("hasSpell")) +
            SpellLevel(state.GetInt("quakeLevel"), false) +
            SpellLevel(state.GetInt("screamLevel"), false);

        private static int SpellLevel(int level, bool baseSpellFallback) =>
            Clamp(Math.Max(level, baseSpellFallback ? 1 : 0), 0, 2);

        private static int CountBools(IGameState state, IEnumerable<string> fields)
        {
            int count = 0;
            foreach (string field in fields)
                if (state.GetBool(field)) count++;
            return count;
        }

        private static int CountPositiveInts(IGameState state, IEnumerable<string> fields)
        {
            int count = 0;
            foreach (string field in fields)
                if (state.GetInt(field) > 0) count++;
            return count;
        }

        private static string Text(string key, params object[] values) =>
            string.Format(CultureInfo.InvariantCulture, LocalizationService.Text(key), values);

        private static string[] BuildReferencedBools()
        {
            List<string> fields = new List<string>();
            AddRange(fields, BossFields);
            AddRange(fields, EquipmentFields);
            AddRange(fields, NailArtFields);
            AddRange(fields, DreamerFields);
            AddRange(fields, ColosseumFields);
            AddRange(fields, GrimmTroupeFields);
            AddRange(fields, AdditionalBoolFields);
            return fields.ToArray();
        }

        private static void AddRange(List<string> destination, IEnumerable<string> source)
        {
            foreach (string value in source)
                if (!destination.Contains(value)) destination.Add(value);
        }

        private static int Clamp(int value, int minimum, int maximum) =>
            Math.Max(minimum, Math.Min(value, maximum));
    }
}
