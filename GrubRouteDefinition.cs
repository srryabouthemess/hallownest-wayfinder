using System;
using System.Collections.Generic;

namespace HallownestWayfinder
{
    /// <summary>
    /// Complete vanilla grub checklist. Scene identifiers are based on the
    /// canonical ItemChanger location data and the game's scenesGrubRescued list.
    /// </summary>
    public static class GrubRouteDefinition
    {
        public const string Name = "Larvas 46/46";

        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            G("grub_crossroads_acid", "Crossroads_35", 270f),
            G("grub_crossroads_center", "Crossroads_05", 90f),
            G("grub_crossroads_stag", "Crossroads_03", 0f),
            G("grub_crossroads_spike", "Crossroads_31", 135f),
            G("grub_crossroads_guarded", "Crossroads_48", 45f),

            G("grub_greenpath_cornifer", "Fungus1_06", 135f),
            G("grub_greenpath_journal", "Fungus1_07", 225f),
            G("grub_greenpath_mmc", "Fungus1_13", 270f),
            G("grub_greenpath_stag", "Fungus1_21", 90f),
            G("grub_cliffs", "Fungus1_28", 315f),

            G("grub_fungal_bouncy", "Fungus2_18", 0f),
            G("grub_fungal_spore", "Fungus2_20", 270f),
            G("grub_deepnest_spike", "Deepnest_03", 180f),

            G("grub_city_left", "Ruins1_05", 45f),
            G("grub_soul_sanctum", "Ruins1_32", 180f),
            G("grub_city_guarded", "Ruins_House_01", 90f),

            G("grub_peak_spike", "Mines_03", 0f),
            G("grub_peak_chest", "Mines_04", 180f),
            G("grub_peak_mimic", "Mines_16", 90f),
            G("grub_peak_crushers", "Mines_19", 90f),
            G("grub_peak_crown", "Mines_24", 315f),
            G("grub_peak_heart", "Mines_31", 270f),
            G("grub_mound", "Mines_35", 90f),

            G("grub_resting", "RestingGrounds_10", 180f),
            G("grub_waterways_main", "Waterways_04", 270f),
            G("grub_isma", "Waterways_13", 0f),
            G("grub_waterways_tram", "Waterways_14", 90f),

            G("grub_basin_dive", "Abyss_17", 180f),
            G("grub_basin_wings", "Abyss_19", 270f),

            G("grub_dark_deepnest", "Deepnest_39", 270f),
            G("grub_deepnest_mimic", "Deepnest_36", 270f),
            G("grub_deepnest_nosk", "Deepnest_31", 270f),
            G("grub_beasts_den", "Deepnest_Spider_Town", 0f),

            G("grub_kingdom_camp", "Deepnest_East_11", 90f),
            G("grub_kingdom_oro", "Deepnest_East_14", 90f),
            G("grub_kings_station", "Ruins2_07", 90f),

            G("grub_gardens_stag", "Fungus3_10", 90f),
            G("grub_gardens_top", "Fungus3_22", 0f),
            G("grub_gardens_marmu", "Fungus3_48", 90f),
            G("grub_fog", "Fungus3_47", 270f),

            G("grub_hive_external", "Hive_03", 90f),
            G("grub_hive_internal", "Hive_04", 90f),

            G("grub_collector", "Ruins2_11", 0f, count: 3),
            G("grub_watcher", "Ruins2_03", 0f)
        };

        private static RouteStep G(string id, string scene,
            float arrow, int count = 1)
        {
            return new RouteStep
            {
                Id = id,
                Icon = "grub.png",
                Completion = new RouteCompletion
                {
                    GrubScene = scene,
                    GrubCountInScene = count
                },
                ArrowDegrees = arrow,
                Prerequisites = PrerequisitesFor(scene)
            };
        }

        private static PlayerDataPrerequisite[][]? PrerequisitesFor(string scene)
        {
            if (scene.StartsWith("Mines_", StringComparison.Ordinal))
                return Any(B("hasLantern"), I("quakeLevel", 1));
            if (scene.StartsWith("Waterways_", StringComparison.Ordinal))
                return All(B("openedWaterwaysManhole"));
            if (scene.StartsWith("Abyss_", StringComparison.Ordinal))
                return Any(B("hasSuperDash"), B("hasDoubleJump"));
            if (scene.StartsWith("Deepnest_", StringComparison.Ordinal))
                return Any(B("hasLantern"), B("defeatedMantisLords"));
            if (scene.StartsWith("Hive_", StringComparison.Ordinal))
                return All(B("hasTramPass"));
            if (scene.StartsWith("Fungus3_", StringComparison.Ordinal))
                return Any(B("hasAcidArmour"), B("hasShadowDash"));
            if (scene.StartsWith("Ruins2_", StringComparison.Ordinal))
                return All(B("visitedRuins"));
            return null;
        }

        private static PlayerDataPrerequisite B(string field) =>
            PlayerDataPrerequisite.Bool(field);

        private static PlayerDataPrerequisite I(string field, int minimum) =>
            PlayerDataPrerequisite.Int(field, minimum);

        private static PlayerDataPrerequisite[][] All(params PlayerDataPrerequisite[] conditions) =>
            new[] { conditions };

        private static PlayerDataPrerequisite[][] Any(params PlayerDataPrerequisite[] conditions)
        {
            PlayerDataPrerequisite[][] alternatives =
                new PlayerDataPrerequisite[conditions.Length][];
            for (int index = 0; index < conditions.Length; index++)
                alternatives[index] = new[] { conditions[index] };
            return alternatives;
        }
    }
}
