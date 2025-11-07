
namespace blendermania_dotnet;

public class BMMap
{
    public string MapPath { get; set; } = "";
    public List<Item> Items { get; set; } = [];
    public List<Block> Blocks { get; set; } = [];
    public string Env { get; set; } = "Stadium";
}