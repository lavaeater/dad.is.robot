using System;
using Otter;
using robot.dad.game.Sprites;
using robot.dad.graphics;

namespace robot.dad.game
{
    public class Player : ImageEntity
    {
        public readonly Session Session;
        private readonly Axis _axis;
        private readonly ThrusterMovement _movement;
        private ThrustComponent _thrustComponent;

        public Image[] Images = new Image[4];
        private float _yOrigin;
        private float _xOrigin;

        public Player(float x, float y, string imagePath, Session session) : base(x, y, imagePath)
        {
            _yOrigin = 200f;//173f;
            _xOrigin = 50f;
            Init();
            //hmmm
            _axis = Axis.CreateArrowKeys();
            _movement = new ThrusterMovement(500, 5, 100, 50, _axis);
            _thrustComponent = new ThrustComponent(this, _axis);
            AddComponents(_axis, _movement, _thrustComponent);

            Session = session;
        }

        private void Init()
        {
            // Create an Image using the path passed in with the constructor
            Images[0] = SpritePipe.Ship0;
            Images[1] = SpritePipe.Ship1;
            Images[2] = SpritePipe.Ship2;
            Images[3] = SpritePipe.Ship3;

            foreach (Image image in Images)
            {
                image.SetOrigin(_xOrigin, _yOrigin);
                image.Scale = 0.5f;
            }

            Graphic = Images[0];
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

        public ThrustComponent(Player player, Axis _axis)
        {
            this._player = player;
            this._axis = _axis;
        }

        public override void Update()
        {
            //Set image according to thrust
            if (_axis.Y != 0f)
            {
                _player.Graphic = _player.Images[3];
            }
            else
            {
                _player.Graphic = _player.Images[0];
            }
        }
    }
}