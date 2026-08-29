using System;
using System.Collections.Generic;

namespace HallownestWayfinder
{
    public sealed class FreeNavigationDestination
    {
        public string Id { get; set; } = string.Empty;
        public string Scene { get; set; } = string.Empty;
    }

    public static class FreeNavigationCatalog
    {
        private static readonly FreeNavigationDestination[] Items =
        {
            // Stag scene IDs match RandomizerMod Resources/Data/locations.json.
            Destination("dirtmouth", "Town"),
            Destination("crossroads_station", "Crossroads_47"),
            Destination("greenpath_station", "Fungus1_16_alt"),
            Destination("queens_station", "Fungus2_02"),
            Destination("city_storerooms", "Ruins1_29"),
            Destination("resting_grounds", "RestingGrounds_09"),
            Destination("kings_station", "Ruins2_08"),
            Destination("distant_village", "Deepnest_09"),
            Destination("hidden_station", "Abyss_22")
        };

        public static IReadOnlyList<FreeNavigationDestination> Destinations => Items;

        public static FreeNavigationDestination Get(int index) =>
            Items[ClampIndex(index)];

        public static int ClampIndex(int index) =>
            Math.Max(0, Math.Min(index, Items.Length - 1));

        public static string[] MenuNames()
        {
            string[] names = new string[Items.Length + 1];
            names[0] = LocalizationService.Text("off");
            for (int index = 0; index < Items.Length; index++)
                names[index + 1] = Name(Items[index]);
            return names;
        }

        public static string Name(FreeNavigationDestination destination) =>
            LocalizationService.Text("destination." + destination.Id);

        private static FreeNavigationDestination Destination(string id, string scene) =>
            new FreeNavigationDestination { Id = id, Scene = scene };
    }
}
