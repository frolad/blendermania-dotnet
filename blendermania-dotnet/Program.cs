using GBX.NET;
using GBX.NET.LZO;
using GBX.NET.Engines.Game;
using blendermania_dotnet;
using System.Text.Json;
using GBX.NET.Exceptions;

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

// GBX.NET.Lzo.SetLzo(typeof(GBX.NET.LZO.MiniLZO));
try
{

    var command = args.ElementAtOrDefault(0);
    if (string.IsNullOrEmpty(command))
    {
        throw new Exception("Command is not provided");
    }

    var payload = args.ElementAtOrDefault(1);
    if (string.IsNullOrEmpty(payload))
    {
        throw new Exception("Payload path is not provided");
    }




    switch (command)
    {
        case PlaceObjectsOnMap.COMMAND_NAME:
            await PlaceObjectsOnMap.Execute(payload);
            break;

        default:
            throw new Exception("No such command: " + command);
    }

    return (int)ExitCodes.Success;
}


catch (Exception err)
{
    Console.WriteLine("ERROR:");
    Console.WriteLine(err.ToString());
    
    switch(err)
    {
        case NotAGbxException:
            return (int)ExitCodes.NotAGbx;

        case ArgumentNullException:
            return (int)ExitCodes.ValueIsNull;

        default:
            return (int)ExitCodes.UnknownError;
    }
}