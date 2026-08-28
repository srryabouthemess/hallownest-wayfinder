using UnityEngine;

namespace HallownestWayfinder
{
    public sealed class RouteGlobalSettings
    {
        // 0 = pequeno, 1 = médio, 2 = grande.
        public int UiSize { get; set; } = 1;
        // 0 = inteligente, 1 = geral, 2 = desligada.
        public int NavigationMode { get; set; } = 0;
        // Índice da rota selecionada em RouteCatalog.Routes.
        public int ActiveRoute { get; set; }
        // 0 = automático, 1 = português (Brasil), 2 = inglês.
        public int Language { get; set; }
        public KeyCode ToggleHudKey { get; set; } = KeyCode.F6;
        public KeyCode PreviousStepKey { get; set; } = KeyCode.F7;
        public KeyCode NextStepKey { get; set; } = KeyCode.F8;
    }
}

