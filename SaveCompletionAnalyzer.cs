using System;
using System.Collections.Generic;

namespace HallownestWayfinder
{
    public static class SaveCompletionAnalyzer
    {
        public static int FindNextStep(IReadOnlyList<RouteStep> steps, IList<string> dismissed)
        {
            if (steps == null || PlayerData.instance == null || GameManager.instance == null) return -1;

            int firstIncomplete = -1;
            int firstDismissedIncomplete = -1;
            for (int index = 0; index < steps.Count; index++)
            {
                RouteStep step = steps[index];
                if (step == null || IsComplete(step)) continue;
                if (Contains(dismissed, step.Id))
                {
                    if (firstDismissedIncomplete < 0) firstDismissedIncomplete = index;
                    continue;
                }
                if (firstIncomplete < 0) firstIncomplete = index;
                if (IsAvailable(step)) return index;
            }

            // If every remaining item has an unknown prerequisite, still show
            // the earliest one and mark it as blocked instead of hiding the HUD.
            return firstIncomplete >= 0 ? firstIncomplete : firstDismissedIncomplete;
        }

        public static int CountCompleted(IReadOnlyList<RouteStep> steps)
        {
            if (steps == null || PlayerData.instance == null || GameManager.instance == null) return 0;

            int completed = 0;
            foreach (RouteStep step in steps)
                if (step != null && IsComplete(step)) completed++;
            return completed;
        }

        public static bool IsAvailable(RouteStep step)
        {
            if (step == null || PlayerData.instance == null) return false;

            switch (step.Id)
            {
                case "soul_catcher":
                    return Bool("hasSpell");
                case "c03_wastes":
                case "c03_ogres":
                case "c03_claw":
                    return Bool("hasDash");
                case "c04_city":
                    return Bool("hasWalljump");
                case "c04_soul_master":
                    return Bool("visitedRuins");
                case "c05_peak":
                    return Int("quakeLevel") >= 1 || Bool("hasLantern");
                case "c05_heart":
                    return Bool("visitedMines");
                case "c05_dark":
                    return Bool("hasSuperDash");
                case "c06_dream_nail":
                    return Bool("hasSuperDash") || Int("quakeLevel") >= 1;
                case "c07_shade_soul":
                    return Bool("hasWhiteKey") || Bool("usedWhiteKey");
                case "c08_isma":
                    return Bool("killedDungDefender") && Bool("hasSuperDash");
                case "c09_wings":
                    return Bool("killedInfectedKnight");
                case "c09_lost_kin":
                    return Bool("hasDreamNail") && Bool("killedInfectedKnight");
                case "c10_ritual":
                case "c11_failed":
                case "c11_xero":
                case "c13_noeyes":
                case "c18_hu":
                case "c19_marmu":
                case "c20_galien":
                case "c22_markoth":
                    return Bool("hasDreamNail");
                case "c16_abyss":
                case "c16_shriek":
                case "c16_cloak":
                    return Bool("hasKingsBrand");
                case "c17_grimm":
                case "c29_grimm_end":
                    return Bool("gotCharm_40");
                case "c17_pride":
                    return Bool("defeatedMantisLords");
                case "c18_uumuu":
                    return Bool("hasAcidArmour");
                case "c18_monomon":
                    return Bool("hasDreamNail") && Bool("killedMegaJellyfish");
                case "c19_traitor":
                    return Bool("hasShadowDash");
                case "c19_queen":
                    return Bool("killedTraitorLord");
                case "c20_herrah":
                    return Bool("hasDreamNail");
                case "c21_hive":
                    return Bool("hasTramPass");
                case "c21_hiveblood":
                    return Bool("killedHiveKnight");
                case "c23_collector":
                    return Bool("hasLoveKey") || Bool("openedLoveDoor");
                case "c23_lurien":
                    return Bool("hasDreamNail") && Bool("killedBlackKnight");
                case "c24_white_defender":
                    return Bool("hasDreamNail") && Bool("killedDungDefender");
                case "c25_grubfather":
                    return PlayerData.instance.grubsCollected >= 46;
                case "c27_palace":
                    return Bool("dreamNailUpgraded");
                case "c27_kingsoul":
                    return Bool("visitedWhitePalace");
                case "c28_void":
                    return Bool("gotCharm_36") || (Bool("gotQueenFragment") && Bool("gotKingFragment"));
                case "c29_pure_nail":
                    return Int("nailSmithUpgrades") >= 3;
                case "c30_pantheons":
                    return Bool("hasGodfinder");
                default:
                    return GrubIsAvailable(step);
            }
        }

        private static bool GrubIsAvailable(RouteStep step)
        {
            if (string.IsNullOrEmpty(step.RequiredGrubScene)) return true;
            string scene = step.RequiredGrubScene;

            if (scene.StartsWith("Mines_", StringComparison.Ordinal))
                return Bool("hasLantern") || Int("quakeLevel") >= 1;
            if (scene.StartsWith("Waterways_", StringComparison.Ordinal))
                return Bool("openedWaterwaysManhole");
            if (scene.StartsWith("Abyss_", StringComparison.Ordinal))
                return Bool("hasSuperDash") || Bool("hasDoubleJump");
            if (scene.StartsWith("Deepnest_", StringComparison.Ordinal))
                return Bool("hasLantern") || Bool("defeatedMantisLords");
            if (scene.StartsWith("Hive_", StringComparison.Ordinal))
                return Bool("hasTramPass");
            if (scene.StartsWith("Fungus3_", StringComparison.Ordinal))
                return Bool("hasAcidArmour") || Bool("hasShadowDash");
            if (scene.StartsWith("Ruins2_", StringComparison.Ordinal))
                return Bool("visitedRuins");

            return true;
        }

        private static bool IsComplete(RouteStep step)
        {
            try { return step.IsComplete(); }
            catch { return false; }
        }

        private static bool Contains(IList<string> values, string value)
        {
            if (values == null) return false;
            for (int i = 0; i < values.Count; i++)
                if (values[i] == value) return true;
            return false;
        }

        private static bool Bool(string field)
        {
            try { return PlayerData.instance.GetBool(field); }
            catch { return false; }
        }

        private static int Int(string field)
        {
            try { return PlayerData.instance.GetInt(field); }
            catch { return 0; }
        }
    }
}
