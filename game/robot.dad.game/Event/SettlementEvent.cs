using ca.axoninteractive.Geometry.Hex;

namespace robot.dad.game.Event
{
    public class SettlementEvent : EventBase
    {
        public SettlementEvent()
        {
            
        }

        public SettlementEvent(CubicHexCoord hex) :base(hex)
        {
            
        }
    }
}