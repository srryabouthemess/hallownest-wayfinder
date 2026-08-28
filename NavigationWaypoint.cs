namespace HallownestWayfinder
{
    /// <summary>
    /// Um ponto de passagem dentro de uma sala. As coordenadas usam o mesmo
    /// espaço de mundo do HeroController. Vários pontos da mesma sala podem
    /// ser ordenados para formar um pequeno percurso interno.
    /// </summary>
    public sealed class NavigationWaypoint
    {
        public string Scene { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public int Order { get; set; }
        public float ArrivalRadius { get; set; } = 2.5f;
        public string? Label { get; set; }
        public string? TargetObjectName { get; set; }
    }
}

