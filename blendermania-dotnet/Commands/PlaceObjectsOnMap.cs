using GBX.NET;
using GBX.NET.Engines.Game;

namespace blendermania_dotnet
{
    class PlaceObjectsOnMap
    {
        public string? MapPath { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Block> Blocks { get; set; } = new List<Block>();
        public bool ShouldOverwrite { get; set; } = false;
        public string MapSuffix { get; set; } = "_modified";
        public bool CleanBlocks { get; set; } = true;
        public bool CleanItems { get; set; } = true;
        public string Env { get; set; } = "Stadium";

        async public Task Exec()
        {
            if (MapPath is null || MapPath.Length == 0)
            {
                throw new Exception("Map file path is null or empty");
            }

            // parse map
            var map = GameBox.ParseNode<CGameCtnChallenge>(MapPath);

            // clean up existed data
            if (CleanBlocks) // DOESN'T WORK ATM IN GBX.NET
            {
                if (Blocks.Count > 0 && map.Blocks is not null)
                {
                    //map.Blocks.Clear();
                }
            }

            if (CleanItems)
            {
                if (map.EmbeddedData is not null)
                {
                    map.EmbeddedData.Clear();
                }

                if (map.AnchoredObjects is not null)
                {
                    map.AnchoredObjects.Clear();
                }
            }

            // palce items
            foreach (var item in Items)
            {
                map = item.AddItemToMap(map, Env);
            }

            // palce blocks
            foreach (var block in Blocks) // VERY UNSTABLE
            {
                //map = block.AddBlockToMap(map);
            }

            // save modified map
            var NewPath = MapPath;
            if (!ShouldOverwrite)
            {
                if (MapSuffix.Trim().Count() == 0)
                {
                    MapSuffix = "_modified";
                }

                map.MapName += MapSuffix;

                // change file name
                var dir = Path.GetDirectoryName(NewPath);
                var fn = Path.GetFileNameWithoutExtension(NewPath);
                var ext = Path.GetExtension(NewPath);
                if (fn.ToLower().Contains(".map"))
                {
                    fn = Path.GetFileNameWithoutExtension(fn) + MapSuffix + ".Map";
                }
                else
                {
                    fn = fn + MapSuffix;
                }

                if (dir is null)
                {
                    dir = "";
                }
                NewPath = Path.Combine(dir, fn + ext);
            }

            await map.SaveAsync(NewPath);
        }
    }
}