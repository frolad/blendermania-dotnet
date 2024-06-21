using GBX.NET;
using GBX.NET.Engines.Game;
using static GBX.NET.Engines.Game.CGameCtnAnchoredObject;

namespace blendermania_dotnet
{
    public class Item
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public Vector3 Position { get; set; } = new Vector3();
        public Vector3 Rotation { get; set; } = new Vector3();
        public Vector3 Pivot { get; set; } = new Vector3();
        public EPhaseOffset? AnimPhaseOffset { get; set; }

        public CGameCtnChallenge AddItemToMap(CGameCtnChallenge Map, string Env)
        {
            if (Path is null || Name is null)
            {
                throw new Exception("Null Path or Name");
            }

            // embed only if it's not embedded already
            if (Path.Length > 0 && (Map.EmbeddedData is null || !Map.EmbeddedData.ContainsKey(Name)))
            {
                Map.ImportFileToEmbed(Path, "Items");
            }

            if (Name.ToLower().StartsWith("items") || Name.ToLower().StartsWith("blocks"))
            {
                var parts = Name.Split("/");
                Name = String.Join("/", parts.Skip(1).Take(parts.Length).ToArray());
            }

            var id = new Id(Env);
            if (Env == "Stadium2020")
            {
                id = new Id(26);
            }

            // add anchor to the item
            var item = Map.PlaceAnchoredObject(
                new Ident(Name.Replace("/", @"\"), id, "Blendermania"),
                Position.ToGBXNetVec3(),
                Rotation.ToGBXNetVec3(),
                Pivot.ToGBXNetVec3()
            );


            item.AnimPhaseOffset = AnimPhaseOffset;

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