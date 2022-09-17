using GBX.NET;
using GBX.NET.Engines.Game;

namespace blendermania_dotnet
{
    public class MediaTrackerClip
    {
        public string? Name { get; set; }
        public List<Int3> Positions { get; set; } = new();

    }
}