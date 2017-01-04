using Newtonsoft.Json;

namespace robot.dad.common
{
    public interface IItem
    {
        [JsonIgnore]
        string ItemKey { get; }
        string Name { get; set; }
        string Description { get; set; }
    }
}