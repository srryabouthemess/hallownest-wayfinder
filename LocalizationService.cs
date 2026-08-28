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
            public string Title { get; set; } = string.Empty;
            public string Hint { get; set; } = string.Empty;
        }

        private static readonly Dictionary<string, StepTranslation> PortugueseSteps =
            new Dictionary<string, StepTranslation>();

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
                ["complete"] = "{0} complete",
                ["skip"] = "{0} skip",
                ["back"] = "{0} back",
                ["hide"] = "{0} hide",
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
                ["later"] = "{0} later",
                ["toggle_key"] = "Key: show/hide",
                ["toggle_key_description"] = "Choose the key that shows or hides the HUD.",
                ["previous_key"] = "Key: previous step",
                ["previous_key_description"] = "Choose the key that returns to the previous step.",
                ["next_key"] = "Key: next step",
                ["next_key_description"] = "Choose the key that advances or postpones the step.",
                ["reset_route"] = "Reset route progress",
                ["reset_route_description"] = "Reset only the manual progress of the currently selected route.",
                ["keep_progress"] = "Keep",
                ["reset_now"] = "Reset now",
                ["route_completed"] = "ROUTE COMPLETE",
                ["completion_checklist"] = "Charms {0}/40  •  Masks {1}/9  •  Vessels {2}/3  •  Nail {3}/4  •  Essence {4}/2400"
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
                    return Language.Language.CurrentLanguage() != Language.LanguageCode.PT;
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
            Dictionary<string, StepTranslation> preferred =
                IsEnglish ? EnglishSteps : PortugueseSteps;
            Dictionary<string, StepTranslation> fallback =
                IsEnglish ? PortugueseSteps : EnglishSteps;
            if (preferred.TryGetValue(step.Id, out StepTranslation value)) return value.Title;
            return fallback.TryGetValue(step.Id, out value) ? value.Title : step.Id;
        }

        public static string StepHint(RouteStep step)
        {
            EnsureLoaded();
            Dictionary<string, StepTranslation> preferred =
                IsEnglish ? EnglishSteps : PortugueseSteps;
            Dictionary<string, StepTranslation> fallback =
                IsEnglish ? PortugueseSteps : EnglishSteps;
            if (preferred.TryGetValue(step.Id, out StepTranslation value)) return value.Hint;
            return fallback.TryGetValue(step.Id, out value) ? value.Hint : step.Id;
        }

        public static string? StepTransport(RouteStep step)
        {
            string? instruction = step.TransportInstruction;
            if (instruction == null || instruction.Length == 0) return null;
            return step.Id == "c03_dirtmouth"
                ? Text("last_stag_to_dirtmouth", instruction)
                : instruction;
        }

        private static void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            LoadSteps("HallownestWayfinder.Assets.localization_pt.txt", PortugueseSteps);
            LoadSteps("HallownestWayfinder.Assets.localization_en.txt", EnglishSteps);
        }

        private static void LoadSteps(string resourceName,
            Dictionary<string, StepTranslation> destination)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
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
                        destination[columns[0]] = new StepTranslation
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
