using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blendermania_dotnet
{
    public class Block
    {
        public string? Name { get; set; }
        public List<int>? Pos { get; set; }
        public List<float>? Rot { get; set; }
        public string? Type { get; set; }
    }

    public class Item
    {
        public string? Name { get; set; }
        public List<float>? Pos { get; set; }
        public List<float>? Rot { get; set; }
        public string? Type { get; set; }
    }

    public class MapObjects
    {
        public List<Block>? Blocks { get; set; }
        public List<Item>? Items { get; set; }
        public string? MapName { get; set; }
    }
}
