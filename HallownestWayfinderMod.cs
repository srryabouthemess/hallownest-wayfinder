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
        public bool HasActiveStep => Progress.CurrentStep >= 0 && Progress.CurrentStep < RouteDefinition.Steps.Count;
        public RouteStep CurrentStep => HasActiveStep ? RouteDefinition.Steps[Progress.CurrentStep] : null;

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

            ClampProgress();
        }

        public RouteProgress OnSaveLocal() => Progress;

        public void OnLoadGlobal(RouteGlobalSettings settings)
        {
            GlobalSettings = settings ?? new RouteGlobalSettings();
            GlobalSettings.UiSize = Math.Max(0, Math.Min(GlobalSettings.UiSize, 2));
            GlobalSettings.NavigationMode = Math.Max(0, Math.Min(GlobalSettings.NavigationMode, 2));
        }

        public RouteGlobalSettings OnSaveGlobal() => GlobalSettings;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            List<IMenuMod.MenuEntry> entries = new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry
                {
                    Name = "Tamanho da interface",
                    Description = "Altera o tamanho do painel de objetivo.",
                    Values = new[] { "Pequeno", "Médio", "Grande" },
                    Saver = value => GlobalSettings.UiSize = value,
                    Loader = () => GlobalSettings.UiSize
                },
                new IMenuMod.MenuEntry
                {
                    Name = "Seta de navegação",
                    Description = "Inteligente usa rotas mapeadas; Geral mostra uma direção aproximada.",
                    Values = new[] { "Inteligente", "Geral", "Desligada" },
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
            if (Progress.CurrentStep < RouteDefinition.Steps.Count)
                Progress.CurrentStep++;
        }

        public void PreviousStep()
        {
            if (Progress.CurrentStep > 0)
                Progress.CurrentStep--;
        }

        public void TryAdvanceAutomatically()
        {
            if (!HasActiveStep || GameManager.instance == null || PlayerData.instance == null) return;

            try
            {
                if (CurrentStep.IsComplete()) NextStep();
            }
            catch (Exception exception)
            {
                LogError("Falha ao avaliar a etapa atual: " + exception);
            }
        }

        private void ClampProgress()
        {
            Progress.CurrentStep = Math.Max(0, Math.Min(Progress.CurrentStep, RouteDefinition.Steps.Count));
        }
    }
}

