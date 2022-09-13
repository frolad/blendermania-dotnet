using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using GBX.NET;
using GBX.NET.Engines.Game;

namespace blendermania_dotnet
{
    class InGameMediaTrackerClips
    {
        public string? ClipName { get; set; }
        public List<Int3> Triggers { get; set; } = new();
    }

    public class MediaTrackerClipsData
    {
        public MediaTrackerClipsData(string MapPath)
        {
            this.MapPath = MapPath;
        }
        public string MapPath { get; set; }

        public string WriteJsonFileGetPath()
        {
            var map      = GameBox.ParseNode<CGameCtnChallenge>(MapPath);
            var clips    = map.ClipGroupInGame?.Clips;
            var jsonPath = MapPath + ".MediatrackerData.json";

            List<InGameMediaTrackerClips> igClips = new();

            if (clips is not null)
            foreach (var clip in clips)
            {
                var igClip = new InGameMediaTrackerClips();
                    igClip.ClipName = clip.Clip.Name;

                foreach (var coord in clip.Trigger.Coords)
                {
                    igClip.Triggers.Add(new Int3
                    {
                        X = coord.X,
                        Y = coord.Y,
                        Z = coord.Z
                    });
                }
                igClips.Add(igClip);
            }
            else
                igClips.Add(new() { ClipName = "None found"});

            var data = JsonSerializer.Serialize(igClips);

            File.WriteAllText(jsonPath, data);

            return jsonPath;
        }
    }
    class PlaceMediaTrackerClipOnMap
    {
        public string? MapPath { get; set; }
        public List<MediaTrackerClip> MTClips { get; set; } = new();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Block> Blocks { get; set; } = new List<Block>();
        public bool ShouldOverwrite { get; set; } = false;
        public string MapSuffix { get; set; } = "_modified";
        public bool CleanBlocks { get; set; } = true;
        public bool CleanItems { get; set; } = true;
        public string Env { get; set; } = "Stadium";

        async public Task Exec()
        {
            throw new NotImplementedException();

            if (MapPath is null || MapPath.Length == 0)
            {
                throw new Exception("Map file path is null or empty");
            }

            // parse map
            var map = GameBox.ParseNode<CGameCtnChallenge>(MapPath);

            var clips = map.ClipGroupInGame?.Clips;

            if (clips is null)
                return;

            foreach (var clip in clips)
            {
                var newCoords = new List<Int3>();

                foreach(var coord in clip.Trigger.Coords)
                {
                    newCoords.Add(new Int3 {
                        X = coord.X + 3,
                        Y = coord.Y + 128,
                        Z = coord.Z + 0
                    });
                }

                var coordsArray = newCoords.ToArray();

                clip.Trigger.Coords = coordsArray;
            }

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
