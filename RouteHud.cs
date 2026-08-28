using GlobalEnums;
using UnityEngine;

namespace HallownestWayfinder
{
    public sealed class RouteHud : MonoBehaviour
    {
        private GUIStyle _titleStyle;
        private GUIStyle _bodyStyle;
        private GUIStyle _footerStyle;
        private Texture2D _background;
        private Texture2D _medallion;
        private Texture2D _arrow;
        private int _lastStep = -1;
        private string _lastRouteId;
        private float _transitionStarted;

        public HallownestWayfinderMod Mod { get; set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6)) Mod.ToggleVisibility();
            if (Input.GetKeyDown(KeyCode.F7)) Mod.PreviousStep();
            if (Input.GetKeyDown(KeyCode.F8)) Mod.NextStep();

            if (IsInsideSave()) Mod.TryAdvanceAutomatically();

            if (Mod != null &&
                (Mod.CurrentStepIndex != _lastStep || Mod.CurrentRoute.Id != _lastRouteId))
            {
                if (_lastStep >= 0) _transitionStarted = Time.unscaledTime;
                _lastStep = Mod.CurrentStepIndex;
                _lastRouteId = Mod.CurrentRoute.Id;
            }
        }

        private void OnGUI()
        {
            if (Mod == null || !Mod.Progress.Visible || !Mod.HasActiveStep || !IsInsideSave()) return;
            EnsureStyles();

            float scale = Mod.GlobalSettings.UiSize == 0 ? 0.78f : Mod.GlobalSettings.UiSize == 2 ? 1.22f : 1f;
            _titleStyle.fontSize = Mathf.RoundToInt(14f * scale);
            _bodyStyle.fontSize = Mathf.RoundToInt(16f * scale);
            _footerStyle.fontSize = Mathf.RoundToInt(12f * scale);

            float width = Mathf.Clamp(Screen.width * 0.30f * scale, 300f, 680f);
            width = Mathf.Min(width, Screen.width - 32f);
            float padding = 34f * scale;
            float contentWidth = width - padding * 2f;
            RouteStep step = Mod.CurrentStep;
            bool navigationVisible = Mod.GlobalSettings.NavigationMode != 2;
            NavigationResult navigation = RouteNavigation.Resolve(step, Mod.GlobalSettings.NavigationMode == 0);
            string heading = $"{Mod.CurrentRoute.Name.ToUpperInvariant()}  •  {Mod.CurrentStepIndex + 1}/{Mod.CurrentRoute.Steps.Count}";
            string objective = (step.Optional ? "[OPCIONAL] " : "") + step.Title;
            string hint = "→ " + step.Hint;
            string footer = step.IsAutomaticallyTracked
                ? step.Optional
                    ? "Avanço automático  •  F8 pular  •  F7 voltar  •  F6 ocultar"
                    : "Avanço automático  •  F7 voltar  •  F6 ocultar"
                : "F8 concluir  •  F7 voltar  •  F6 ocultar";

            Texture2D icon = IconLoader.Get(step.Icon);
            float iconSize = icon == null ? 0f : 78f * scale;
            float iconGap = icon == null ? 0f : 12f * scale;
            float bodyWidth = contentWidth - iconSize - iconGap;
            float headingHeight = _titleStyle.CalcHeight(new GUIContent(heading), contentWidth);
            float objectiveHeight = _bodyStyle.CalcHeight(new GUIContent(objective), bodyWidth);
            float hintHeight = _bodyStyle.CalcHeight(new GUIContent(hint), bodyWidth);
            float footerHeight = _footerStyle.CalcHeight(new GUIContent(footer), contentWidth);
            float bodyHeight = Mathf.Max(iconSize, objectiveHeight + 8f * scale + hintHeight);
            float navigationHeight = navigationVisible
                ? _footerStyle.CalcHeight(new GUIContent(navigation.Label), contentWidth)
                : 0f;
            float panelHeight = 25f * scale + headingHeight + 9f * scale + bodyHeight + 13f * scale +
                navigationHeight + (navigationHeight > 0f ? 5f * scale : 0f) + footerHeight + 25f * scale;

            float elapsed = Time.unscaledTime - _transitionStarted;
            float reveal = _transitionStarted <= 0f ? 1f : Mathf.Clamp01(elapsed / 0.28f);
            reveal = reveal * reveal * (3f - 2f * reveal);
            float slide = (1f - reveal) * 24f * scale;
            Rect panel = new Rect(Screen.width - width - 16f + slide, 18f, width, panelHeight);

            Color previousColor = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, reveal);
            GUI.DrawTexture(panel, _background);

            if (navigationVisible && navigation.ShowArrow)
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
            if (navigationVisible)
            {
                string navigationText = navigation.Kind == NavigationKind.Precise
                    ? "◇ Navegação: " + navigation.Label
                    : navigation.Kind == NavigationKind.Transport
                        ? "◇ " + navigation.Label
                        : navigation.Kind == NavigationKind.Arrived
                            ? "◇ " + navigation.Label
                            : navigation.Kind == NavigationKind.Unmapped
                                ? "◇ Navegação inteligente: trecho ainda não mapeado"
                                : "◇ Seta: direção geral (aproximada)";
                GUI.Label(new Rect(panel.x + padding, y, contentWidth, navigationHeight), navigationText, _footerStyle);
                y += navigationHeight + 5f * scale;
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

            Font gameFont = FindGameFont();

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

        private static Font FindGameFont()
        {
            Font fallback = null;
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

