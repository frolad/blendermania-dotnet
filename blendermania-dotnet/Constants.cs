using System.Text.Json;

namespace blendermania_dotnet;

public static class Constants
{
    public static JsonSerializerOptions CommonJsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };


    public const string PLACE_OBJECTS_ON_MAP_COMMAND = "place-objects-on-map";
}