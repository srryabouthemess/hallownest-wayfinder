using GlobalEnums;
using System.Globalization;
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
        private HudLayout? _layout;

        private sealed class HudLayout
        {
            public string RouteId = string.Empty;
            public string StepId = string.Empty;
            public int StepIndex;
            public int CompletedSteps;
            public int TotalSteps;
            public int ScreenWidth;
            public int UiSize;
            public bool English;
            public bool Available;
            public bool Completed;
            public int NavigationMode;
            public NavigationKind NavigationKind;
            public string NavigationLabel = string.Empty;
            public KeyCode ToggleKey;
            public KeyCode PreviousKey;
            public KeyCode NextKey;
            public IGameState? State;
            public float Scale;
            public float Width;
            public float Padding;
            public float ContentWidth;
            public float IconSize;
            public float IconGap;
            public float BodyWidth;
            public float HeadingHeight;
            public float ObjectiveHeight;
            public float HintHeight;
            public float BodyHeight;
            public float NavigationHeight;
            public float ChecklistHeight;
            public float FooterHeight;
            public float PanelHeight;
            public string Heading = string.Empty;
            public string Objective = string.Empty;
            public string Hint = string.Empty;
            public string NavigationText = string.Empty;
            public string? Checklist;
            public string Footer = string.Empty;
            public Texture2D? Icon;
        }

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
                    mod.GlobalSettings.NavigationMode == 0, mod.CurrentGameState)
                : null;
        }

        private void OnGUI()
        {
            HallownestWayfinderMod? mod = Mod;
            if (mod == null || !mod.Progress.Visible || !IsInsideSave()) return;
            if (Event.current.type != EventType.Repaint) return;
            EnsureStyles();
            if (!mod.HasActiveStep)
            {
                if (mod.IsRouteComplete) DrawCompleted(GetLayout(mod, null));
                return;
            }
            RouteStep? step = mod.CurrentStep;
            if (step == null) return;
            HudLayout layout = GetLayout(mod, step);
            NavigationResult navigation = _navigation.GetValueOrDefault();

            float elapsed = Time.unscaledTime - _transitionStarted;
            float reveal = _transitionStarted <= 0f ? 1f : Mathf.Clamp01(elapsed / 0.28f);
            reveal = reveal * reveal * (3f - 2f * reveal);
            float slide = (1f - reveal) * 24f * layout.Scale;
            Rect panel = new Rect(Screen.width - layout.Width - 16f + slide, 18f,
                layout.Width, layout.PanelHeight);

            Color previousColor = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, reveal);
            GUI.DrawTexture(panel, _background);

            if (layout.NavigationHeight > 0f && navigation.ShowArrow)
            {
                float arrowSize = 48f * layout.Scale;
                Rect arrowRect = new Rect(panel.x - arrowSize - 12f * layout.Scale,
                    panel.y + (panel.height - arrowSize) * 0.5f, arrowSize, arrowSize);
                DrawRotatedTexture(arrowRect, _arrow, navigation.Degrees);
            }

            float y = panel.y + 25f * layout.Scale;
            GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                layout.HeadingHeight), layout.Heading, _titleStyle);
            y += layout.HeadingHeight + 9f * layout.Scale;
            if (layout.Icon != null)
            {
                GUI.DrawTexture(new Rect(panel.x + layout.Padding, y, layout.IconSize,
                    layout.IconSize), _medallion, ScaleMode.ScaleToFit, true);
                float inset = 10f * layout.Scale;
                GUI.DrawTexture(new Rect(panel.x + layout.Padding + inset, y + inset,
                    layout.IconSize - inset * 2f, layout.IconSize - inset * 2f),
                    layout.Icon, ScaleMode.ScaleToFit, true);
            }
            float textX = panel.x + layout.Padding + layout.IconSize + layout.IconGap;
            GUI.Label(new Rect(textX, y, layout.BodyWidth, layout.ObjectiveHeight),
                layout.Objective, _bodyStyle);
            GUI.Label(new Rect(textX, y + layout.ObjectiveHeight + 8f * layout.Scale,
                layout.BodyWidth, layout.HintHeight), layout.Hint, _bodyStyle);
            y += layout.BodyHeight + 11f * layout.Scale;
            if (layout.NavigationHeight > 0f)
            {
                GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                    layout.NavigationHeight), layout.NavigationText, _footerStyle);
                y += layout.NavigationHeight + 5f * layout.Scale;
            }
            if (layout.Checklist != null)
            {
                GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                    layout.ChecklistHeight), layout.Checklist, _footerStyle);
                y += layout.ChecklistHeight + 7f * layout.Scale;
            }
            GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                layout.FooterHeight), layout.Footer, _footerStyle);

            if (_transitionStarted > 0f && elapsed < 0.65f)
            {
                float glow = Mathf.Sin(elapsed / 0.65f * Mathf.PI) * 0.18f;
                GUI.color = new Color(0.65f, 0.88f, 1f, glow);
                GUI.DrawTexture(new Rect(panel.x + 10f, panel.y + 9f, panel.width - 20f, panel.height - 18f), Texture2D.whiteTexture);
            }
            GUI.color = previousColor;
        }

        private void DrawCompleted(HudLayout layout)
        {
            Rect panel = new Rect(Screen.width - layout.Width - 16f, 18f,
                layout.Width, layout.PanelHeight);

            GUI.DrawTexture(panel, _background);
            float y = panel.y + 25f * layout.Scale;
            GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                layout.HeadingHeight), layout.Heading, _titleStyle);
            y += layout.HeadingHeight + 14f * layout.Scale;
            GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                layout.ObjectiveHeight), layout.Objective, _bodyStyle);
            y += layout.ObjectiveHeight + 12f * layout.Scale;
            if (layout.Checklist != null)
            {
                GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                    layout.ChecklistHeight), layout.Checklist, _footerStyle);
                y += layout.ChecklistHeight + 12f * layout.Scale;
            }
            GUI.Label(new Rect(panel.x + layout.Padding, y, layout.ContentWidth,
                layout.FooterHeight), layout.Footer, _footerStyle);
        }

        private HudLayout GetLayout(HallownestWayfinderMod mod, RouteStep? step)
        {
            IGameState? state = mod.CurrentGameState;
            IGameState? layoutState = ShouldShowChecklist(mod) ? state : null;
            NavigationResult navigation = _navigation.GetValueOrDefault();
            bool completed = step == null;
            int currentStep = mod.CurrentStepIndex;
            int completedSteps = mod.CompletedStepCount;
            bool available = mod.CurrentStepIsAvailable;
            HudLayout? cached = _layout;
            if (cached != null && cached.RouteId == mod.CurrentRoute.Id &&
                cached.StepId == (step?.Id ?? string.Empty) && cached.StepIndex == currentStep &&
                cached.CompletedSteps == completedSteps && cached.ScreenWidth == Screen.width &&
                cached.UiSize == mod.GlobalSettings.UiSize && cached.English == LocalizationService.IsEnglish &&
                cached.Available == available && cached.Completed == completed &&
                cached.NavigationMode == mod.GlobalSettings.NavigationMode &&
                cached.NavigationKind == navigation.Kind && cached.NavigationLabel == navigation.Label &&
                cached.ToggleKey == mod.GlobalSettings.ToggleHudKey &&
                cached.PreviousKey == mod.GlobalSettings.PreviousStepKey &&
                cached.NextKey == mod.GlobalSettings.NextStepKey && ReferenceEquals(cached.State, layoutState))
                return cached;

            HudLayout layout = new HudLayout
            {
                RouteId = mod.CurrentRoute.Id,
                StepId = step?.Id ?? string.Empty,
                StepIndex = currentStep,
                CompletedSteps = completedSteps,
                TotalSteps = mod.CurrentRoute.Steps.Count,
                ScreenWidth = Screen.width,
                UiSize = mod.GlobalSettings.UiSize,
                English = LocalizationService.IsEnglish,
                Available = available,
                Completed = completed,
                NavigationMode = mod.GlobalSettings.NavigationMode,
                NavigationKind = navigation.Kind,
                NavigationLabel = navigation.Label,
                ToggleKey = mod.GlobalSettings.ToggleHudKey,
                PreviousKey = mod.GlobalSettings.PreviousStepKey,
                NextKey = mod.GlobalSettings.NextStepKey,
                State = layoutState
            };

            layout.Scale = layout.UiSize == 0 ? 0.78f : layout.UiSize == 2 ? 1.22f : 1f;
            _titleStyle.fontSize = Mathf.RoundToInt(14f * layout.Scale);
            _bodyStyle.fontSize = Mathf.RoundToInt((completed ? 18f : 16f) * layout.Scale);
            _footerStyle.fontSize = Mathf.RoundToInt(12f * layout.Scale);
            layout.Width = Mathf.Min(Mathf.Clamp(Screen.width * 0.30f * layout.Scale,
                300f, 680f), Screen.width - 32f);
            layout.Padding = 34f * layout.Scale;
            layout.ContentWidth = layout.Width - layout.Padding * 2f;
            layout.Heading = LocalizationService.RouteName(mod.CurrentRoute).ToUpperInvariant() +
                "  •  " + (mod.IsSaveCompletion ? completedSteps : completed ? layout.TotalSteps : currentStep + 1) +
                "/" + layout.TotalSteps;
            layout.Checklist = ShouldShowChecklist(mod) && state != null
                ? CompletionChecklist.Format(state)
                : null;
            layout.Footer = Action("hide", layout.ToggleKey);

            if (completed)
            {
                layout.Objective = LocalizationService.Text("route_completed");
                layout.HeadingHeight = _titleStyle.CalcHeight(new GUIContent(layout.Heading), layout.ContentWidth);
                layout.ObjectiveHeight = _bodyStyle.CalcHeight(new GUIContent(layout.Objective), layout.ContentWidth);
                layout.ChecklistHeight = layout.Checklist == null ? 0f :
                    _footerStyle.CalcHeight(new GUIContent(layout.Checklist), layout.ContentWidth);
                layout.FooterHeight = _footerStyle.CalcHeight(new GUIContent(layout.Footer), layout.ContentWidth);
                layout.PanelHeight = 25f * layout.Scale + layout.HeadingHeight + 14f * layout.Scale +
                    layout.ObjectiveHeight + 12f * layout.Scale +
                    (layout.ChecklistHeight > 0f ? layout.ChecklistHeight + 12f * layout.Scale : 0f) +
                    layout.FooterHeight + 25f * layout.Scale;
                _layout = layout;
                return layout;
            }

            layout.Objective = (step!.SkippableInRoute
                ? LocalizationService.Text("optional") + " " : string.Empty) +
                LocalizationService.StepTitle(step);
            layout.Hint = "→ " + LocalizationService.StepHint(step);
            string back = Action("back", layout.PreviousKey);
            string hide = layout.Footer;
            if (mod.IsSaveCompletion)
            {
                layout.Footer = (available
                    ? LocalizationService.Text("save_analyzed")
                    : LocalizationService.Text("prerequisites_missing")) +
                    "  •  " + Action("later", layout.NextKey) +
                    "  •  " + back + "  •  " + hide;
            }
            else if (step.IsAutomaticallyTracked)
            {
                layout.Footer = LocalizationService.Text("automatic_progress") +
                    (step.SkippableInRoute ? "  •  " + Action("skip", layout.NextKey) : string.Empty) +
                    "  •  " + back + "  •  " + hide;
            }
            else
            {
                layout.Footer = Action("complete", layout.NextKey) +
                    "  •  " + back + "  •  " + hide;
            }

            bool navigationVisible = layout.NavigationMode != 2 && _navigation.HasValue;
            if (navigationVisible)
            {
                layout.NavigationText = navigation.Kind == NavigationKind.Precise
                    ? "◇ " + LocalizationService.Text("navigation") + " " + navigation.Label
                    : navigation.Kind == NavigationKind.Transport || navigation.Kind == NavigationKind.Arrived
                        ? "◇ " + navigation.Label
                        : navigation.Kind == NavigationKind.Unmapped
                            ? "◇ " + LocalizationService.Text("smart_unmapped")
                            : "◇ " + LocalizationService.Text("arrow_approximate");
            }

            layout.Icon = IconLoader.Get(step.Icon);
            layout.IconSize = layout.Icon == null ? 0f : 78f * layout.Scale;
            layout.IconGap = layout.Icon == null ? 0f : 12f * layout.Scale;
            layout.BodyWidth = layout.ContentWidth - layout.IconSize - layout.IconGap;
            layout.HeadingHeight = _titleStyle.CalcHeight(new GUIContent(layout.Heading), layout.ContentWidth);
            layout.ObjectiveHeight = _bodyStyle.CalcHeight(new GUIContent(layout.Objective), layout.BodyWidth);
            layout.HintHeight = _bodyStyle.CalcHeight(new GUIContent(layout.Hint), layout.BodyWidth);
            layout.FooterHeight = _footerStyle.CalcHeight(new GUIContent(layout.Footer), layout.ContentWidth);
            layout.BodyHeight = Mathf.Max(layout.IconSize,
                layout.ObjectiveHeight + 8f * layout.Scale + layout.HintHeight);
            layout.NavigationHeight = navigationVisible
                ? _footerStyle.CalcHeight(new GUIContent(layout.NavigationText), layout.ContentWidth)
                : 0f;
            layout.ChecklistHeight = layout.Checklist == null ? 0f :
                _footerStyle.CalcHeight(new GUIContent(layout.Checklist), layout.ContentWidth);
            layout.PanelHeight = 25f * layout.Scale + layout.HeadingHeight + 9f * layout.Scale +
                layout.BodyHeight + 13f * layout.Scale + layout.NavigationHeight +
                (layout.NavigationHeight > 0f ? 5f * layout.Scale : 0f) + layout.ChecklistHeight +
                (layout.ChecklistHeight > 0f ? 7f * layout.Scale : 0f) +
                layout.FooterHeight + 25f * layout.Scale;
            _layout = layout;
            return layout;
        }

        private static bool ShouldShowChecklist(HallownestWayfinderMod mod) =>
            mod.CurrentRoute.Id == "completion_112" || mod.IsSaveCompletion;

        private static string Action(string key, KeyCode binding) =>
            string.Format(CultureInfo.InvariantCulture, LocalizationService.Text(key),
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


