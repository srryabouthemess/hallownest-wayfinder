using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HallownestWayfinder
{
    public static class LocalizationService
    {
        private sealed class StepTranslation
        {
            public string Title { get; set; }
            public string Hint { get; set; }
        }

        private static readonly Dictionary<string, StepTranslation> EnglishSteps =
            new Dictionary<string, StepTranslation>();

        private static readonly Dictionary<string, string> EnglishUi =
            new Dictionary<string, string>
            {
                ["route"] = "Active route",
                ["route_description"] = "Choose the route displayed by Hallownest Wayfinder.",
                ["grubs_route"] = "Grubs 46/46",
                ["save_completion_route"] = "Save Completion",
                ["language"] = "Language",
                ["language_description"] = "Choose the language used by the mod.",
                ["automatic"] = "Automatic",
                ["portuguese"] = "Portuguese (Brazil)",
                ["english"] = "English",
                ["ui_size"] = "Interface size",
                ["ui_size_description"] = "Changes the size of the objective panel.",
                ["small"] = "Small",
                ["medium"] = "Medium",
                ["large"] = "Large",
                ["navigation_arrow"] = "Navigation arrow",
                ["navigation_description"] = "Smart uses mapped routes; General shows an approximate direction.",
                ["smart"] = "Smart",
                ["general"] = "General",
                ["off"] = "Off",
                ["optional"] = "[OPTIONAL] ",
                ["automatic_progress"] = "Automatic progress",
                ["complete"] = "F8 complete",
                ["skip"] = "F8 skip",
                ["back"] = "F7 back",
                ["hide"] = "F6 hide",
                ["navigation"] = "Navigation: ",
                ["smart_unmapped"] = "Smart navigation: section not mapped yet",
                ["arrow_approximate"] = "Arrow: general direction (approximate)",
                ["next_exit"] = "Next exit",
                ["point_reached"] = "Point reached — follow the instruction",
                ["objective_here"] = "Objective is in this room",
                ["exit_pending"] = "Route found; waiting for the room exit position",
                ["section_unmapped"] = "Section not mapped yet",
                ["general_direction"] = "General direction",
                ["last_stag_to_dirtmouth"] = "Use the Last Stag to travel to Dirtmouth",
                ["save_analyzed"] = "Save analyzed",
                ["prerequisites_missing"] = "Prerequisites not detected",
                ["later"] = "F8 later"
            };

        private static int _language;
        private static bool _loaded;

        public static void SetLanguage(int language)
        {
            _language = Math.Max(0, Math.Min(language, 2));
            EnsureLoaded();
        }

        public static bool IsEnglish
        {
            get
            {
                if (_language == 1) return false;
                if (_language == 2) return true;

                try
                {
                    string gameLanguage = GameManager.instance?.gameSettings?.gameLanguage.ToString();
                    if (!string.IsNullOrEmpty(gameLanguage))
                        return gameLanguage.IndexOf("Portugu", StringComparison.OrdinalIgnoreCase) < 0;
                }
                catch
                {
                    // Fall back to the operating-system language below.
                }

                return Application.systemLanguage != SystemLanguage.Portuguese;
            }
        }

        public static string Text(string key, string portuguese)
        {
            if (!IsEnglish) return portuguese;
            return EnglishUi.TryGetValue(key, out string value) ? value : portuguese;
        }

        public static string RouteName(RoutePlan route) =>
            IsEnglish && !string.IsNullOrEmpty(route.EnglishName) ? route.EnglishName : route.Name;

        public static string StepTitle(RouteStep step)
        {
            EnsureLoaded();
            return IsEnglish && EnglishSteps.TryGetValue(step.Id, out StepTranslation value)
                ? value.Title
                : step.Title;
        }

        public static string StepHint(RouteStep step)
        {
            EnsureLoaded();
            return IsEnglish && EnglishSteps.TryGetValue(step.Id, out StepTranslation value)
                ? value.Hint
                : step.Hint;
        }

        public static string StepTransport(RouteStep step)
        {
            if (step == null || string.IsNullOrEmpty(step.TransportInstruction)) return null;
            return step.Id == "c03_dirtmouth"
                ? Text("last_stag_to_dirtmouth", step.TransportInstruction)
                : step.TransportInstruction;
        }

        private static void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(
                "HallownestWayfinder.Assets.localization_en.txt"))
            {
                if (stream == null) return;
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                        string[] columns = line.Split(new[] { '|' }, 3);
                        if (columns.Length != 3) continue;
                        EnglishSteps[columns[0]] = new StepTranslation
                        {
                            Title = columns[1],
                            Hint = columns[2]
                        };
                    }
                }
            }
        }
    }
}
