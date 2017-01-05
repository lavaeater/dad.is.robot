using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;
using Otter;
using robot.dad.game.Components;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class ScavengerEvent : TileEvent
    {
        public ScavengerEvent(string eventType, CubicHexCoord hex)
        {
            Hex = hex;
            EventType = eventType;
            AddTileEntity();
            Entity.AddComponents(new AggressorComponent(this, 2));
            Entity.Visible = false;
            ShouldUpdate = true;
        }

        public override void Identify()
        {
            /*
             * Could trigger the Pirates-like dialogue where they 
             * player tries to get away before the fight. Right now, you just get attacked!
             */
        }
    }

    public class TileEntity : Entity
    {
        
    }

    public class TileEvent
    {
        private TileEntity _entity;
        public CubicHexCoord Hex { get; set; }
        public string EventType;
        [JsonIgnore]
        public TileEntity Entity { get; set; }

        public TileEvent()
        {
            AddTileEntity();
        }

        public TileEvent(string eventType, CubicHexCoord hex)
        {
            Hex = hex;
            EventType = eventType;
            AddTileEntity();

            if (eventType == "Ruin")
            {
                Entity.AddComponents(new AggressorComponent(this, 2));
            }

            IdentifiedImage = SpritePipe.Ruin;

            ShouldUpdate = true;
            UnknownImage = SpritePipe.UnknownTile;

            Entity.Graphic = SpritePipe.UnknownTile;

            Entity.Graphic.CenterOrigin();
        }

        protected void AddTileEntity()
        {
            Entity = new TileEntity {Visible = false};
            var tilePos = graphics.Hex.Grid.CubicToPoint(Hex);

            Entity.X = tilePos.x;
            Entity.Y = tilePos.y;
        }

        [JsonIgnore]
        public Image UnknownImage { get; set; }

        public virtual void Identify()
        {
            if (!Identified)
            {
                Identified = true;
                Entity.Graphic = IdentifiedImage;
                Entity.Graphic.CenterOrigin();
            }
        }

        [JsonIgnore]
        public Image IdentifiedImage { get; set; }

        public bool Identified { get; set; }
        public bool ShouldUpdate { get; set; }
    }
}