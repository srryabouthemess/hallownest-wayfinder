using System.Collections.Generic;

namespace HallownestWayfinder
{
    /// <summary>
    /// Continuação pós-Hornet, adaptada e resumida do guia 112% de Almech Alfarion.
    /// Os textos são orientações próprias e curtas para uso dentro do HUD.
    /// </summary>
    public static class GuideContinuation
    {
        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            // Capítulo 3 — Ermos Fúngicos
            S("c03_dirtmouth", "LastStag.png", skippable: true, notRequiredFor112: true, scene: "Town", transportScene: "Fungus1_16_alt", transport: "Use o Último Besouro para viajar até Dirtmouth", arrow: 90f),
            S("c03_compass", "Cornifer.png", skippable: true, pb: "gotCharm_2", targetScene: "Room_mapper", arrow: 90f),
            S("c03_grub6", "grub.png", grubScene: "Crossroads_05", arrow: 90f),
            S("c03_wastes", "crawlid.png", pb: "visitedFungus", targetScene: "Fungus2_06", arrow: 180f, prerequisites: All(B("hasDash"))),
            S("c03_ogres", "Shrumal_Warrior.png", pb: "notchShroomOgres", targetScene: "Fungus2_05", arrow: 270f, prerequisites: All(B("hasDash"))),
            S("c03_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapFungalWastes", arrow: 225f),
            S("c03_claw", "Mantis_Claw.png", pb: "hasWalljump", arrow: 270f, prerequisites: All(B("hasDash"))),

            // Capítulo 4 — Cidade das Lágrimas
            S("c04_grub7", "grub.png", grubScene: "Fungus2_18", arrow: 0f),
            S("c04_queen_station", "LastStag.png", pb: "openedFungalWastes", arrow: 270f),
            S("c04_lantern", "Lumafly_Lantern.png", pb: "hasLantern", arrow: 90f),
            S("c04_grub8", "grub.png", grubScene: "Fungus2_20", arrow: 270f),
            S("c04_spore", "Spore_Shroom.png", pb: "gotCharm_17", arrow: 180f),
            S("c04_deepnest_grub", "grub.png", grubScene: "Deepnest_03", arrow: 180f),
            S("c04_deepnest_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapDeepnest", arrow: 225f),
            S("c04_city", "crawlid.png", pb: "visitedRuins", arrow: 90f, prerequisites: All(B("hasWalljump"))),
            S("c04_nail1", "Nailsmith.png", skippable: true, pi: "nailSmithUpgrades", min: 1, arrow: 270f),
            S("c04_lemm", "Wanderers_Journal.png", pb: "metRelicDealer", arrow: 90f),
            S("c04_grub10", "grub.png", grubScene: "Ruins1_05", arrow: 45f),
            S("c04_city_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapCity", arrow: 270f),
            S("c04_spell_twister", "Spell_Twister.png", pb: "gotCharm_33", arrow: 45f),
            S("c04_soul_master", "Soul_Master.png", pi: "quakeLevel", min: 1, arrow: 270f, prerequisites: All(B("visitedRuins"))),
            S("c04_grub11", "grub.png", grubScene: "Ruins1_32", arrow: 180f),
            S("c04_storerooms", "LastStag.png", pb: "openedRuins1", arrow: 90f),

            // Capítulos 5–7 — Pico de Cristal, sonhos e Alma Sombria
            S("c05_salubra", "Salubra.png", pb: "salubraNotch1", arrow: 90f),
            S("c05_peak", "Elder_Baldur.png", pb: "visitedMines", arrow: 0f, prerequisites: Any(I("quakeLevel", 1), B("hasLantern"))),
            S("c05_grub12", "grub.png", grubScene: "Mines_03", arrow: 0f),
            S("c05_peak_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapMines", arrow: 0f),
            S("c05_shop_key", "Shopkeepers_Key.png", pb: "hasSlykey", arrow: 45f),
            S("c05_heart", "Crystal_Heart.png", pb: "hasSuperDash", arrow: 90f, prerequisites: All(B("visitedMines"))),
            S("c05_grubs", "grub.png", pi: "grubsCollected", min: 18, arrow: 270f),
            S("c05_dark", "Descending_Dark.png", pi: "quakeLevel", min: 2, arrow: 90f, prerequisites: All(B("hasSuperDash"))),
            S("c06_dream_nail", "Dream_Nail.png", pb: "hasDreamNail", arrow: 180f, prerequisites: Any(B("hasSuperDash"), I("quakeLevel", 1))),
            S("c06_dreamshield", "Dreamshield.png", pb: "gotCharm_38", arrow: 180f),
            S("c06_station", "LastStag.png", pb: "openedRestingGrounds", arrow: 90f),
            S("c07_elegant", "Elegant_Key.png", anyBools: new[] { "hasWhiteKey", "usedWhiteKey" }, arrow: 90f),
            S("c07_shade_soul", "Vengeful_Spirit_Icon.png", pi: "fireballLevel", min: 2, arrow: 0f, prerequisites: Any(B("hasWhiteKey"), B("usedWhiteKey"))),
            S("c07_waterways", "crawlid.png", pb: "openedWaterwaysManhole", arrow: 180f),

            // Capítulos 8–9 — Esgotos e Bacia Antiga
            S("c08_grub17", "grub.png", grubScene: "Waterways_04", arrow: 270f),
            S("c08_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapWaterways", arrow: 315f),
            S("c08_dung", "Dung_Defender.png", pb: "killedDungDefender", arrow: 90f),
            S("c08_isma", "Ismas_Tear.png", pb: "hasAcidArmour", arrow: 90f, prerequisites: All(B("killedDungDefender"), B("hasSuperDash"))),
            S("c09_grub18", "grub.png", grubScene: "Waterways_13", arrow: 0f),
            S("c09_ore1", "Pale_Ore.png", pi: "ore", min: 1, arrow: 225f),
            S("c09_grub19", "grub.png", grubScene: "Abyss_17", arrow: 180f),
            S("c09_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapAbyss", arrow: 180f),
            S("c09_broken", "Broken_Vessel.png", pb: "killedInfectedKnight", arrow: 270f),
            S("c09_wings", "Monarch_Wings.png", pb: "hasDoubleJump", arrow: 225f, prerequisites: All(B("killedInfectedKnight"))),
            S("c09_lost_kin", "Lost_Kin.png", pb: "infectedKnightDreamDefeated", arrow: 90f, prerequisites: All(B("hasDreamNail"), B("killedInfectedKnight"))),
            S("c09_hidden_station", "LastStag.png", pb: "openedHiddenStation", arrow: 90f),

            // Capítulos 10–13 — Grimm, montanha e segredos do Caminho Verde
            S("c10_ritual", null, pb: "nightmareLanternLit", arrow: 315f, prerequisites: All(B("hasDreamNail"))),
            S("c10_cliffs_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapCliffs", arrow: 0f),
            S("c10_joni", "Jonis_Blessing.png", pb: "gotCharm_27", arrow: 90f),
            S("c10_cyclone", "Cyclone_Slash.png", pb: "hasCyclone", arrow: 90f),
            S("c10_gorb", "Gorb.png", pi: "aladarSlugDefeated", min: 1, arrow: 315f),
            S("c10_stag_nest", "LastStag.png", pb: "openedStagNest", arrow: 270f),
            S("c11_grimmchild", "Grimmchild.png", pb: "gotCharm_40", arrow: 90f),
            S("c11_failed", "Failed_Champion.png", pb: "falseKnightDreamDefeated", arrow: 315f, prerequisites: All(B("hasDreamNail"))),
            S("c11_xero", "Xero.png", pi: "xeroDefeated", min: 1, arrow: 270f, prerequisites: All(B("hasDreamNail"))),
            S("c11_dreamgate", "Dreamgate.png", pb: "hasDreamGate", arrow: 90f),
            S("c12_deep_focus", "Deep_Focus.png", pb: "gotCharm_34", arrow: 90f),
            S("c12_guardians", "Crystal_Guardian.png", pb: "defeatedMegaBeamMiner2", arrow: 0f),
            S("c12_ore", "Pale_Ore.png", pi: "ore", min: 3, arrow: 315f),
            S("c13_sheo", "Great_Slash.png", pb: "hasUpwardSlash", arrow: 270f),
            S("c13_thorns", null, pb: "gotCharm_12", arrow: 90f),
            S("c13_grubs", "grub.png", pi: "grubsCollected", min: 27, arrow: 270f),
            S("c13_wraiths", null, pi: "screamLevel", min: 1, arrow: 270f),
            S("c13_noeyes", null, pi: "noEyesDefeated", min: 1, arrow: 90f, prerequisites: All(B("hasDreamNail"))),

            // Capítulos 14–18 — Borda do reino, Abismo e Sonhadores
            S("c14_souleater", null, pb: "gotCharm_21", arrow: 90f),
            S("c15_kings_station", "LastStag.png", pb: "openedRuins2", arrow: 180f),
            S("c15_edge_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapOutskirts", arrow: 180f),
            S("c15_hornet2", "Hornet.png", pb: "hornetOutskirtsDefeated", arrow: 90f),
            S("c15_brand", null, pb: "hasKingsBrand", arrow: 270f),
            S("c16_abyss", "crawlid.png", pb: "visitedAbyss", arrow: 180f, prerequisites: All(B("hasKingsBrand"))),
            S("c16_shriek", null, pi: "screamLevel", min: 2, arrow: 270f, prerequisites: All(B("hasKingsBrand"))),
            S("c16_cloak", null, pb: "hasShadowDash", arrow: 90f, prerequisites: All(B("hasKingsBrand"))),
            S("c17_grimm", null, pb: "killedGrimm", arrow: 90f, prerequisites: All(B("gotCharm_40"))),
            S("c17_nail", null, pi: "nailSmithUpgrades", min: 3, arrow: 270f),
            S("c17_fluke", null, pb: "killedFlukeMother", arrow: 225f),
            S("c17_dashmaster", null, pb: "gotCharm_31", arrow: 270f),
            S("c17_bretta", null, skippable: true, notRequiredFor112: true, pb: "brettaRescued", arrow: 270f),
            S("c17_lords", null, pb: "defeatedMantisLords", arrow: 180f),
            S("c17_pride", null, pb: "gotCharm_13", arrow: 90f, prerequisites: All(B("defeatedMantisLords"))),
            S("c18_mawlek", null, pb: "killedMawlek", arrow: 270f),
            S("c18_notch", null, pb: "notchFogCanyon", arrow: 270f),
            S("c18_fragile", null, pb: "gotCharm_25", arrow: 90f),
            S("c18_hu", null, pi: "elderHuDefeated", min: 1, arrow: 90f, prerequisites: All(B("hasDreamNail"))),
            S("c18_fog_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapFogCanyon", arrow: 270f),
            S("c18_uumuu", null, pb: "killedMegaJellyfish", arrow: 180f, prerequisites: All(B("hasAcidArmour"))),
            S("c18_monomon", null, pb: "monomonDefeated", arrow: 90f, prerequisites: All(B("hasDreamNail"), B("killedMegaJellyfish"))),

            // Capítulos 19–23 — Jardins, Ninho Profundo, Colmeia e Vigia
            S("c19_gardens_map", "Cornifer.png", skippable: true, notRequiredFor112: true, pb: "mapRoyalGardens", arrow: 270f),
            S("c19_love_key", null, pb: "hasLoveKey", arrow: 180f),
            S("c19_marmu", null, pi: "mumCaterpillarDefeated", min: 1, arrow: 90f, prerequisites: All(B("hasDreamNail"))),
            S("c19_station", "LastStag.png", pb: "openedRoyalGardens", arrow: 90f),
            S("c19_traitor", null, pb: "killedTraitorLord", arrow: 270f, prerequisites: All(B("hasShadowDash"))),
            S("c19_queen", null, pb: "gotQueenFragment", arrow: 270f, prerequisites: All(B("killedTraitorLord"))),
            S("c20_herrah", null, pb: "hegemolDefeated", arrow: 0f, prerequisites: All(B("hasDreamNail"))),
            S("c20_weaver", null, pb: "gotCharm_39", arrow: 270f),
            S("c20_galien", null, pi: "galienDefeated", min: 1, arrow: 270f, prerequisites: All(B("hasDreamNail"))),
            S("c20_pass", null, pb: "hasTramPass", arrow: 270f),
            S("c20_zote", "Vengefly_King_Zote.png", skippable: true, notRequiredFor112: true, pb: "zoteRescuedDeepnest", arrow: 180f),
            S("c20_nosk", null, pb: "killedMimicSpider", arrow: 270f),
            S("c20_sharp", null, pb: "gotCharm_16", arrow: 90f),
            S("c21_hive", "crawlid.png", pb: "visitedHive", arrow: 90f, prerequisites: All(B("hasTramPass"))),
            S("c21_knight", null, pb: "killedHiveKnight", arrow: 90f),
            S("c21_hiveblood", null, pb: "gotCharm_29", arrow: 90f, prerequisites: All(B("killedHiveKnight"))),
            S("c22_markoth", null, pi: "markothDefeated", min: 1, arrow: 90f, prerequisites: All(B("hasDreamNail"))),
            S("c22_dashslash", null, pb: "hasDashSlash", arrow: 90f),
            S("c22_quickslash", null, pb: "gotCharm_32", arrow: 270f),
            S("c23_collector", null, pb: "collectorDefeated", arrow: 0f, prerequisites: Any(B("hasLoveKey"), B("openedLoveDoor"))),
            S("c23_grubs", "grub.png", pi: "grubsCollected", min: 44, arrow: 0f),
            S("c23_final_grub", "grub.png", grubScene: "Ruins2_03", arrow: 0f),
            S("c23_watchers", null, pb: "killedBlackKnight", arrow: 90f),
            S("c23_lurien", null, pb: "lurienDefeated", arrow: 0f, prerequisites: All(B("hasDreamNail"), B("killedBlackKnight"))),

            // Capítulos 24–30 — conclusão 112%
            S("c24_white_defender", null, pb: "killedWhiteDefender", arrow: 180f, prerequisites: All(B("hasDreamNail"), B("killedDungDefender"))),
            S("c24_colosseum", null, pb: "colosseumGoldCompleted", arrow: 0f),
            S("c25_zote", null, skippable: true, notRequiredFor112: true, pb: "killedGreyPrince", arrow: 180f),
            S("c25_grubfather", "grub.png", pb: "gotCharm_35", arrow: 270f, prerequisites: All(I("grubsCollected", 46))),
            S("c25_sly", "Sly_Basement.png", pb: "gotCharm_26", arrow: 90f),
            S("c25_awoken", null, pb: "dreamNailUpgraded", arrow: 90f),
            S("c26_flower", null, pb: "xunRewardGiven", arrow: 270f),
            S("c27_palace", null, pb: "visitedWhitePalace", arrow: 90f, prerequisites: All(B("dreamNailUpgraded"))),
            S("c27_kingsoul", null, pb: "gotKingFragment", arrow: 0f, prerequisites: All(B("visitedWhitePalace"))),
            S("c28_lifeblood", null, pb: "gotCharm_9", arrow: 180f),
            S("c28_void", null, pb: "gotShadeCharm", arrow: 180f, prerequisites: Alternatives(Alternative(B("gotCharm_36")), Alternative(B("gotQueenFragment"), B("gotKingFragment")))),
            S("c29_pure_nail", null, pi: "nailSmithUpgrades", min: 4, arrow: 270f, prerequisites: All(I("nailSmithUpgrades", 3))),
            S("c29_unbreakable", null, skippable: true, notRequiredFor112: true, allBools: new[] { "fragileHealth_unbreakable", "fragileGreed_unbreakable", "fragileStrength_unbreakable" }, arrow: 270f),
            S("c29_grimm_end", null, anyBools: new[] { "defeatedNightmareGrimm", "destroyedNightmareLantern" }, arrow: 90f, prerequisites: All(B("gotCharm_40"))),
            S("c30_godhome", null, pb: "hasGodfinder", arrow: 270f),
            S("c30_pantheons", null, pantheons: 4, arrow: 90f, prerequisites: All(B("hasGodfinder")))
        };

        private static RouteStep S(string id, string? icon = null,
            bool skippable = false, bool notRequiredFor112 = false,
            string? pb = null, string? pi = null, int min = 0,
            float arrow = 90f, string? scene = null, string? targetScene = null,
            string? transportScene = null, string? transport = null, string? grubScene = null,
            string[]? allBools = null, string[]? anyBools = null, int pantheons = 0,
            PlayerDataPrerequisite[][]? prerequisites = null)
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
                    Scene = scene,
                    GrubScene = grubScene,
                    AllPlayerBools = allBools,
                    AnyPlayerBools = anyBools,
                    PantheonCount = pantheons
                },
                ArrowDegrees = arrow,
                TargetScene = targetScene,
                TransportScene = transportScene,
                TransportInstruction = transport
                ,Prerequisites = prerequisites
            };
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

        private static PlayerDataPrerequisite[] Alternative(
            params PlayerDataPrerequisite[] conditions) => conditions;

        private static PlayerDataPrerequisite[][] Alternatives(
            params PlayerDataPrerequisite[][] alternatives) => alternatives;
    }
}

