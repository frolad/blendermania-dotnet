using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.Engines.Game;
using blendermania_dotnet;
using System.Text.Json;

// run to debug:
// dotnet run -- <command> <payload>

// run to publish:
// dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained false -c Release

// run to start
// blendermania-dotnet.exe <command> <string json payload>
// commands: "place-objects-on-map"

// PAYLOAD example for "place-objects-on-map"
/*
{
    "Path": "C:/Users/Vladimir/Documents/Trackmania/Maps/Debuger/TestMap.Map.Gbx",
    "Items": [
        {
            "Name": "TestItem.Item.Gbx",
            "Path": "C:/Users/Vladimir/Documents/Trackmania/Items/TestItem.Item.Gbx",
            "Position": {"X": 0,"Y": 0,"Z": 0},
            "Rotation": {"X": 0,"Y": 0,"Z": 0},
            "Pivot": {"X": 0,"Y": 0,"Z": 0}
        }
    ]
}
*/

GBX.NET.Lzo.SetLzo(typeof(GBX.NET.LZO.MiniLZO));

var command = args.ElementAtOrDefault(0);
if (string.IsNullOrEmpty(command))
{
    throw new Exception("Command is not provided");
}

var payload = args.ElementAtOrDefault(1);
if (string.IsNullOrEmpty(payload))
{
    throw new Exception("Payload is not provided");
}

// move bellow to separate file?
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

switch (command)
{
    case "place-objects-on-map":
        var body = JsonSerializer.Deserialize<PlaceObjectsOnMap>(System.Text.RegularExpressions.Regex.Unescape(payload), options);
        if (body is null)
        {
            throw new Exception("Invalid json");
        }
        await body.Exec();
        break;

    default:
        throw new Exception("No such command: " + command);
}