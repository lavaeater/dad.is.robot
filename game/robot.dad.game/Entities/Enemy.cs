using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class Enemy : Entity
    {
        public Enemy(float x, float y)
        {
            X = x;
            Y = y;
            this.Graphic = SpritePipe.Ship;
            this.Graphic.Scale = 0.5f;
        }
    }
}