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

        private static readonly Dictionary<string, string> PortugueseUi =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private static readonly Dictionary<string, string> EnglishUi =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private static readonly char[] ColumnSeparator = { '|' };

        private static int _language;
        private static bool _loaded;
        private static bool _isEnglish;

        public static void SetLanguage(int language)
        {
            _language = Math.Max(0, Math.Min(language, 2));
            EnsureLoaded();
            _isEnglish = DetectEnglish();
        }

        public static bool IsEnglish => _isEnglish;

        private static bool DetectEnglish()
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

        public static string Text(string key)
        {
            EnsureLoaded();
            Dictionary<string, string> preferred = IsEnglish ? EnglishUi : PortugueseUi;
            Dictionary<string, string> fallback = IsEnglish ? PortugueseUi : EnglishUi;
            if (preferred.TryGetValue(key, out string value)) return value;
            return fallback.TryGetValue(key, out value) ? value : key;
        }

        public static string RouteName(RoutePlan route)
        {
            string localized = Text("route." + route.Id);
            if (localized != "route." + route.Id) return localized;
            return IsEnglish && !string.IsNullOrEmpty(route.EnglishName) ? route.EnglishName : route.Name;
        }

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
                ? Text("last_stag_to_dirtmouth")
                : instruction;
        }

        private static void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            Load("HallownestWayfinder.Assets.localization_pt.txt", PortugueseSteps, PortugueseUi);
            Load("HallownestWayfinder.Assets.localization_en.txt", EnglishSteps, EnglishUi);
        }

        private static void Load(string resourceName,
            Dictionary<string, StepTranslation> steps, Dictionary<string, string> ui)
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
                        if (string.IsNullOrWhiteSpace(line) ||
                            line.StartsWith("#", StringComparison.Ordinal)) continue;
                        string[] columns = line.Split(ColumnSeparator, 3);
                        if (columns.Length >= 2 && columns[0].StartsWith("@", StringComparison.Ordinal))
                        {
                            ui[columns[0].Substring(1)] = columns[1];
                            continue;
                        }
                        if (columns.Length != 3) continue;
                        steps[columns[0]] = new StepTranslation
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
