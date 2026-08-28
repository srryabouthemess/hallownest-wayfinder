using System;
using System.Collections.Generic;
using System.Reflection;
using Modding;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace HallownestWayfinder
{
    public sealed class HallownestWayfinderMod : Mod, ITogglableMod, IMenuMod,
        IGlobalSettings<RouteGlobalSettings>, ILocalSettings<RouteProgress>
    {
        private enum KeyBindingAction
        {
            ToggleHud,
            PreviousStep,
            NextStep
        }

        private static readonly KeyCode[] SupportedKeys =
        {
            KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4,
            KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8,
            KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12,
            KeyCode.Home, KeyCode.End, KeyCode.Insert, KeyCode.Delete,
            KeyCode.PageUp, KeyCode.PageDown
        };

        private static HallownestWayfinderMod? _instance;
        private GameObject? _hudObject;
        private readonly HashSet<string> _automaticAdvanceErrors =
            new HashSet<string>(StringComparer.Ordinal);
        private IGameState? _gameState;
        private float _nextGameStateRefresh;
        private string? _analysisRouteId;
        private int _cachedSaveStep = -1;
        private int _cachedCompletedSteps;
        private bool _cachedStepAvailable;

        public RouteProgress Progress { get; private set; } = new RouteProgress();
        public RouteGlobalSettings GlobalSettings { get; private set; } = new RouteGlobalSettings();
        public bool ToggleButtonInsideMenu => true;
        public RoutePlan CurrentRoute => RouteCatalog.Routes[GlobalSettings.ActiveRoute];
        public bool IsSaveCompletion => CurrentRoute.IsSaveCompletion;
        public int CurrentStepIndex
        {
            get
            {
                if (IsSaveCompletion)
                {
                    RefreshGameState();
                    return _cachedSaveStep;
                }
                return GetStoredProgress(CurrentRoute.Id);
            }
            private set
            {
                if (IsSaveCompletion) return;
                Progress.StepByRoute[CurrentRoute.Id] = value;
            }
        }
        public bool HasActiveStep => CurrentStepIndex >= 0 && CurrentStepIndex < CurrentRoute.Steps.Count;
        public bool IsRouteComplete => CompletedStepCount >= CurrentRoute.Steps.Count;
        public RouteStep? CurrentStep => HasActiveStep ? CurrentRoute.Steps[CurrentStepIndex] : null;
        public int CompletedStepCount => IsSaveCompletion
            ? GetSaveCompletedCount()
            : CurrentStepIndex;
        public bool CurrentStepIsAvailable
        {
            get
            {
                if (!IsSaveCompletion) return true;
                RefreshGameState();
                return _cachedStepAvailable;
            }
        }
        public IGameState? CurrentGameState
        {
            get
            {
                RefreshGameState();
                return _gameState;
            }
        }

        public override string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string? informationalVersion = assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;
            if (informationalVersion != null && informationalVersion.Length > 0)
                return informationalVersion.Split('+')[0];

            return assembly.GetName().Version?.ToString() ?? "0.0.0";
        }

        public override void Initialize()
        {
            if (_instance != null) return;
            _instance = this;
            _automaticAdvanceErrors.Clear();

            foreach (string error in RouteDataValidator.Validate(RouteCatalog.Routes))
                LogError("Invalid route data: " + error);
            foreach (string error in VanillaRouteGraph.ValidateRequirements())
                LogError("Invalid navigation graph: " + error);

            _hudObject = new GameObject("HallownestWayfinder HUD");
            UObject.DontDestroyOnLoad(_hudObject);
            RouteHud hud = _hudObject.AddComponent<RouteHud>();
            hud.Mod = this;
            Log("Hallownest Wayfinder loaded. Configurable controls are available in the mod menu.");
        }

        public void Unload()
        {
            if (_hudObject != null) UObject.Destroy(_hudObject);
            _hudObject = null;
            _instance = null;
        }

        public void OnLoadLocal(RouteProgress? settings)
        {
            Progress = settings ?? new RouteProgress();

            // Version 2 removed Myla, which occupied the old index 7.
            if (Progress.DataVersion < 2)
            {
                if (Progress.CurrentStep > 7) Progress.CurrentStep--;
                Progress.DataVersion = 2;
            }

            if (Progress.DataVersion < 3) Progress.DataVersion = 3;
            if (Progress.SaveCompletionDismissedStepIds == null)
                Progress.SaveCompletionDismissedStepIds = new List<string>();
            if (Progress.DataVersion < 4) Progress.DataVersion = 4;

            Progress.MigrateRouteDictionary();

            ClampProgress();
            InvalidateGameState();
        }

        public RouteProgress OnSaveLocal() => Progress;

        public void OnLoadGlobal(RouteGlobalSettings? settings)
        {
            GlobalSettings = settings ?? new RouteGlobalSettings();
            GlobalSettings.UiSize = Math.Max(0, Math.Min(GlobalSettings.UiSize, 2));
            GlobalSettings.NavigationMode = Math.Max(0, Math.Min(GlobalSettings.NavigationMode, 2));
            GlobalSettings.ActiveRoute = Math.Max(0, Math.Min(GlobalSettings.ActiveRoute, RouteCatalog.Routes.Count - 1));
            GlobalSettings.Language = Math.Max(0, Math.Min(GlobalSettings.Language, 2));
            GlobalSettings.ToggleHudKey = NormalizeKey(GlobalSettings.ToggleHudKey, KeyCode.F6);
            GlobalSettings.PreviousStepKey = NormalizeKey(GlobalSettings.PreviousStepKey, KeyCode.F7);
            GlobalSettings.NextStepKey = NormalizeKey(GlobalSettings.NextStepKey, KeyCode.F8);
            LocalizationService.SetLanguage(GlobalSettings.Language);
        }

        public RouteGlobalSettings OnSaveGlobal() => GlobalSettings;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            List<IMenuMod.MenuEntry> entries = new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry
                {
                    Name = LocalizationService.Text("route"),
                    Description = LocalizationService.Text("route_description"),
                    Values = RouteMenuNames(),
                    Saver = value =>
                    {
                        GlobalSettings.ActiveRoute = value;
                        ClampProgress();
                        InvalidateGameState();
                    },
                    Loader = () => GlobalSettings.ActiveRoute
                },
                new IMenuMod.MenuEntry
                {
                    Name = LocalizationService.Text("language"),
                    Description = LocalizationService.Text("language_description"),
                    Values = new[]
                    {
                        LocalizationService.Text("automatic"),
                        LocalizationService.Text("portuguese"),
                        LocalizationService.Text("english")
                    },
                    Saver = value =>
                    {
                        GlobalSettings.Language = value;
                        LocalizationService.SetLanguage(value);
                    },
                    Loader = () => GlobalSettings.Language
                },
                new IMenuMod.MenuEntry
                {
                    Name = LocalizationService.Text("ui_size"),
                    Description = LocalizationService.Text("ui_size_description"),
                    Values = new[]
                    {
                        LocalizationService.Text("small"),
                        LocalizationService.Text("medium"),
                        LocalizationService.Text("large")
                    },
                    Saver = value => GlobalSettings.UiSize = value,
                    Loader = () => GlobalSettings.UiSize
                },
                new IMenuMod.MenuEntry
                {
                    Name = LocalizationService.Text("navigation_arrow"),
                    Description = LocalizationService.Text("navigation_description"),
                    Values = new[]
                    {
                        LocalizationService.Text("smart"),
                        LocalizationService.Text("general"),
                        LocalizationService.Text("off")
                    },
                    Saver = value => GlobalSettings.NavigationMode = value,
                    Loader = () => GlobalSettings.NavigationMode
                }
            };

            entries.Add(CreateKeyEntry(
                LocalizationService.Text("toggle_key"),
                LocalizationService.Text("toggle_key_description"),
                KeyBindingAction.ToggleHud));
            entries.Add(CreateKeyEntry(
                LocalizationService.Text("previous_key"),
                LocalizationService.Text("previous_key_description"),
                KeyBindingAction.PreviousStep));
            entries.Add(CreateKeyEntry(
                LocalizationService.Text("next_key"),
                LocalizationService.Text("next_key_description"),
                KeyBindingAction.NextStep));
            entries.Add(new IMenuMod.MenuEntry
            {
                Name = LocalizationService.Text("reset_route"),
                Description = LocalizationService.Text("reset_route_description"),
                Values = new[]
                {
                    LocalizationService.Text("keep_progress"),
                    LocalizationService.Text("reset_now")
                },
                Saver = value =>
                {
                    if (value == 1) ResetCurrentRoute();
                },
                Loader = () => 0
            });

            if (toggleButtonEntry.HasValue) entries.Insert(0, toggleButtonEntry.Value);
            return entries;
        }

        public void ToggleVisibility() => Progress.Visible = !Progress.Visible;

        public void ResetCurrentRoute()
        {
            if (IsSaveCompletion) Progress.SaveCompletionDismissedStepIds.Clear();
            else Progress.StepByRoute[CurrentRoute.Id] = 0;
            _automaticAdvanceErrors.Clear();
            InvalidateGameState();
        }

        public void NextStep()
        {
            if (IsSaveCompletion)
            {
                RouteStep? step = CurrentStep;
                if (step != null && !Progress.SaveCompletionDismissedStepIds.Contains(step.Id))
                    Progress.SaveCompletionDismissedStepIds.Add(step.Id);
                InvalidateGameState();
                return;
            }

            if (CurrentStepIndex < CurrentRoute.Steps.Count)
                CurrentStepIndex++;
        }

        public void PreviousStep()
        {
            if (IsSaveCompletion)
            {
                int count = Progress.SaveCompletionDismissedStepIds.Count;
                if (count > 0) Progress.SaveCompletionDismissedStepIds.RemoveAt(count - 1);
                InvalidateGameState();
                return;
            }

            if (CurrentStepIndex > 0)
                CurrentStepIndex--;
        }

        public void TryAdvanceAutomatically()
        {
            if (!HasActiveStep || GameManager.instance == null || PlayerData.instance == null) return;

            RefreshGameState();
            IGameState? state = _gameState;
            if (state == null) return;

            RoutePlan route = CurrentRoute;
            RouteStep? step = CurrentStep;
            if (step == null) return;
            try
            {
                if (step.IsComplete(state) && !IsSaveCompletion) NextStep();
            }
            catch (Exception exception)
            {
                string errorKey = route.Id + "\n" + step.Id;
                if (_automaticAdvanceErrors.Add(errorKey))
                {
                    LogError("Failed to evaluate step '" + step.Id +
                        "' from route '" + route.Id + "': " + exception);
                }
            }
        }

        private void ClampProgress()
        {
            foreach (RoutePlan route in RouteCatalog.Routes)
            {
                if (route.IsSaveCompletion) continue;
                int progress = GetStoredProgress(route.Id);
                Progress.StepByRoute[route.Id] = Math.Max(0, Math.Min(progress, route.Steps.Count));
            }
        }

        public void RefreshGameState(bool force = false)
        {
            string routeId = CurrentRoute.Id;
            if (!force && _gameState != null && _analysisRouteId == routeId &&
                Time.unscaledTime < _nextGameStateRefresh)
                return;

            _nextGameStateRefresh = Time.unscaledTime + 0.25f;
            _analysisRouteId = routeId;
            if (!PlayerDataGameState.TryCapture(out PlayerDataGameState? state) || state == null)
            {
                _gameState = null;
                _cachedSaveStep = -1;
                _cachedCompletedSteps = 0;
                _cachedStepAvailable = false;
                return;
            }

            _gameState = state;
            VanillaRouteGraph.SetGameState(state);
            if (!IsSaveCompletion) return;

            _cachedSaveStep = SaveCompletionAnalyzer.FindNextStep(CurrentRoute.Steps,
                Progress.SaveCompletionDismissedStepIds, state);
            _cachedCompletedSteps = SaveCompletionAnalyzer.CountCompleted(CurrentRoute.Steps, state);
            _cachedStepAvailable = _cachedSaveStep >= 0 &&
                _cachedSaveStep < CurrentRoute.Steps.Count &&
                SaveCompletionAnalyzer.IsAvailable(CurrentRoute.Steps[_cachedSaveStep], state);
        }

        private int GetSaveCompletedCount()
        {
            RefreshGameState();
            return _cachedCompletedSteps;
        }

        private int GetStoredProgress(string routeId)
        {
            return Progress.StepByRoute.TryGetValue(routeId, out int value) ? value : 0;
        }

        private void InvalidateGameState()
        {
            _nextGameStateRefresh = 0f;
            _analysisRouteId = null;
        }

        private static string[] RouteMenuNames()
        {
            string[] names = new string[RouteCatalog.Routes.Count];
            for (int index = 0; index < names.Length; index++)
                names[index] = LocalizationService.RouteName(RouteCatalog.Routes[index]);
            return names;
        }

        public static string KeyName(KeyCode key) => key.ToString();

        private IMenuMod.MenuEntry CreateKeyEntry(string name, string description,
            KeyBindingAction action)
        {
            string[] values = new string[SupportedKeys.Length];
            for (int index = 0; index < SupportedKeys.Length; index++)
                values[index] = KeyName(SupportedKeys[index]);

            return new IMenuMod.MenuEntry
            {
                Name = name,
                Description = description,
                Values = values,
                Saver = value => SetKeyBinding(action,
                    SupportedKeys[Math.Max(0, Math.Min(value, SupportedKeys.Length - 1))]),
                Loader = () => KeyIndex(GetKeyBinding(action))
            };
        }

        private void SetKeyBinding(KeyBindingAction action, KeyCode key)
        {
            KeyCode previous = GetKeyBinding(action);
            foreach (KeyBindingAction other in Enum.GetValues(typeof(KeyBindingAction)))
            {
                if (other != action && GetKeyBinding(other) == key)
                    AssignKeyBinding(other, previous);
            }
            AssignKeyBinding(action, key);
        }

        private KeyCode GetKeyBinding(KeyBindingAction action) =>
            action == KeyBindingAction.ToggleHud
                ? GlobalSettings.ToggleHudKey
                : action == KeyBindingAction.PreviousStep
                    ? GlobalSettings.PreviousStepKey
                    : GlobalSettings.NextStepKey;

        private void AssignKeyBinding(KeyBindingAction action, KeyCode key)
        {
            if (action == KeyBindingAction.ToggleHud) GlobalSettings.ToggleHudKey = key;
            else if (action == KeyBindingAction.PreviousStep) GlobalSettings.PreviousStepKey = key;
            else GlobalSettings.NextStepKey = key;
        }

        private static KeyCode NormalizeKey(KeyCode key, KeyCode fallback) =>
            KeyIndex(key) >= 0 ? key : fallback;

        private static int KeyIndex(KeyCode key)
        {
            for (int index = 0; index < SupportedKeys.Length; index++)
                if (SupportedKeys[index] == key) return index;
            return -1;
        }
    }
}


