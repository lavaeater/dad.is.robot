using ca.axoninteractive.Geometry.Hex;
using robot.dad.game.Components;

namespace robot.dad.game.Event
{
    public class ScavengerEvent : EventBase
    {
        public ScavengerEvent()
        {
            
        }

        public ScavengerEvent(CubicHexCoord hex):base(hex)
        {
            
        }

        protected override void AddTileEntity()
        {
            base.AddTileEntity();
            Entity.AddComponent(new AggressorComponent(this, 2, Reveal));
            EntityVisible = false;
            ShouldUpdate = true;
        }

        private void Reveal()
        {
            EventDone = true;
            EntityVisible = true;
            
        }

        public override void Show()
        {
            ShouldUpdate = true;
        }
    }
}