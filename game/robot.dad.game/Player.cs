using System;
using Otter;
using robot.dad.game.Sprites;
using robot.dad.graphics;

namespace robot.dad.game
{
    public class Player : Entity
    {
        public readonly Session Session;
     
        public Player(float x, float y, Session session)
        {
            //_yOrigin = 200f;//173f;
            //_xOrigin = 50f;
            Init();
            //hmmm
            var axis = Axis.CreateArrowKeys();

            var movement = new ThrusterMovement(3, 5, axis, 90f, 200f, 500f, true);
            AddComponents(axis, movement);

            Session = session;
            X = x;
            Y = y;
        }

        private void Init()
        {
            // Create an Image using the path passed in with the constructor
            AddGraphics(SpritePipe.Ship);
            Graphic.CenterOrigin();
            Graphic.Scale = 0.5f;
        }

        public override void Update()
        {
            base.Update();
            Scene.BringToFront(this);
        }
    }
}