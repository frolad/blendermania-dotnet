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

var oayload = args.ElementAtOrDefault(1);
if (string.IsNullOrEmpty(oayload))
{
    throw new Exception("Payload path is not provided");
}


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
            {
                var json = File.ReadAllText(oayload);
                var map = JsonSerializer.Deserialize<PlaceObjectsOnMap>(json, options);
                if (map is null) { throw new Exception("Invalid json"); }
                await map.Exec();
                Console.Write($"SUCCESS");
            }
            break;

        case "convert-item-to-obj":
            {
                var json = File.ReadAllText(oayload);
                var item = JsonSerializer.Deserialize<ConvertItemToObj>(json, options);
                if (item is null) { throw new Exception("Invalid json"); }
                var outputFile = item.Exec();
                Console.Write($"SUCCESS: {outputFile}");
            }
            break;

        case "place-mediatracker-clip-on-map":
            {
                var json = File.ReadAllText(oayload);
                var map = JsonSerializer.Deserialize<PlaceMediaTrackerClipOnMap>(json, options);
                if (map is null) { throw new Exception("Invalid json"); }
                await map.Exec();
                Console.Write($"SUCCESS");
            }
            break;
        
        case "get-mediatracker-clips":
            var mtData = new MediaTrackerClipsData(MapPath: oayload);
            var jsonPath = mtData.WriteJsonFileGetPath();
            Console.Write(jsonPath);
            break;

        default:
            throw new Exception("No such command: " + command);
    }
}
catch (System.Exception err)
{
    Console.Write($"ERROR: {err.Message}");

    //throw;
}