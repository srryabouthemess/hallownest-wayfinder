using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace HallownestWayfinder.Tests
{
    public sealed class RouteDataTests
    {
        private static readonly string AssetsDirectory =
            Path.Combine(AppContext.BaseDirectory, "Assets");
        private static readonly char[] LocalizationSeparator = { '|' };

        [Fact]
        public void EveryReferencedIconExists()
        {
            foreach ((RoutePlan route, RouteStep step) in AllSteps())
            {
                if (string.IsNullOrEmpty(step.Icon)) continue;

                string path = Path.Combine(AssetsDirectory, step.Icon);
                Assert.True(File.Exists(path),
                    $"Route '{route.Id}', step '{step.Id}' references missing icon '{step.Icon}'.");
            }
        }

        [Fact]
        public void EmbeddedRouteDataLoadsEveryCanonicalRouteAndStep()
        {
            Dictionary<string, int> expected = new Dictionary<string, int>
            {
                ["completion_112"] = 155,
                ["speedrun_5h"] = 49,
                ["grubs_46"] = 44,
                ["save_completion"] = 154
            };

            Assert.Equal(expected.Count, RouteCatalog.Routes.Count);
            foreach (RoutePlan route in RouteCatalog.Routes)
            {
                Assert.True(expected.TryGetValue(route.Id, out int stepCount),
                    $"Unexpected route '{route.Id}'.");
                Assert.Equal(stepCount, route.Steps.Count);
            }
        }

        [Fact]
        public void EveryGrubSceneBelongsToTheCanonicalFortySixGrubs()
        {
            RouteStep[] grubSteps = Route("grubs_46").Steps.ToArray();
            HashSet<string> scenes = new HashSet<string>(
                grubSteps.Select(step => step.Completion.GrubScene!),
                StringComparer.Ordinal);

            Assert.Equal(46, grubSteps.Sum(step => step.Completion.GrubCountInScene));
            foreach ((RoutePlan route, RouteStep step) in AllSteps())
            {
                string? scene = step.Completion.GrubScene;
                if (scene == null || scene.Length == 0) continue;

                Assert.Contains(scene, scenes);
            }
        }

        [Fact]
        public void StepIdsAreUniqueInsideEachRoute()
        {
            foreach (RoutePlan route in RouteCatalog.Routes)
            {
                string[] duplicates = route.Steps
                    .GroupBy(step => step.Id, StringComparer.Ordinal)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key)
                    .ToArray();

                Assert.True(duplicates.Length == 0,
                    $"Route '{route.Id}' has duplicate IDs: {string.Join(", ", duplicates)}");
            }
        }

        [Fact]
        public void EveryStepHasEnglishAndPortugueseLocalization()
        {
            HashSet<string> english = ReadLocalizationIds("localization_en.txt");
            HashSet<string> portuguese = ReadLocalizationIds("localization_pt.txt");

            foreach ((RoutePlan route, RouteStep step) in AllSteps())
            {
                Assert.True(english.Contains(step.Id),
                    $"Route '{route.Id}', step '{step.Id}' is missing English localization.");
                Assert.True(portuguese.Contains(step.Id),
                    $"Route '{route.Id}', step '{step.Id}' is missing Portuguese localization.");
            }
        }

        [Fact]
        public void InterfaceLocalizationKeysMatchBetweenLanguages()
        {
            HashSet<string> english = ReadUiLocalizationKeys("localization_en.txt");
            HashSet<string> portuguese = ReadUiLocalizationKeys("localization_pt.txt");

            Assert.NotEmpty(english);
            Assert.True(english.SetEquals(portuguese),
                "Portuguese and English interface localization keys differ.");
        }

        [Fact]
        public void EveryPlayerDataReferenceExistsWithTheExpectedType()
        {
            IReadOnlyList<string> errors = RouteDataValidator.Validate(RouteCatalog.Routes);
            Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));
        }

        [Fact]
        public void SaveCompletionPrerequisitesAreStoredOnSteps()
        {
            HashSet<string> routeIds = new HashSet<string>(
                RouteCatalog.Routes.SelectMany(route => route.Steps).Select(step => step.Id),
                StringComparer.Ordinal);

            foreach (RouteStep step in Route("save_completion").Steps)
            {
                Assert.Contains(step.Id, routeIds);
                if (step.Prerequisites == null) continue;
                Assert.All(step.Prerequisites, alternative => Assert.NotEmpty(alternative));
            }
        }

        [Fact]
        public void NavigationRequirementsReferenceBooleanPlayerDataFields()
        {
            IReadOnlyList<string> errors = VanillaRouteGraph.ValidateRequirements();
            Assert.True(errors.Count == 0, string.Join(Environment.NewLine, errors));

            string[] transitions = File.ReadAllLines(
                Path.Combine(AssetsDirectory, "vanilla_transitions.txt"));
            string[][] rows = transitions
                .Where(line => line.Length > 0 && !line.StartsWith("#", StringComparison.Ordinal))
                .Select(line => line.Split('|'))
                .ToArray();
            Assert.All(rows, columns => Assert.Equal(5, columns.Length));

            string[][] gated = rows
                .Where(columns => columns[4].Length > 0)
                .ToArray();
            Assert.NotEmpty(gated);
        }

        [Fact]
        public void InitialRouteContainsAuthoredObjectWaypoints()
        {
            NavigationWaypoint[] points = Route("completion_112").Steps
                .Take(4)
                .Where(step => step.Navigation != null)
                .SelectMany(step => step.Navigation!)
                .ToArray();

            Assert.True(points.Length >= 4);
            Assert.All(points, point => Assert.False(string.IsNullOrEmpty(point.TargetObjectName)));
        }

        [Fact]
        public void DefaultKeyBindingsDoNotConflict()
        {
            RouteGlobalSettings settings = new RouteGlobalSettings();
            object?[] bindings =
            {
                typeof(RouteGlobalSettings).GetProperty("ToggleHudKey")?.GetValue(settings),
                typeof(RouteGlobalSettings).GetProperty("PreviousStepKey")?.GetValue(settings),
                typeof(RouteGlobalSettings).GetProperty("NextStepKey")?.GetValue(settings),
                typeof(RouteGlobalSettings).GetProperty("RecordWaypointKey")?.GetValue(settings),
                typeof(RouteGlobalSettings).GetProperty("ToggleFreeNavigationKey")?.GetValue(settings)
            };
            Assert.DoesNotContain(null, bindings);
            Assert.Equal(bindings.Length, bindings.Distinct().Count());
        }

        [Fact]
        public void WaypointRecorderSerializesCoordinatesForOpenRoomPoints()
        {
            NavigationWaypoint point = WaypointRecorder.CreateWaypoint(
                "Town", 12.345f, -4.566f, 0, null, 2.5f);

            string json = WaypointRecorder.SerializeSnippet(new[] { point });

            Assert.Contains("\"Navigation\"", json);
            Assert.Contains("\"X\": 12.35", json);
            Assert.Contains("\"Y\": -4.57", json);
            Assert.DoesNotContain("TargetObjectName", json);
        }

        [Fact]
        public void WaypointRecorderPrefersStableObjectNameForDoorPoints()
        {
            NavigationWaypoint point = WaypointRecorder.CreateWaypoint(
                "Town", 12f, -4f, 1, "bot1", 2.5f);

            string json = WaypointRecorder.SerializeSnippet(new[] { point });

            Assert.Contains("\"TargetObjectName\": \"bot1\"", json);
            Assert.DoesNotContain("\"X\"", json);
            Assert.DoesNotContain("\"Y\"", json);
        }

        [Fact]
        public void WaypointRecorderSelectsOnlyDoorsInsideCaptureRadius()
        {
            WaypointDoorCandidate[] doors =
            {
                new WaypointDoorCandidate { Name = "left1", X = 0f, Y = 0f },
                new WaypointDoorCandidate { Name = "right1", X = 5f, Y = 0f }
            };

            Assert.Equal("right1", WaypointRecorder.SelectNearestDoor(doors, 4f, 0f, 3f));
            Assert.Null(WaypointRecorder.SelectNearestDoor(doors, 4f, 0f, 0.5f));
        }

        [Fact]
        public void FreeNavigationDestinationsAreUniqueMappedScenes()
        {
            IReadOnlyList<FreeNavigationDestination> destinations =
                FreeNavigationCatalog.Destinations;

            Assert.Equal(destinations.Count + 1,
                FreeNavigationCatalog.MenuNames().Length);
            Assert.Equal(destinations.Count,
                destinations.Select(destination => destination.Id).Distinct().Count());
            Assert.Equal(destinations.Count,
                destinations.Select(destination => destination.Scene).Distinct().Count());
            Assert.All(destinations,
                destination => Assert.True(VanillaRouteGraph.ContainsScene(destination.Scene),
                    $"Free-navigation scene '{destination.Scene}' is not in the graph."));
        }

        [Fact]
        public void FreeNavigationCanRouteFromDirtmouthToCrossroadsStation()
        {
            VanillaRouteGraph.SetGameState(new FakeGameState());

            Assert.True(VanillaRouteGraph.TryGetNextDoor(
                "Town", "Crossroads_47", out string? door));
            Assert.Equal("bot1", door);
        }

        [Fact]
        public void CompletionChecklistUsesPersistentPlayerDataCounters()
        {
            CompletionChecklistSnapshot snapshot = CompletionChecklist.FromValues(
                charms: 38, maxHealthBase: 8, reserveSoulMaximum: 66,
                nailUpgrades: 3, currentEssence: 1200, spentEssence: 300);

            Assert.Equal(38, snapshot.Charms);
            Assert.Equal(8, snapshot.Masks);
            Assert.Equal(2, snapshot.Vessels);
            Assert.Equal(3, snapshot.NailUpgrades);
            Assert.Equal(1500, snapshot.Essence);
        }

        [Fact]
        public void DetailedCompletionChecklistAccountsForAll112PercentagePoints()
        {
            FakeGameState state = new FakeGameState
            {
                CharmsOwned = CompletionChecklist.TotalCharms,
                MaxHealthBase = CompletionChecklist.TotalMasks,
                SoulReserveMaximum = CompletionChecklist.TotalVessels * 33,
                NailUpgrades = CompletionChecklist.TotalNailUpgrades,
                CurrentEssence = CompletionChecklist.TotalEssence,
                CompletedPantheons = 4
            };
            foreach (string field in CompletionChecklist.ReferencedPlayerBools)
                state.Bools[field] = true;
            foreach (string field in CompletionChecklist.ReferencedPlayerInts)
                state.Ints[field] = 1;
            state.Ints["fireballLevel"] = 2;
            state.Ints["quakeLevel"] = 2;
            state.Ints["screamLevel"] = 2;

            CompletionChecklistSnapshot snapshot = CompletionChecklist.Read(state);

            Assert.Equal(CompletionChecklist.TotalPercentage, snapshot.Percentage);
            Assert.Equal(CompletionChecklist.TotalBosses, snapshot.Bosses);
            Assert.Equal(CompletionChecklist.TotalWarriorDreams, snapshot.WarriorDreams);
            Assert.Equal(CompletionChecklist.TotalEquipment, snapshot.Equipment);
            Assert.Equal(CompletionChecklist.TotalSpells, snapshot.Spells);
            Assert.Equal(CompletionChecklist.TotalNailArts, snapshot.NailArts);
            Assert.Equal(CompletionChecklist.TotalDreamers, snapshot.Dreamers);
            Assert.Equal(CompletionChecklist.TotalColosseum, snapshot.Colosseum);
            Assert.Equal(CompletionChecklist.TotalGrimmTroupe, snapshot.GrimmTroupe);
            Assert.Equal(CompletionChecklist.TotalHive, snapshot.Hive);
            Assert.Equal(CompletionChecklist.TotalGodhome, snapshot.Godhome);
            Assert.Equal(6, CompletionChecklist.Format(snapshot, true)
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length);
        }

        [Fact]
        public void SeerFinalWordsRequireThe2400EssenceReward()
        {
            RouteStep routeStep = Route("completion_112").Steps.Single(
                step => step.Id == "c25_awoken");
            RouteStep saveStep = Route("save_completion").Steps.Single(
                step => step.Id == "c25_awoken");

            Assert.Equal("dreamReward9", routeStep.Completion.PlayerBool);
            Assert.Equal("dreamReward9", saveStep.Completion.PlayerBool);
        }

        [Fact]
        public void ChecklistPlayerDataReferencesExistWithExpectedTypes()
        {
            Assert.Empty(CompletionChecklist.ValidatePlayerDataReferences());
        }

        [Fact]
        public void CompletionUsesVisitedSceneAndMemoizedStateCollections()
        {
            FakeGameState state = new FakeGameState();
            state.VisitedScenes.Add("Town");
            RouteCompletion completion = new RouteCompletion { Scene = "Town" };

            Assert.True(completion.IsComplete(state));
            Assert.False(new RouteCompletion { Scene = "Crossroads_01" }.IsComplete(state));
        }

        [Fact]
        public void SaveCompletionSelectsAvailableStepBeforeBlockedStep()
        {
            FakeGameState state = new FakeGameState();
            state.Bools["done"] = true;
            state.Bools["locked"] = false;
            RouteStep[] steps =
            {
                new RouteStep
                {
                    Id = "done",
                    Completion = new RouteCompletion { PlayerBool = "done" }
                },
                new RouteStep
                {
                    Id = "blocked",
                    Completion = new RouteCompletion { PlayerBool = "blockedDone" },
                    Prerequisites = new[]
                    {
                        new[] { PlayerDataPrerequisite.Bool("locked") }
                    }
                },
                new RouteStep
                {
                    Id = "available",
                    Completion = new RouteCompletion { PlayerBool = "availableDone" }
                }
            };

            Assert.Equal(2, SaveCompletionAnalyzer.FindNextStep(
                steps, Array.Empty<string>(), state));
            Assert.Equal(1, SaveCompletionAnalyzer.CountCompleted(steps, state));
        }

        [Fact]
        public void SaveCompletionFallsBackToDismissedOrBlockedStep()
        {
            FakeGameState state = new FakeGameState();
            state.Bools["locked"] = false;
            RouteStep blocked = new RouteStep
            {
                Id = "blocked",
                Completion = new RouteCompletion { PlayerBool = "done" },
                Prerequisites = new[]
                {
                    new[] { PlayerDataPrerequisite.Bool("locked") }
                }
            };

            Assert.Equal(0, SaveCompletionAnalyzer.FindNextStep(
                new List<RouteStep> { blocked }, new List<string> { "blocked" }, state));
        }

        [Fact]
        public void NavigationGraphInvalidatesCacheWhenAccessChanges()
        {
            FakeGameState blocked = new FakeGameState();
            blocked.Bools["hasKingsBrand"] = false;
            VanillaRouteGraph.SetGameState(blocked);
            Assert.False(VanillaRouteGraph.TryGetNextDoor(
                "Abyss_04", "Abyss_06_Core", out _));

            FakeGameState unlocked = new FakeGameState();
            unlocked.Bools["hasKingsBrand"] = true;
            VanillaRouteGraph.SetGameState(unlocked);
            Assert.True(VanillaRouteGraph.TryGetNextDoor(
                "Abyss_04", "Abyss_06_Core", out string? door));
            Assert.Equal("bot1", door);
        }

        [Fact]
        public void VersionFourProgressMigratesToRouteDictionary()
        {
            RouteProgress progress = new RouteProgress
            {
                DataVersion = 4,
                CurrentStep = 11,
                SpeedrunCurrentStep = 7,
                GrubCurrentStep = 23,
                StepByRoute = new Dictionary<string, int>()
            };

            progress.MigrateRouteDictionary();

            Assert.Equal(5, progress.DataVersion);
            Assert.Equal(11, progress.StepByRoute["completion_112"]);
            Assert.Equal(7, progress.StepByRoute["speedrun_5h"]);
            Assert.Equal(23, progress.StepByRoute["grubs_46"]);
        }

        private static IEnumerable<(RoutePlan Route, RouteStep Step)> AllSteps()
        {
            return RouteCatalog.Routes.SelectMany(
                route => route.Steps.Select(step => (route, step)));
        }

        private static RoutePlan Route(string id) =>
            RouteCatalog.Routes.Single(route => route.Id == id);

        private static HashSet<string> ReadLocalizationIds(string fileName)
        {
            string path = Path.Combine(AssetsDirectory, fileName);
            Assert.True(File.Exists(path), $"Missing localization file '{path}'.");

            return new HashSet<string>(
                File.ReadAllLines(path)
                    .Where(line => line.Length > 0 && !line.StartsWith("#", StringComparison.Ordinal))
                    .Select(line => line.Split(LocalizationSeparator, 2)[0]),
                StringComparer.Ordinal);
        }

        private static HashSet<string> ReadUiLocalizationKeys(string fileName)
        {
            return new HashSet<string>(
                File.ReadAllLines(Path.Combine(AssetsDirectory, fileName))
                    .Where(line => line.StartsWith("@", StringComparison.Ordinal))
                    .Select(line => line.Split(LocalizationSeparator, 2)[0].Substring(1)),
                StringComparer.Ordinal);
        }

        private sealed class FakeGameState : IGameState
        {
            public Dictionary<string, bool> Bools { get; } =
                new Dictionary<string, bool>(StringComparer.Ordinal);
            public Dictionary<string, int> Ints { get; } =
                new Dictionary<string, int>(StringComparer.Ordinal);
            public HashSet<string> VisitedScenes { get; } =
                new HashSet<string>(StringComparer.Ordinal);
            public HashSet<string> RescuedGrubs { get; } =
                new HashSet<string>(StringComparer.Ordinal);
            public string SceneName { get; set; } = string.Empty;
            public bool AtBench { get; set; }
            public string RespawnScene { get; set; } = string.Empty;
            public int RelicCount { get; set; }
            public int CompletedPantheons { get; set; }
            public int GrubsCollected { get; set; }
            public int GrubSceneCount => RescuedGrubs.Count;
            public int CharmsOwned { get; set; }
            public int MaxHealthBase { get; set; }
            public int SoulReserveMaximum { get; set; }
            public int NailUpgrades { get; set; }
            public int CurrentEssence { get; set; }
            public int SpentEssence { get; set; }

            public bool GetBool(string field) =>
                Bools.TryGetValue(field, out bool value) && value;
            public int GetInt(string field) =>
                Ints.TryGetValue(field, out int value) ? value : 0;
            public bool HasVisitedScene(string scene) => VisitedScenes.Contains(scene);
            public bool HasRescuedGrub(string scene) => RescuedGrubs.Contains(scene);
        }
    }
}
