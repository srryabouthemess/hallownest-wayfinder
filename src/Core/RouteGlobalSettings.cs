using UnityEngine;

namespace HallownestWayfinder
{
    public sealed class RouteGlobalSettings
    {
        // 0 = small, 1 = medium, 2 = large.
        public int UiSize { get; set; } = 1;
        // 0 = smart, 1 = general, 2 = off.
        public int NavigationMode { get; set; }
        // 0 = detailed, 1 = compact, 2 = off.
        public int ChecklistMode { get; set; }
        // Index of the selected route in RouteCatalog.Routes.
        public int ActiveRoute { get; set; }
        // 0 = automatic, 1 = Portuguese (Brazil), 2 = English.
        public int Language { get; set; }
        public bool WaypointRecorderEnabled { get; set; }
        public bool FreeNavigationEnabled { get; set; }
        public int FreeNavigationDestination { get; set; }
        public KeyCode ToggleHudKey { get; set; } = KeyCode.F6;
        public KeyCode PreviousStepKey { get; set; } = KeyCode.F7;
        public KeyCode NextStepKey { get; set; } = KeyCode.F8;
        public KeyCode RecordWaypointKey { get; set; } = KeyCode.F9;
        public KeyCode ToggleFreeNavigationKey { get; set; } = KeyCode.F10;
    }
}

