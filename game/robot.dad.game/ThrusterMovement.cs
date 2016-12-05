using System;
using Otter;

namespace robot.dad.game
{
    public class ThrusterMovement : Movement
    {
        private readonly float MaxSpeed;
        private readonly Speed TargetSpeed;
        private readonly float _rotationSpeed;
        private readonly float _drag;
        private readonly float _roation;
        private readonly Speed Speed;
        private readonly Axis _axis;
        private float _angle;

        private readonly float _maxAcceleration;
        private float _currentAcceleration;

        private readonly float _maxSpeed;
        private float _currentSpeed;

        public ThrusterMovement(float maxSpeed, float rotationSpeed, float drag, float acceleration, Axis axis)
        {
            //Hardcoded values until we get it right!
            /*
             * When the player presses the accelerator, acceleration goes up! (of course)
             * So, acceleration AND speed have maxValues, and currentValues
             * 
             * _currentSpeed is then lowered by drag in opposite direction
             */
            _angle = 90;
            _drag = 5;
            _axis = axis;
            _currentAcceleration = 0;
            _maxAcceleration = 200;
            _currentSpeed = 0;
            _maxSpeed = 500;
            _rotationSpeed = 3;
        }


        public override void Update()
        {
            if (Freeze) return;

            _angle = Util.ApproachAngle(_angle, Util.WrapAngle(_angle + (_rotationSpeed * -_axis.X)), _rotationSpeed);

            Entity.Graphics.ForEach(g => g.Angle = _angle + 90f);

            if (_axis != null)
            {
                if (Math.Abs(_axis.Y) > 0)
                {
                    _currentAcceleration = Util.Approach(_currentAcceleration, _maxAcceleration, 50);
                }
                else
                {
                    _currentAcceleration = 0;
                }
            }

            if (_currentAcceleration > 0)
            {
                _currentSpeed = Util.Approach(_currentSpeed, _maxSpeed, _currentAcceleration);
            }
            else
            {
                _currentSpeed = Util.Approach(_currentSpeed, 0, _drag);
            }
            _speedX = (int)Util.PolarX(_angle, _currentSpeed);
            _speedY = (int) Util.PolarY(_angle, _currentSpeed);


            MoveXY(_speedX, _speedY, Collider);

            if (OnMove != null)
            {
                OnMove();
            }
        }

        public bool Freeze;
        private float Accel;
        private int _speedX;
        private int _speedY;
    }
}