using System.Collections.Generic;

namespace HallownestWayfinder
{
    /// <summary>
    /// Safe, glitchless route for the Speedrun 1 achievement, adapted from
    /// fireb0rn's Steam guide. Hints are original summaries for the in-game HUD.
    /// </summary>
    public static class SpeedrunRouteDefinition
    {
        public const string Name = "Speedrun 5h • Sem glitches";

        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            // Segmento 1 — Espírito Vingativo
            S("sr01_fury", "Fury_of_the_Fallen.png", pb: "gotCharm_6", target: "Tutorial_01", arrow: 135f),
            S("sr01_geo", "crawlid.png", pi: "geo", min: 50, arrow: 225f),
            S("sr01_stag", "LastStag.png", pb: "openedCrossroads", target: "Crossroads_47", arrow: 225f),
            S("sr01_false_knight", "False_Knight.png", pb: "killedFalseKnight", target: "Crossroads_10", arrow: 315f),
            S("sr01_spell", "Vengeful_Spirit_Icon.png", pb: "hasSpell", target: "Crossroads_ShamanTemple", arrow: 270f),
            S("sr01_soul_catcher", "Soul_Catcher.png", pb: "gotCharm_20", target: "Crossroads_ShamanTemple", arrow: 90f),

            // Segmento 2 — Manto de Asa de Mariposa
            S("sr02_vengefly", "Vengefly_King_Zote.png", pb: "zoteRescuedBuzzer", target: "Fungus1_20_v02", arrow: 315f),
            S("sr02_journal", "Wanderers_Journal.png", pi: "trinket1", min: 1, target: "Fungus1_22", arrow: 270f),
            S("sr02_bench", "bench.png", benchScene: "Fungus1_16_alt", target: "Fungus1_16_alt", arrow: 180f),
            S("sr02_hornet", "Hornet.png", pb: "hasDash", target: "Fungus1_04", arrow: 270f),

            // Segmento 3 — Garra de Louva-a-Deus
            S("sr03_queen_bench", "bench.png", benchScene: "Fungus2_02", arrow: 180f),
            S("sr03_seal", "Wanderers_Journal.png", intSum: new[] { "trinket2", "soldTrinket2" }, sumMin: 1, arrow: 90f),
            S("sr03_claw", "Mantis_Claw.png", pb: "hasWalljump", arrow: 270f),
            S("sr03_city", "crawlid.png", pb: "visitedRuins", arrow: 90f),

            // Segmento 4 — Santuário das Almas
            S("sr04_nail", "Nailsmith.png", pi: "nailSmithUpgrades", min: 1, arrow: 270f),
            S("sr04_seal", "Wanderers_Journal.png", intSum: new[] { "trinket2", "soldTrinket2" }, sumMin: 2, arrow: 0f),
            S("sr04_bench", "bench.png", pb: "tollBenchCity", arrow: 90f),
            S("sr04_twister", "Spell_Twister.png", pb: "gotCharm_33", arrow: 45f),
            S("sr04_master", "Soul_Master.png", pi: "quakeLevel", min: 1, arrow: 270f),
            S("sr04_sell", "Wanderers_Journal.png", pb: "metRelicDealer", noRelics: true, arrow: 180f),
            S("sr04_key", "Elegant_Key.png", pi: "simpleKeys", min: 1, arrow: 90f),
            S("sr04_stag", "LastStag.png", pb: "openedRuins1", arrow: 90f),

            // Segmento 5 — Compras e entrada no Pico
            S("sr05_gruz", "gruz_mother.png", pb: "killedBigFly", target: "Crossroads_04", arrow: 180f),
            S("sr05_sly", "Sly_Basement.png", pb: "slyRescued", target: "Room_shop", arrow: 90f),
            S("sr05_steady", "Salubra.png", pb: "gotCharm_14", arrow: 90f),
            S("sr05_shaman", "Salubra.png", pb: "gotCharm_19", arrow: 90f),
            S("sr05_notch", "Salubra.png", pi: "charmSlots", min: 4, arrow: 90f),
            S("sr05_lantern", "Lumafly_Lantern.png", pb: "hasLantern", arrow: 90f),
            S("sr05_peak", "Crystal_Heart.png", pb: "visitedMines", arrow: 0f),

            // Segmento 6 — Pico de Cristal
            S("sr06_heart", "Crystal_Heart.png", pb: "hasSuperDash", arrow: 90f),
            S("sr06_dark", "Descending_Dark.png", pi: "quakeLevel", min: 2, arrow: 90f),
            S("sr06_dream", "Dream_Nail.png", pb: "hasDreamNail", arrow: 180f),
            S("sr06_stag", "LastStag.png", pb: "openedRestingGrounds", arrow: 90f),

            // Segmento 7 — Lágrima de Isma e Lurien
            S("sr07_waterways", "crawlid.png", pb: "openedWaterwaysManhole", arrow: 180f),
            S("sr07_dung", "Dung_Defender.png", pb: "killedDungDefender", arrow: 90f),
            S("sr07_isma", "Ismas_Tear.png", pb: "hasAcidArmour", arrow: 90f),
            S("sr07_skip", "Monarch_Wings.png", skippable: true, notRequiredFor112: true, visitedScene: "Ruins2_03", arrow: 0f),
            S("sr07_watchers", null, pb: "killedBlackKnight", arrow: 90f),
            S("sr07_lurien", "Dream_Nail.png", pb: "lurienDefeated", arrow: 0f),
            S("sr07_kings", "LastStag.png", pb: "openedRuins2", arrow: 180f),

            // Segmento 8 — Monomon
            S("sr08_archives", "Cornifer.png", visitedScene: "Fungus3_archive", arrow: 180f),
            S("sr08_uumuu", null, pb: "killedMegaJellyfish", arrow: 180f),
            S("sr08_monomon", "Dream_Nail.png", pb: "monomonDefeated", arrow: 90f),
            S("sr08_gardens", "crawlid.png", pb: "visitedRoyalGardens", arrow: 270f),

            // Segmento 9 — Herrah
            S("sr09_bench", "bench.png", pb: "tollBenchQueensGardens", arrow: 270f),
            S("sr09_herrah", "Dream_Nail.png", pb: "hegemolDefeated", arrow: 0f),
            S("sr09_stag", "LastStag.png", pb: "openedDeepnest", arrow: 90f),

            // Segmento 10 — Hollow Knight
            S("sr10_egg", "Dream_Nail.png", pb: "openedBlackEggDoor", arrow: 90f),
            S("sr10_hollow_knight", null, pb: "killedHollowKnight", arrow: 90f)
        };

        private static RouteStep S(string id, string? icon,
            bool skippable = false, bool notRequiredFor112 = false,
            string? pb = null, string? pi = null, int min = 0,
            string? scene = null, string? target = null, float arrow = 90f,
            string[]? allBools = null, string[]? anyBools = null,
            string? visitedScene = null, string? benchScene = null,
            string[]? intSum = null, int sumMin = 0, bool noRelics = false)
        {
            return new RouteStep
            {
                Id = id,
                Icon = icon,
                SkippableInRoute = skippable,
                NotRequiredFor112 = notRequiredFor112,
                Completion = new RouteCompletion
                {
                    PlayerBool = pb,
                    PlayerInt = pi,
                    Minimum = min,
                    AllPlayerBools = allBools,
                    AnyPlayerBools = anyBools,
                    VisitedScene = visitedScene,
                    BenchScene = benchScene,
                    PlayerIntSum = intSum,
                    PlayerIntSumMinimum = sumMin,
                    NoRelics = noRelics,
                    Scene = scene
                },
                TargetScene = target,
                ArrowDegrees = arrow
            };
        }
    }
}
