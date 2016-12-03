using ca.axoninteractive.Geometry.Hex;
using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game
{
    public class TileEvent : Entity
    {
        public CubicHexCoord Hex { get; set; }
        public string EventType;
        private int _xDiff;
        private int _yDiff;

        public TileEvent(string eventType, CubicHexCoord hex)
        {
            Hex = hex;
            this.EventType = eventType;

            IdentifiedImage = SpritePipe.Ruin;
            
            Visible = false;

            Graphic = SpritePipe.UnknownTile;
            Graphic.CenterOrigin();
            var tilePos = Otter.Custom.Hex.Grid.CubicToPoint(Hex);

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