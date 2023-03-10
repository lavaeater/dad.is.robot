using ca.axoninteractive.Geometry.Hex;
using Otter;
using robot.dad.game.Components;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class TileEvent : Entity
    {
        public CubicHexCoord Hex { get; set; }
        public string EventType;

        public TileEvent(string eventType, CubicHexCoord hex)
        {
            Hex = hex;
            this.EventType = eventType;

            if (eventType == "Ruin")
            {
                AddComponents(new AggressorComponent(this, 2));
            }

            IdentifiedImage = SpritePipe.Ruin;

            Visible = false;

            Graphic = SpritePipe.UnknownTile;
            Graphic.CenterOrigin();
            var tilePos = graphics.Hex.Grid.CubicToPoint(Hex);

            X = tilePos.x;
            Y = tilePos.y;
        }

        public void Identify()
        {
            if (!Identified)
            {
                Identified = true;
                Graphic = IdentifiedImage;
                Graphic.CenterOrigin();
            }
        }

        public Image IdentifiedImage { get; set; }

        public bool Identified { get; set; }
    }
}