using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.Sprites;

namespace robot.dad.game.Event
{
    public abstract class EventBase : IEvent
    {
        protected TileEntity Entity;

        protected EventBase()
        {

        }

        protected EventBase(CubicHexCoord hex) : this()
        {
            Hex = hex;
        }

        public CubicHexCoord Hex { get; set; }

        public virtual Entity TileEntity
        {
            get
            {
                if (Entity == null)
                {
                    AddTileEntity();
                }
                return Entity;
            }
        }

        public bool ShouldUpdate { get; set; }

        [JsonIgnore]
        public virtual bool EntityVisible
        {
            get { return Entity.Visible; }
            set { Entity.Visible = value; }
        }

        public bool Identified { get; set; }
        public bool EventDone { get; set; }

        public virtual void Identify()
        {
            if (Identified) return;
            Identified = true;
            Entity.Graphic = IdentifiedImage;
            Entity.Graphic.CenterOrigin();
        }

        public virtual void Hide()
        {
            TileEntity.Visible = false;
            ShouldUpdate = false;
        }

        public virtual void Show()
        {
            TileEntity.Visible = true;
            ShouldUpdate = true;
        }

        protected virtual void AddTileEntity()
        {
            Entity = new TileEntity { Visible = false };
            var tilePos = graphics.Hex.Grid.CubicToPoint(Hex);

            Entity.X = tilePos.x;
            Entity.Y = tilePos.y;
            Entity.Graphic = SpritePipe.UnknownTile;
            Entity.Graphic.CenterOrigin();
        }

        [JsonIgnore]
        public virtual Image UnknownImage { get; set; } = SpritePipe.UnknownTile;

        [JsonIgnore]
        public virtual Image IdentifiedImage { get; set; } = SpritePipe.Ruin;

    }
}