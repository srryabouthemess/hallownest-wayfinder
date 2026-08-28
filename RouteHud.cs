using GlobalEnums;
using UnityEngine;

namespace HallownestWayfinder
{
    public sealed class RouteHud : MonoBehaviour
    {
        private GUIStyle _titleStyle = null!;
        private GUIStyle _bodyStyle = null!;
        private GUIStyle _footerStyle = null!;
        private Texture2D _background = null!;
        private Texture2D _medallion = null!;
        private Texture2D _arrow = null!;
        private int _lastStep = -1;
        private string? _lastRouteId;
        private float _transitionStarted;
        private NavigationResult? _navigation;

        public HallownestWayfinderMod? Mod { get; set; }

        private void Update()
        {
            HallownestWayfinderMod? mod = Mod;
            if (mod == null) return;

            if (Input.GetKeyDown(mod.GlobalSettings.ToggleHudKey)) mod.ToggleVisibility();
            if (Input.GetKeyDown(mod.GlobalSettings.PreviousStepKey)) mod.PreviousStep();
            if (Input.GetKeyDown(mod.GlobalSettings.NextStepKey)) mod.NextStep();

            if (IsInsideSave()) mod.TryAdvanceAutomatically();

            if (mod.CurrentStepIndex != _lastStep || mod.CurrentRoute.Id != _lastRouteId)
            {
                if (_lastStep >= 0) _transitionStarted = Time.unscaledTime;
                _lastStep = mod.CurrentStepIndex;
                _lastRouteId = mod.CurrentRoute.Id;
            }

            bool canResolveNavigation = mod.Progress.Visible && mod.HasActiveStep &&
                IsInsideSave() && mod.GlobalSettings.NavigationMode != 2;
            _navigation = canResolveNavigation
                ? RouteNavigation.Resolve(mod.CurrentStep,
                    mod.GlobalSettings.NavigationMode == 0)
                : null;
        }

        private void OnGUI()
        {
            HallownestWayfinderMod? mod = Mod;
            if (mod == null || !mod.Progress.Visible || !IsInsideSave()) return;
            EnsureStyles();
            if (!mod.HasActiveStep)
            {
                if (mod.IsRouteComplete) DrawCompleted(mod);
                return;
            }
            RouteStep? step = mod.CurrentStep;
            if (step == null) return;

            float scale = mod.GlobalSettings.UiSize == 0 ? 0.78f : mod.GlobalSettings.UiSize == 2 ? 1.22f : 1f;
            _titleStyle.fontSize = Mathf.RoundToInt(14f * scale);
            _bodyStyle.fontSize = Mathf.RoundToInt(16f * scale);
            _footerStyle.fontSize = Mathf.RoundToInt(12f * scale);

            float width = Mathf.Clamp(Screen.width * 0.30f * scale, 300f, 680f);
            width = Mathf.Min(width, Screen.width - 32f);
            float padding = 34f * scale;
            float contentWidth = width - padding * 2f;
            NavigationResult? navigation = _navigation;
            bool navigationVisible = mod.GlobalSettings.NavigationMode != 2 && navigation != null;
            string heading = mod.IsSaveCompletion
                ? $"{LocalizationService.RouteName(mod.CurrentRoute).ToUpperInvariant()}  •  {mod.CompletedStepCount}/{mod.CurrentRoute.Steps.Count}"
                : $"{LocalizationService.RouteName(mod.CurrentRoute).ToUpperInvariant()}  •  {mod.CurrentStepIndex + 1}/{mod.CurrentRoute.Steps.Count}";
            string objective = (step.SkippableInRoute ? LocalizationService.Text("optional", "[OPCIONAL] ") : "") + LocalizationService.StepTitle(step);
            string hint = "→ " + LocalizationService.StepHint(step);
            string nextAction = Action("complete", "{0} concluir", mod.GlobalSettings.NextStepKey);
            string skipAction = Action("skip", "{0} pular", mod.GlobalSettings.NextStepKey);
            string laterAction = Action("later", "{0} ver depois", mod.GlobalSettings.NextStepKey);
            string backAction = Action("back", "{0} voltar", mod.GlobalSettings.PreviousStepKey);
            string hideAction = Action("hide", "{0} ocultar", mod.GlobalSettings.ToggleHudKey);
            string footer = mod.IsSaveCompletion
                ? (mod.CurrentStepIsAvailable
                    ? LocalizationService.Text("save_analyzed", "Save analisado")
                    : LocalizationService.Text("prerequisites_missing", "Pré-requisitos não detectados"))
                    + "  •  " + laterAction
                    + "  •  " + backAction
                    + "  •  " + hideAction
                : step.IsAutomaticallyTracked
                ? step.SkippableInRoute
                    ? LocalizationService.Text("automatic_progress", "Avanço automático") + "  •  " + skipAction + "  •  " + backAction + "  •  " + hideAction
                    : LocalizationService.Text("automatic_progress", "Avanço automático") + "  •  " + backAction + "  •  " + hideAction
                : nextAction + "  •  " + backAction + "  •  " + hideAction;
            string? checklist = ShouldShowChecklist(mod) ? CompletionChecklist.Format() : null;

            Texture2D? icon = IconLoader.Get(step.Icon);
            float iconSize = icon == null ? 0f : 78f * scale;
            float iconGap = icon == null ? 0f : 12f * scale;
            float bodyWidth = contentWidth - iconSize - iconGap;
            float headingHeight = _titleStyle.CalcHeight(new GUIContent(heading), contentWidth);
            float objectiveHeight = _bodyStyle.CalcHeight(new GUIContent(objective), bodyWidth);
            float hintHeight = _bodyStyle.CalcHeight(new GUIContent(hint), bodyWidth);
            float footerHeight = _footerStyle.CalcHeight(new GUIContent(footer), contentWidth);
            float bodyHeight = Mathf.Max(iconSize, objectiveHeight + 8f * scale + hintHeight);
            float navigationHeight = navigation != null && navigationVisible
                ? _footerStyle.CalcHeight(new GUIContent(navigation.Label), contentWidth)
                : 0f;
            float checklistHeight = checklist == null
                ? 0f
                : _footerStyle.CalcHeight(new GUIContent(checklist), contentWidth);
            float panelHeight = 25f * scale + headingHeight + 9f * scale + bodyHeight + 13f * scale +
                navigationHeight + (navigationHeight > 0f ? 5f * scale : 0f) +
                checklistHeight + (checklistHeight > 0f ? 7f * scale : 0f) + footerHeight + 25f * scale;

            float elapsed = Time.unscaledTime - _transitionStarted;
            float reveal = _transitionStarted <= 0f ? 1f : Mathf.Clamp01(elapsed / 0.28f);
            reveal = reveal * reveal * (3f - 2f * reveal);
            float slide = (1f - reveal) * 24f * scale;
            Rect panel = new Rect(Screen.width - width - 16f + slide, 18f, width, panelHeight);

            Color previousColor = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, reveal);
            GUI.DrawTexture(panel, _background);

            if (navigation != null && navigationVisible && navigation.ShowArrow)
            {
                float arrowSize = 48f * scale;
                Rect arrowRect = new Rect(panel.x - arrowSize - 12f * scale,
                    panel.y + (panel.height - arrowSize) * 0.5f, arrowSize, arrowSize);
                DrawRotatedTexture(arrowRect, _arrow, navigation.Degrees);
            }

            float y = panel.y + 25f * scale;
            GUI.Label(new Rect(panel.x + padding, y, contentWidth, headingHeight), heading, _titleStyle);
            y += headingHeight + 9f * scale;
            if (icon != null)
            {
                GUI.DrawTexture(new Rect(panel.x + padding, y, iconSize, iconSize), _medallion, ScaleMode.ScaleToFit, true);
                float inset = 10f * scale;
                GUI.DrawTexture(new Rect(panel.x + padding + inset, y + inset, iconSize - inset * 2f, iconSize - inset * 2f), icon, ScaleMode.ScaleToFit, true);
            }
            float textX = panel.x + padding + iconSize + iconGap;
            GUI.Label(new Rect(textX, y, bodyWidth, objectiveHeight), objective, _bodyStyle);
            GUI.Label(new Rect(textX, y + objectiveHeight + 8f * scale, bodyWidth, hintHeight), hint, _bodyStyle);
            y += bodyHeight + 11f * scale;
            if (navigation != null && navigationVisible)
            {
                string navigationText = navigation.Kind == NavigationKind.Precise
                    ? "◇ " + LocalizationService.Text("navigation", "Navegação: ") + navigation.Label
                    : navigation.Kind == NavigationKind.Transport
                        ? "◇ " + navigation.Label
                        : navigation.Kind == NavigationKind.Arrived
                            ? "◇ " + navigation.Label
                            : navigation.Kind == NavigationKind.Unmapped
                                ? "◇ " + LocalizationService.Text("smart_unmapped", "Navegação inteligente: trecho ainda não mapeado")
                                : "◇ " + LocalizationService.Text("arrow_approximate", "Seta: direção geral (aproximada)");
                GUI.Label(new Rect(panel.x + padding, y, contentWidth, navigationHeight), navigationText, _footerStyle);
                y += navigationHeight + 5f * scale;
            }
            if (checklist != null)
            {
                GUI.Label(new Rect(panel.x + padding, y, contentWidth, checklistHeight), checklist, _footerStyle);
                y += checklistHeight + 7f * scale;
            }
            GUI.Label(new Rect(panel.x + padding, y, contentWidth, footerHeight), footer, _footerStyle);

            if (_transitionStarted > 0f && elapsed < 0.65f)
            {
                float glow = Mathf.Sin(elapsed / 0.65f * Mathf.PI) * 0.18f;
                GUI.color = new Color(0.65f, 0.88f, 1f, glow);
                GUI.DrawTexture(new Rect(panel.x + 10f, panel.y + 9f, panel.width - 20f, panel.height - 18f), Texture2D.whiteTexture);
            }
            GUI.color = previousColor;
        }

        private void DrawCompleted(HallownestWayfinderMod mod)
        {
            float scale = mod.GlobalSettings.UiSize == 0 ? 0.78f : mod.GlobalSettings.UiSize == 2 ? 1.22f : 1f;
            _titleStyle.fontSize = Mathf.RoundToInt(14f * scale);
            _bodyStyle.fontSize = Mathf.RoundToInt(18f * scale);
            _footerStyle.fontSize = Mathf.RoundToInt(12f * scale);

            float width = Mathf.Clamp(Screen.width * 0.30f * scale, 300f, 680f);
            width = Mathf.Min(width, Screen.width - 32f);
            float padding = 34f * scale;
            float contentWidth = width - padding * 2f;
            string heading = LocalizationService.RouteName(mod.CurrentRoute).ToUpperInvariant() +
                "  •  " + mod.CurrentRoute.Steps.Count + "/" + mod.CurrentRoute.Steps.Count;
            string completed = LocalizationService.Text("route_completed", "ROTA CONCLUÍDA");
            string? checklist = ShouldShowChecklist(mod) ? CompletionChecklist.Format() : null;
            string footer = Action("hide", "{0} ocultar", mod.GlobalSettings.ToggleHudKey);

            float headingHeight = _titleStyle.CalcHeight(new GUIContent(heading), contentWidth);
            float completedHeight = _bodyStyle.CalcHeight(new GUIContent(completed), contentWidth);
            float checklistHeight = checklist == null ? 0f : _footerStyle.CalcHeight(new GUIContent(checklist), contentWidth);
            float footerHeight = _footerStyle.CalcHeight(new GUIContent(footer), contentWidth);
            float panelHeight = 25f * scale + headingHeight + 14f * scale + completedHeight + 12f * scale +
                (checklistHeight > 0f ? checklistHeight + 12f * scale : 0f) + footerHeight + 25f * scale;
            Rect panel = new Rect(Screen.width - width - 16f, 18f, width, panelHeight);

            GUI.DrawTexture(panel, _background);
            float y = panel.y + 25f * scale;
            GUI.Label(new Rect(panel.x + padding, y, contentWidth, headingHeight), heading, _titleStyle);
            y += headingHeight + 14f * scale;
            GUI.Label(new Rect(panel.x + padding, y, contentWidth, completedHeight), completed, _bodyStyle);
            y += completedHeight + 12f * scale;
            if (checklist != null)
            {
                GUI.Label(new Rect(panel.x + padding, y, contentWidth, checklistHeight), checklist, _footerStyle);
                y += checklistHeight + 12f * scale;
            }
            GUI.Label(new Rect(panel.x + padding, y, contentWidth, footerHeight), footer, _footerStyle);
        }

        private static bool ShouldShowChecklist(HallownestWayfinderMod mod) =>
            mod.CurrentRoute.Id == "completion_112" || mod.IsSaveCompletion;

        private static string Action(string key, string portuguese, KeyCode binding) =>
            string.Format(LocalizationService.Text(key, portuguese),
                HallownestWayfinderMod.KeyName(binding));

        private static void DrawRotatedTexture(Rect destination, Texture2D texture, float degrees)
        {
            if (texture == null) return;
            Matrix4x4 previousMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(degrees, destination.center);
            GUI.DrawTexture(destination, texture, ScaleMode.ScaleToFit, true);
            GUI.matrix = previousMatrix;
        }

        private static bool IsInsideSave()
        {
            if (GameManager.instance == null || HeroController.instance == null) return false;

            GameState state = GameManager.instance.gameState;
            return state != GameState.MAIN_MENU && state != GameState.INACTIVE;
        }

        private void EnsureStyles()
        {
            if (_background != null) return;

            _background = new Texture2D(2, 2);
            _background.SetPixel(0, 0, new Color(0.015f, 0.025f, 0.05f, 0.94f));
            _background.SetPixel(1, 0, new Color(0.015f, 0.025f, 0.05f, 0.94f));
            _background.SetPixel(0, 1, new Color(0.055f, 0.09f, 0.14f, 0.91f));
            _background.SetPixel(1, 1, new Color(0.055f, 0.09f, 0.14f, 0.91f));
            _background.Apply();
            _medallion = CreateMedallion(96);
            _arrow = CreateArrow(96);

            Font? gameFont = FindGameFont();

            _titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                wordWrap = true,
                normal = { textColor = new Color(0.55f, 0.82f, 0.95f) }
            };
            _bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
            _footerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                wordWrap = true,
                normal = { textColor = new Color(0.68f, 0.7f, 0.74f) }
            };
            if (gameFont != null)
            {
                _titleStyle.font = gameFont;
                _bodyStyle.font = gameFont;
                _footerStyle.font = gameFont;
            }
        }

        private static Font? FindGameFont()
        {
            Font? fallback = null;
            foreach (Font font in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (font == null) continue;
                string name = font.name.ToLowerInvariant();
                if (name.Contains("perpetua") || name.Contains("trajan")) return font;
                if (fallback == null && (name.Contains("serif") || name.Contains("display"))) fallback = font;
            }
            return fallback;
        }

        private static Texture2D CreateMedallion(int size)
        {
            Texture2D texture = new Texture2D(size, size);
            texture.name = "HallownestWayfinder Medallion";
            float center = (size - 1) * 0.5f;
            float radius = size * 0.47f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Mathf.Sqrt((x - center) * (x - center) + (y - center) * (y - center));
                    Color color;
                    if (distance > radius) color = Color.clear;
                    else if (distance > radius - 3f) color = new Color(0.75f, 0.86f, 0.93f, 0.92f);
                    else if (distance > radius - 6f) color = new Color(0.22f, 0.35f, 0.48f, 0.92f);
                    else color = new Color(0.02f, 0.035f, 0.065f, 0.94f);
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateArrow(int size)
        {
            Texture2D texture = new Texture2D(size, size);
            texture.name = "HallownestWayfinder Arrow";
            float center = (size - 1) * 0.5f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float normalizedY = y / (float)(size - 1);
                    float halfWidth = normalizedY < 0.55f
                        ? 6f
                        : (1f - normalizedY) / 0.45f * center * 0.72f;
                    bool shaft = normalizedY < 0.64f && Mathf.Abs(x - center) <= 5f;
                    bool head = normalizedY >= 0.55f && Mathf.Abs(x - center) <= halfWidth;
                    float edge = Mathf.Abs(x - center);
                    Color color = shaft || head
                        ? new Color(0.72f, 0.9f, 1f, edge > halfWidth - 2f ? 0.7f : 0.95f)
                        : Color.clear;
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }

        private void OnDestroy()
        {
            if (_background != null) Destroy(_background);
            if (_medallion != null) Destroy(_medallion);
            if (_arrow != null) Destroy(_arrow);
            IconLoader.Unload();
        }
    }
}

