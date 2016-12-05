using System;
using Otter;
using robot.dad.game.Sprites;
using robot.dad.graphics;

namespace robot.dad.game
{
    public class Player : Entity
    {
        public readonly Session Session;
        private ThrustComponent _thrustComponent;

        public Player(float x, float y, Session session)
        {
            //_yOrigin = 200f;//173f;
            //_xOrigin = 50f;
            Init();
            //hmmm
            var axis = Axis.CreateArrowKeys();
            var movement = new ThrusterMovement(500, 5, 100, 50, axis);
            //_thrustComponent = new ThrustComponent(this, _axis);
            AddComponents(axis, movement);//, _thrustComponent);

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
            //SpritePipe.Ship.CenterOrigin();
            //SpritePipe.Ship.Scale = 0.5f;

        }

        public override void Update()
        {
            base.Update();
            Scene.BringToFront(this);
        }
    }

    public class ThrustComponent : Component
    {
        private Player _player;
        private Axis _axis;
        private Graphic[] Images = new Graphic[3];

        public ThrustComponent(Player player, Axis _axis)
        {
            Images[1] = SpritePipe.ThrustOne;
            Images[2] = SpritePipe.ThrustTwo;
            Images[3] = SpritePipe.ThrustThree;

            this._player = player;
            this._axis = _axis;
        }

        public override void Update()
        {
            ////Set image according to thrust
            //if (_axis.Y != 0f)
            //{
            //    _player.Graphic = _player.Images[3];
            //}
            //else
            //{
            //    _player.Graphic = _player.Images[0];
            //}
        }
    }
}