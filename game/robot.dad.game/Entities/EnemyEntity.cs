using System;
using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class EnemyEntity : Entity
    {
        public EnemyEntity(float x, float y)
        {
            X = x;
            Y = y;
            Graphic = SpritePipe.EnemySprite;
            Graphic.Scale = 0.5f;
        }
    }
}