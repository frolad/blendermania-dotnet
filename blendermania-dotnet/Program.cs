using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.Engines.Game;
using blendermania_dotnet;
using System.Text.Json;

// run to debug:
// dotnet run -- <command> <payload>

// run to publish:
// dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true -c Release

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

var payloadPath = args.ElementAtOrDefault(1);
if (string.IsNullOrEmpty(payloadPath))
{
    throw new Exception("Payload path is not provided");
}

var payload = File.ReadAllText(payloadPath);

// move bellow to separate file?
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

try
{
    switch (command)
    {
        case "place-objects-on-map":
            var map = JsonSerializer.Deserialize<PlaceObjectsOnMap>(payload, options);
            if (map is null) { throw new Exception("Invalid json"); }
            await map.Exec();
            Console.Write($"SUCCESS");
            break;

        case "convert-item-to-obj":
            var item = JsonSerializer.Deserialize<ConvertItemToObj>(payload, options);
            if (item is null) { throw new Exception("Invalid json"); }
            var outputFile = item.Exec();
            Console.Write($"SUCCESS: {outputFile}");
            break;

        default:
            throw new Exception("No such command: " + command);
    }
}
catch (System.Exception err)
{
    Console.Write($"ERROR: {err.Message}");
}