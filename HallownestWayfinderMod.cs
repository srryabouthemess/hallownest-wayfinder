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

        public RouteProgress Progress { get; private set; } = new RouteProgress();
        public RouteGlobalSettings GlobalSettings { get; private set; } = new RouteGlobalSettings();
        public bool ToggleButtonInsideMenu => true;
        public RoutePlan CurrentRoute => RouteCatalog.Routes[GlobalSettings.ActiveRoute];
        public bool IsSaveCompletion => CurrentRoute.IsSaveCompletion;
        public int CurrentStepIndex
        {
            get => IsSaveCompletion
                ? SaveCompletionAnalyzer.FindNextStep(CurrentRoute.Steps,
                    Progress.SaveCompletionDismissedStepIds)
                : CurrentRoute.Id == "speedrun_5h"
                    ? Progress.SpeedrunCurrentStep
                    : CurrentRoute.Id == "grubs_46" ? Progress.GrubCurrentStep : Progress.CurrentStep;
            private set
            {
                if (IsSaveCompletion) return;
                if (CurrentRoute.Id == "speedrun_5h") Progress.SpeedrunCurrentStep = value;
                else if (CurrentRoute.Id == "grubs_46") Progress.GrubCurrentStep = value;
                else Progress.CurrentStep = value;
            }
        }
        public bool HasActiveStep => CurrentStepIndex >= 0 && CurrentStepIndex < CurrentRoute.Steps.Count;
        public bool IsRouteComplete => CompletedStepCount >= CurrentRoute.Steps.Count;
        public RouteStep? CurrentStep => HasActiveStep ? CurrentRoute.Steps[CurrentStepIndex] : null;
        public int CompletedStepCount => IsSaveCompletion
            ? SaveCompletionAnalyzer.CountCompleted(CurrentRoute.Steps)
            : CurrentStepIndex;
        public bool CurrentStepIsAvailable => !IsSaveCompletion ||
            (CurrentStep != null && SaveCompletionAnalyzer.IsAvailable(CurrentStep));

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
                LogError("Dados de rota inválidos: " + error);
            foreach (string error in VanillaRouteGraph.ValidateRequirements())
                LogError("Grafo de navegação inválido: " + error);

            _hudObject = new GameObject("HallownestWayfinder HUD");
            UObject.DontDestroyOnLoad(_hudObject);
            RouteHud hud = _hudObject.AddComponent<RouteHud>();
            hud.Mod = this;
            Log("HallownestWayfinder carregado. Controles configuráveis disponíveis no menu do mod.");
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

            // Versão 2 removeu Myla, que ocupava o antigo índice 7.
            if (Progress.DataVersion < 2)
            {
                if (Progress.CurrentStep > 7) Progress.CurrentStep--;
                Progress.DataVersion = 2;
            }

            if (Progress.DataVersion < 3) Progress.DataVersion = 3;
            if (Progress.SaveCompletionDismissedStepIds == null)
                Progress.SaveCompletionDismissedStepIds = new List<string>();
            if (Progress.DataVersion < 4) Progress.DataVersion = 4;

            ClampProgress();
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
                    Name = LocalizationService.Text("route", "Rota ativa"),
                    Description = LocalizationService.Text("route_description", "Escolha a rota exibida pelo Hallownest Wayfinder."),
                    Values = new[]
                    {
                        "112%",
                        "Speedrun 5h",
                        LocalizationService.Text("grubs_route", "Larvas 46/46"),
                        LocalizationService.Text("save_completion_route", "Completar save")
                    },
                    Saver = value =>
                    {
                        GlobalSettings.ActiveRoute = value;
                        ClampProgress();
                    },
                    Loader = () => GlobalSettings.ActiveRoute
                },
                new IMenuMod.MenuEntry
                {
                    Name = LocalizationService.Text("language", "Idioma"),
                    Description = LocalizationService.Text("language_description", "Escolha o idioma usado pelo mod."),
                    Values = new[]
                    {
                        LocalizationService.Text("automatic", "Automático"),
                        LocalizationService.Text("portuguese", "Português (Brasil)"),
                        LocalizationService.Text("english", "English")
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
                    Name = LocalizationService.Text("ui_size", "Tamanho da interface"),
                    Description = LocalizationService.Text("ui_size_description", "Altera o tamanho do painel de objetivo."),
                    Values = new[]
                    {
                        LocalizationService.Text("small", "Pequeno"),
                        LocalizationService.Text("medium", "Médio"),
                        LocalizationService.Text("large", "Grande")
                    },
                    Saver = value => GlobalSettings.UiSize = value,
                    Loader = () => GlobalSettings.UiSize
                },
                new IMenuMod.MenuEntry
                {
                    Name = LocalizationService.Text("navigation_arrow", "Seta de navegação"),
                    Description = LocalizationService.Text("navigation_description", "Inteligente usa rotas mapeadas; Geral mostra uma direção aproximada."),
                    Values = new[]
                    {
                        LocalizationService.Text("smart", "Inteligente"),
                        LocalizationService.Text("general", "Geral"),
                        LocalizationService.Text("off", "Desligada")
                    },
                    Saver = value => GlobalSettings.NavigationMode = value,
                    Loader = () => GlobalSettings.NavigationMode
                }
            };

            entries.Add(CreateKeyEntry(
                LocalizationService.Text("toggle_key", "Tecla: mostrar/ocultar"),
                LocalizationService.Text("toggle_key_description", "Escolha a tecla que mostra ou oculta o HUD."),
                KeyBindingAction.ToggleHud));
            entries.Add(CreateKeyEntry(
                LocalizationService.Text("previous_key", "Tecla: voltar etapa"),
                LocalizationService.Text("previous_key_description", "Escolha a tecla que retorna à etapa anterior."),
                KeyBindingAction.PreviousStep));
            entries.Add(CreateKeyEntry(
                LocalizationService.Text("next_key", "Tecla: avançar etapa"),
                LocalizationService.Text("next_key_description", "Escolha a tecla que avança ou adia a etapa."),
                KeyBindingAction.NextStep));
            entries.Add(new IMenuMod.MenuEntry
            {
                Name = LocalizationService.Text("reset_route", "Resetar progresso da rota"),
                Description = LocalizationService.Text("reset_route_description", "Reinicia somente o progresso manual da rota atualmente selecionada."),
                Values = new[]
                {
                    LocalizationService.Text("keep_progress", "Manter"),
                    LocalizationService.Text("reset_now", "Resetar agora")
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
            if (CurrentRoute.Id == "speedrun_5h") Progress.SpeedrunCurrentStep = 0;
            else if (CurrentRoute.Id == "grubs_46") Progress.GrubCurrentStep = 0;
            else if (IsSaveCompletion) Progress.SaveCompletionDismissedStepIds.Clear();
            else Progress.CurrentStep = 0;
            _automaticAdvanceErrors.Clear();
        }

        public void NextStep()
        {
            if (IsSaveCompletion)
            {
                RouteStep? step = CurrentStep;
                if (step != null && !Progress.SaveCompletionDismissedStepIds.Contains(step.Id))
                    Progress.SaveCompletionDismissedStepIds.Add(step.Id);
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
                return;
            }

            if (CurrentStepIndex > 0)
                CurrentStepIndex--;
        }

        public void TryAdvanceAutomatically()
        {
            if (!HasActiveStep || GameManager.instance == null || PlayerData.instance == null) return;

            RoutePlan route = CurrentRoute;
            RouteStep? step = CurrentStep;
            if (step == null) return;
            try
            {
                if (step.IsComplete() && !IsSaveCompletion) NextStep();
            }
            catch (Exception exception)
            {
                string errorKey = route.Id + "\n" + step.Id;
                if (_automaticAdvanceErrors.Add(errorKey))
                {
                    LogError("Falha ao avaliar a etapa '" + step.Id +
                        "' da rota '" + route.Id + "': " + exception);
                }
            }
        }

        private void ClampProgress()
        {
            Progress.CurrentStep = Math.Max(0, Math.Min(Progress.CurrentStep, RouteDefinition.Steps.Count));
            Progress.SpeedrunCurrentStep = Math.Max(0,
                Math.Min(Progress.SpeedrunCurrentStep, SpeedrunRouteDefinition.Steps.Count));
            Progress.GrubCurrentStep = Math.Max(0,
                Math.Min(Progress.GrubCurrentStep, GrubRouteDefinition.Steps.Count));
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

