using ca.axoninteractive.Geometry.Hex;
using robot.dad.game.Components;

namespace robot.dad.game.Event
{
    public class RuinEvent : EventBase
    {
        public RuinEvent()
        {
            
        }

        public RuinEvent(CubicHexCoord hex) : base(hex)
        {
            
        }

        protected override void AddTileEntity()
        {
            base.AddTileEntity();
            Entity.AddComponent(new AggressorComponent(this, 2, Done));
        }

        public void Done()
        {
            EventDone = true;
        }
    }
}