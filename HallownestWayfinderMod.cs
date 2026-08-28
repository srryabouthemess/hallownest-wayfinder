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
        private static HallownestWayfinderMod _instance;
        private GameObject _hudObject;

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
        public RouteStep CurrentStep => HasActiveStep ? CurrentRoute.Steps[CurrentStepIndex] : null;
        public int CompletedStepCount => IsSaveCompletion
            ? SaveCompletionAnalyzer.CountCompleted(CurrentRoute.Steps)
            : CurrentStepIndex;
        public bool CurrentStepIsAvailable => !IsSaveCompletion ||
            (CurrentStep != null && SaveCompletionAnalyzer.IsAvailable(CurrentStep));

        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public override void Initialize()
        {
            if (_instance != null) return;
            _instance = this;

            _hudObject = new GameObject("HallownestWayfinder HUD");
            UObject.DontDestroyOnLoad(_hudObject);
            RouteHud hud = _hudObject.AddComponent<RouteHud>();
            hud.Mod = this;
            Log("HallownestWayfinder carregado. F6: mostrar/ocultar, F7: voltar, F8: avançar.");
        }

        public void Unload()
        {
            if (_hudObject != null) UObject.Destroy(_hudObject);
            _hudObject = null;
            _instance = null;
        }

        public void OnLoadLocal(RouteProgress settings)
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

        public void OnLoadGlobal(RouteGlobalSettings settings)
        {
            GlobalSettings = settings ?? new RouteGlobalSettings();
            GlobalSettings.UiSize = Math.Max(0, Math.Min(GlobalSettings.UiSize, 2));
            GlobalSettings.NavigationMode = Math.Max(0, Math.Min(GlobalSettings.NavigationMode, 2));
            GlobalSettings.ActiveRoute = Math.Max(0, Math.Min(GlobalSettings.ActiveRoute, RouteCatalog.Routes.Count - 1));
            GlobalSettings.Language = Math.Max(0, Math.Min(GlobalSettings.Language, 2));
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

            if (toggleButtonEntry.HasValue) entries.Insert(0, toggleButtonEntry.Value);
            return entries;
        }

        public void ToggleVisibility() => Progress.Visible = !Progress.Visible;

        public void NextStep()
        {
            if (IsSaveCompletion)
            {
                RouteStep step = CurrentStep;
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

            try
            {
                if (CurrentStep.IsComplete() && !IsSaveCompletion) NextStep();
            }
            catch (Exception exception)
            {
                LogError("Falha ao avaliar a etapa atual: " + exception);
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
    }
}

