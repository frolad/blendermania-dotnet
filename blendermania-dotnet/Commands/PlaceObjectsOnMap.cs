using GBX.NET;
using GBX.NET.Engines.Game;

namespace blendermania_dotnet
{
    class PlaceObjectsOnMap
    {
        public string? MapPath { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        // TODO
        public List<Block> Blocks { get; set; } = new List<Block>();

        async public Task Exec()
        {
            if (MapPath is null || MapPath.Length == 0)
            {
                throw new Exception("Map file path is null or empty");
            }

            // parse map
            var map = GameBox.ParseNode<CGameCtnChallenge>(MapPath);

            // clean up
            if (map.EmbeddedObjects is not null)
            {
                map.EmbeddedObjects.Clear();
            }

            if (map.AnchoredObjects is not null)
            {
                map.AnchoredObjects.Clear();
            }

            // palce items
            foreach (var item in Items)
            {
                map = item.AddItemToMap(map);
            }

            // TODO place blocks

            await map.SaveAsync(MapPath);
        }
    }
}