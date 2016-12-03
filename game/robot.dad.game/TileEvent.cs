using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game
{
    public class TileEvent : Entity
    {
        public string EventType;

        public TileEvent(string eventType, float x, float y) : base(x, y)
        {
            this.EventType = eventType;
            //Set image, set position

            // Create an Image using the path passed in with the constructor
            var image = SpritePipe.Ruin;
            // Center the origin of the Image
            //image.CenterOrigin();
            //image.Scale = 0.5f;
            // Add the Image to the Entity's Graphic list.
            AddGraphic(image);
            Visible = false;
            X = x;
            Y = y;
        }
    }
}