using System.Collections.Generic;
using System.Linq;

namespace HallownestWayfinder
{
    public static class RouteDefinition
    {
        public const string Name = "Rota 112%";

        public static readonly IReadOnlyList<RouteStep> Steps = new List<RouteStep>
        {
            Step("fury", "Fury_of_the_Fallen.png", 135f, playerBool: "gotCharm_6", targetScene: "Tutorial_01"),
            Step("dirtmouth", "Elderbug.png", 90f, scene: "Town",
                navigation: Waypoints(Waypoint("Tutorial_01", "right1"))),
            Step("crossroads", "crawlid.png", 180f, scene: "Crossroads_01",
                navigation: Waypoints(Waypoint("Town", "bot1"))),
            Step("crossroads_map", "Cornifer.png", 225f, skippable: true, notRequiredFor112: true, playerBool: "mapCrossroads", targetScene: "Crossroads_33",
                navigation: Waypoints(Waypoint("Crossroads_01", "left1"), Waypoint("Crossroads_07", "bot1"))),
            Step("grub_1", "grub.png", 270f, grubScene: "Crossroads_35"),
            Step("crossroads_station", "LastStag.png", 225f, playerBool: "openedCrossroads", targetScene: "Crossroads_47"),
            Step("grub_2", "grub.png", 0f, grubScene: "Crossroads_03"),
            Step("grub_3", "grub.png", 45f, grubScene: "Crossroads_48"),
            Step("grub_4", "grub.png", 135f, grubScene: "Crossroads_31"),
            Step("gruz_mother", "gruz_mother.png", 180f, playerBool: "killedBigFly", targetScene: "Crossroads_04"),
            Step("sly", "Sly_Basement.png", 90f, playerBool: "slyRescued", targetScene: "Room_shop"),
            Step("false_knight", "False_Knight.png", 315f, playerBool: "killedFalseKnight", targetScene: "Crossroads_10"),
            Step("vengeful_spirit", "Vengeful_Spirit_Icon.png", 270f, playerBool: "hasSpell", targetScene: "Crossroads_ShamanTemple"),
            Step("soul_catcher", "Soul_Catcher.png", 90f, playerBool: "gotCharm_20", targetScene: "Crossroads_ShamanTemple", prerequisites: All(B("hasSpell"))),
            Step("greenpath", "Elder_Baldur.png", 270f, scene: "Fungus1_01"),
            Step("grub_5", "grub.png", 135f, grubScene: "Fungus1_06"),
            Step("greenpath_map", "Cornifer.png", 270f, skippable: true, notRequiredFor112: true, playerBool: "mapGreenpath", targetScene: "Fungus1_06"),
            Step("hunters_journal", "Hunter.png", 135f, playerBool: "hasJournal", targetScene: "Fungus1_08"),
            Step("grub_6", "grub.png", 225f, grubScene: "Fungus1_07"),
            Step("greenpath_bench", "bench.png", 315f, playerBool: "atBench"),
            Step("zote", "Vengefly_King_Zote.png", 315f, playerBool: "zoteRescuedBuzzer", targetScene: "Fungus1_20_v02"),
            Step("grub_7", "grub.png", 90f, grubScene: "Fungus1_21"),
            Step("greenpath_station", "LastStag.png", 180f, playerBool: "openedGreenpath", targetScene: "Fungus1_16_alt"),
            Step("wanderers_journal", "Wanderers_Journal.png", 270f, playerInt: "trinket1", minimum: 1, targetScene: "Fungus1_22"),
            Step("hornet", "Hornet.png", 270f, playerBool: "hasDash", targetScene: "Fungus1_04")
        }.Concat(GuideContinuation.Steps).ToList();

        private static RouteStep Step(string id, string? icon, float arrowDegrees = 0f,
            bool skippable = false, bool notRequiredFor112 = false,
            string? scene = null, string? playerBool = null,
            string? playerInt = null, int minimum = 0, string? targetScene = null,
            string? grubScene = null, PlayerDataPrerequisite[][]? prerequisites = null,
            NavigationWaypoint[]? navigation = null)
        {
            return new RouteStep
            {
                Id = id,
                Icon = icon,
                ArrowDegrees = arrowDegrees,
                SkippableInRoute = skippable,
                NotRequiredFor112 = notRequiredFor112,
                Completion = new RouteCompletion
                {
                    Scene = scene,
                    PlayerBool = playerBool,
                    PlayerInt = playerInt,
                    Minimum = minimum,
                    GrubScene = grubScene
                }
                ,TargetScene = targetScene
                ,Prerequisites = prerequisites
                ,Navigation = navigation
            };
        }

        private static NavigationWaypoint Waypoint(string scene, string targetObjectName) =>
            new NavigationWaypoint { Scene = scene, TargetObjectName = targetObjectName };

        private static NavigationWaypoint[] Waypoints(params NavigationWaypoint[] points) => points;

        private static PlayerDataPrerequisite B(string field) =>
            PlayerDataPrerequisite.Bool(field);

        private static PlayerDataPrerequisite[][] All(params PlayerDataPrerequisite[] conditions) =>
            new[] { conditions };
    }
}

