using UnityEngine;

namespace HallownestWayfinder
{
    public sealed class RouteGlobalSettings
    {
        // 0 = small, 1 = medium, 2 = large.
        public int UiSize { get; set; } = 1;
        // 0 = smart, 1 = general, 2 = off.
        public int NavigationMode { get; set; }
        // Index of the selected route in RouteCatalog.Routes.
        public int ActiveRoute { get; set; }
        // 0 = automatic, 1 = Portuguese (Brazil), 2 = English.
        public int Language { get; set; }
        public KeyCode ToggleHudKey { get; set; } = KeyCode.F6;
        public KeyCode PreviousStepKey { get; set; } = KeyCode.F7;
        public KeyCode NextStepKey { get; set; } = KeyCode.F8;
    }
}

