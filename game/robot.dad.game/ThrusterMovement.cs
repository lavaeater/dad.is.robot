using System;
using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game
{
    public class ThrusterMovement : Movement
    {
        private float _rotationSpeed;
        private readonly float _drag;
        private readonly Axis _axis;
        private float _angle;
        private readonly float _maxThrust;
        private float _currentThrust;
        private readonly float _maxSpeed;
        private float _currentSpeed;
        private int _speedX;
        private int _speedY;
        private readonly bool _enableThrustGraphic;
        public bool Freeze;
        private int _smallThrust;
        private int _mediumThrust;

        public ThrusterMovement(float rotationSpeed, float drag, Axis axis, float angle, float maxThrust, float maxSpeed, bool enableThrustGraphic)
        {
            _rotationSpeed = rotationSpeed;
            _drag = drag;
            _axis = axis;
            _angle = angle;
            _maxThrust = maxThrust;
            _maxSpeed = maxSpeed;
            _enableThrustGraphic = enableThrustGraphic;
            _currentSpeed = 0;
            _speedX = 0;
            _speedY = 0;
            _smallThrust = (int)_maxThrust / 2;
            _mediumThrust = (int)_maxThrust - (int)_maxThrust / 4;
        }

        public override void Update()
        {
            if (Freeze) return;

            if (_axis != null)
            {
                if (Math.Abs(_axis.Y) > 0)
                {
                    _currentThrust = Util.Approach(_currentThrust, _maxThrust, 5);
                }
                else
                {
                    _currentThrust = 0;
                }
            }

            if (_enableThrustGraphic)
            {
                int currThrust = (int)_currentThrust;
                if (0 < currThrust && currThrust < _smallThrust)
                {
                    _rotationSpeed = 5;
                    SmallThrust.Visible = true;
                }
                else if (_smallThrust < currThrust && currThrust < _mediumThrust)
                {
                    _rotationSpeed = 3;
                    MediumThrust.Visible = true;
                }
                else if (_mediumThrust < currThrust)
                {
                    _rotationSpeed = 2;
                    MaxThrust.Visible = true;
                }
                else if (currThrust == 0)
                {
                    if (_currentSpeed < 1)
                    {
                        _rotationSpeed = 0;
                    }
                    else
                    {
                        _rotationSpeed = 5;
                    }
                    SmallThrust.Visible = false;
                    MediumThrust.Visible = false;
                    MaxThrust.Visible = false;
                }
            }

            _angle = Util.ApproachAngle(_angle, Util.WrapAngle(_angle + (_rotationSpeed * -_axis.X)), _rotationSpeed);

            Entity.Graphics.ForEach(g => g.Angle = _angle + 90f);




            if (_currentThrust > 0)
            {
                _currentSpeed = Util.Approach(_currentSpeed, _maxSpeed, _currentThrust);
            }
            else
            {
                _currentSpeed = Util.Approach(_currentSpeed, 0, _drag);
            }
            _speedX = (int)Util.PolarX(_angle, _currentSpeed);
            _speedY = (int)Util.PolarY(_angle, _currentSpeed);


            MoveXY(_speedX, _speedY, Collider);

            if (OnMove != null)
            {
                OnMove();
            }
        }

        private readonly Graphic SmallThrust = SpritePipe.ThrustOne;
        private readonly Graphic MediumThrust = SpritePipe.ThrustTwo;
        private readonly Graphic MaxThrust = SpritePipe.ThrustThree;


        public override void Added()
        {
            if (_enableThrustGraphic)
            {

                float originY = SmallThrust.ScaledHeight + Entity.Graphic.ScaledHeight / 2;
                SmallThrust.SetOrigin(SmallThrust.HalfWidth / 2 +1, originY);
                MediumThrust.SetOrigin(MediumThrust.HalfWidth / 2 +1, originY);
                MaxThrust.SetOrigin(MaxThrust.HalfWidth / 2 +1, originY);

                SmallThrust.Angle = 90;
                MediumThrust.Angle = 90;
                MaxThrust.Angle = 90;

                SmallThrust.Visible = false;
                MediumThrust.Visible = false;
                MaxThrust.Visible = false;

                Entity.AddGraphics(SmallThrust, MediumThrust, MaxThrust);
            }
        }
    }
}