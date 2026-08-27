namespace HallownestWayfinder
{
    public sealed class RouteGlobalSettings
    {
        // 0 = pequeno, 1 = médio, 2 = grande.
        public int UiSize { get; set; } = 1;
        // 0 = inteligente, 1 = geral, 2 = desligada.
        public int NavigationMode { get; set; } = 2;
    }
}

