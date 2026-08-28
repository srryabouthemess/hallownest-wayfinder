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
        public void EveryGrubSceneBelongsToTheCanonicalFortySixGrubs()
        {
            RouteStep[] grubSteps = GrubRouteDefinition.Steps.ToArray();
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

            foreach (RouteStep step in SaveCompletionDefinition.Steps)
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
            NavigationWaypoint[] points = RouteDefinition.Steps
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
                typeof(RouteGlobalSettings).GetProperty("NextStepKey")?.GetValue(settings)
            };
            Assert.DoesNotContain(null, bindings);
            Assert.Equal(bindings.Length, bindings.Distinct().Count());
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

        private static IEnumerable<(RoutePlan Route, RouteStep Step)> AllSteps()
        {
            return RouteCatalog.Routes.SelectMany(
                route => route.Steps.Select(step => (route, step)));
        }

        private static HashSet<string> ReadLocalizationIds(string fileName)
        {
            string path = Path.Combine(AssetsDirectory, fileName);
            Assert.True(File.Exists(path), $"Missing localization file '{path}'.");

            return new HashSet<string>(
                File.ReadAllLines(path)
                    .Where(line => line.Length > 0 && !line.StartsWith("#", StringComparison.Ordinal))
                    .Select(line => line.Split(new[] { '|' }, 2)[0]),
                StringComparer.Ordinal);
        }
    }
}
