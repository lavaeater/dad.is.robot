using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;
using Otter;

namespace robot.dad.game.Event
{
    public interface IEvent
    {
        CubicHexCoord Hex { get; set; }
        [JsonIgnore]
        Entity TileEntity { get; }
        bool ShouldUpdate { get; set; }
        bool EntityVisible { get; set; }
        bool Identified { get; set; }
        bool EventDone { get; set; }
        void Identify();
        void Hide();
        void Show();
    }
}