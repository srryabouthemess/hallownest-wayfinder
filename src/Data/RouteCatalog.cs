namespace HallownestWayfinder
{
    public static class RouteCatalog
    {
        public static readonly System.Collections.Generic.IReadOnlyList<RoutePlan> Routes =
            RouteDataLoader.Load();
    }
}
