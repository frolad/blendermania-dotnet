//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System.Text.Json;
using System.Text.Json.Serialization;
using blendermania_dotnet;
using GBX.NET;
using GBX.NET.Engines.Game;


//var user = GameBox.ParseNode<CGamePlayerProfile>("niklas.Profile.Gbx");
//Console.WriteLine(user.OnlineLogin);

if (string.IsNullOrEmpty(args.ElementAtOrDefault(0)))
{
    throw new Exception("Json string expected as first and only argument");
}

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var data = JsonSerializer.Deserialize<MapObjects>(args[0], options);

if (string.IsNullOrEmpty(data?.MapName))
    throw new Exception("Map name is null");

if (data.Blocks is null || data.Blocks.Count == 0)
    throw new Exception("Blocks are 0 or null");

var map = GameBox.ParseNode<CGameCtnChallenge>(data.MapName);


foreach (var block in data.Blocks)
{
    var newBlockIdent = new Ident(Id: block.Name!, Collection: "Stadium", Author: "skyslide");
    var newBlockDirection = Direction.West;
    var newBlockCoords = new Int3(block.Pos[0], block.Pos[1], block.Pos[2]);

    var newBlock = new CGameCtnBlock(
                        blockModel: newBlockIdent,
                        direction: newBlockDirection,
                        coord: newBlockCoords,
                        flags: 0);

    map.Blocks.Add(newBlock);


}

await map.SaveAsync("edited_" + data.MapName);










//return 0;
//foreach (var block in map.Blocks ?? new List<CGameCtnBlock>() )
//{
//    Console.WriteLine(block.Name);
//}


//foreach (var item in map.AnchoredObjects ?? new List<CGameCtnAnchoredObject>() )
//{

//    if (item.ItemModel.Id.Contains("venus"))
//    {
//        item.AbsolutePositionInMap =
//            item.AbsolutePositionInMap with { Y = item.AbsolutePositionInMap.Y + 1024 };

//    }

//}

//await map.SaveAsync("modified-items-z.Map.Gbx");

//Console.WriteLine(map.DayDuration);
//Console.WriteLine(map.DayTime);
//Console.WriteLine(map.DynamicDaylight);

//foreach (string arg in args)
//{
//    Console.WriteLine(arg);
//}

//return 0;