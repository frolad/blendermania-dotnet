using GBX.NET;
using GBX.NET.Engines.Game;

namespace blendermania_dotnet
{
    public class Item
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public Vector3 Position { get; set; } = new Vector3();
        public Vector3 Rotation { get; set; } = new Vector3();
        public Vector3 Pivot { get; set; } = new Vector3();

        public CGameCtnChallenge AddItemToMap(CGameCtnChallenge Map)
        {
            if (Path is null || Name is null)
            {
                throw new Exception("Null Path or Name or Position");
            }

            // embed only if it's not embedded already
            if (Map.EmbeddedObjects is null || !Map.EmbeddedObjects.ContainsKey(Name))
            {
                Map.ImportFileToEmbed(Path, "Items");
            }

            // add anchor to the item
            var item = Map.PlaceAnchoredObject(
                new Ident(Name.Replace("Items/", "").Replace("/", @"\"), new Id(26), "Blendermania"),
                Position.ToGBXNetVec3(),
                Rotation.ToGBXNetVec3(),
                Pivot.ToGBXNetVec3()
            );

            // GBX.net workaround, wait for fix to remove
            var chunk = Map.GetChunk<CGameCtnChallenge.Chunk03043040>();
            if (chunk != null)
            {
                chunk.Version = 4;
                chunk.U04 = null;
            }
            Map.RemoveChunk<CGameCtnChallenge.Chunk03043062>();
            Map.RemoveChunk<CGameCtnChallenge.Chunk03043063>();
            Map.RemoveChunk<CGameCtnChallenge.Chunk03043065>();
            Map.RemoveChunk(0x03043068);
            Map.RemoveChunk(0x03043069);

            return Map;
        }

        public CGameCtnMacroBlockInfo AddItemToMacroblock(CGameCtnMacroBlockInfo Macro)
        {
            if (Path is null || Name is null)
            {
                throw new Exception("Null Path or Name or Position");
            }

            if (Macro.ObjectSpawns is not null)
            {
                var obj = new CGameCtnMacroBlockInfo.ObjectSpawn();
                obj.ItemModel = new Ident(Name.Replace("Items/", "").Replace("/", @"\"), new Id(26), "Blendermania");
                obj.AbsolutePositionInMap = Position.ToGBXNetVec3();
                obj.PitchYawRoll = Rotation.ToGBXNetVec3();
                obj.PivotPosition = Pivot.ToGBXNetVec3();

                Macro.ObjectSpawns.Add(obj);
            }

            return Macro;
        }
    }
}