using System;
using Otter;
using robot.dad.common;
using robot.dad.game.Sprites;

namespace robot.dad.game.Components
{
    public class ThrusterMovement : Movement
    {
        private readonly float _rotationSpeed;
        private float _currentRotationSpeed;
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
        private readonly int _smallThrust;
        private readonly int _mediumThrust;

        public ThrusterMovement(float rotationSpeed, float drag, Axis axis, float angle, float maxThrust, float maxSpeed, bool enableThrustGraphic)
        {
            _rotationSpeed = rotationSpeed;
            _currentRotationSpeed = rotationSpeed;
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
            _mediumThrust = (int)_maxThrust - (int)_maxThrust / 5;
            Wind = new WindFlute();
        }

        public WindFlute Wind { get; set; }

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
                    _currentRotationSpeed = _rotationSpeed + 2;
                    SmallThrust.Visible = true;
                }
                else if (_smallThrust < currThrust && currThrust < _mediumThrust)
                {
                    _currentRotationSpeed = _rotationSpeed;
                    MediumThrust.Visible = true;
                }
                else if (_mediumThrust < currThrust)
                {
                    _currentRotationSpeed = _rotationSpeed - 1;
                    MaxThrust.Visible = true;
                }
                else if (currThrust == 0)
                {
                    if (_currentSpeed < 1)
                    {
                        _currentRotationSpeed = 0;
                    }
                    else
                    {
                        _currentRotationSpeed = _rotationSpeed + 2;
                    }
                    SmallThrust.Visible = false;
                    MediumThrust.Visible = false;
                    MaxThrust.Visible = false;
                }
            }

            _angle = Util.ApproachAngle(_angle, Util.WrapAngle(_angle + (_currentRotationSpeed * -_axis.X)), _currentRotationSpeed);

            Entity.Graphics.ForEach(g => g.Angle = _angle + 90f);

            if (_currentThrust > 0)
            {
                _currentSpeed = Util.Approach(_currentSpeed, _maxSpeed, _currentThrust);
            }
            else
            {
                _currentSpeed = Util.Approach(_currentSpeed, 0, _drag);
            }

            Wind.Update();

            _speedX = (int)Util.PolarX(_angle, _currentSpeed) + Wind.WindForceX;
            _speedY = (int)Util.PolarY(_angle, _currentSpeed) + Wind.WindForceY;


            MoveXY(_speedX, _speedY, Collider);

            OnMove?.Invoke();
        }

        private readonly Graphic SmallThrust = SpritePipe.ThrustOne;
        private readonly Graphic MediumThrust = SpritePipe.ThrustTwo;
        private readonly Graphic MaxThrust = SpritePipe.ThrustThree;


        public override void Added()
        {
            if (_enableThrustGraphic)
            {

                float originY = SmallThrust.ScaledHeight + Entity.Graphic.ScaledHeight / 2;
                SmallThrust.SetOrigin(SmallThrust.HalfWidth / 2 + 1, originY);
                MediumThrust.SetOrigin(MediumThrust.HalfWidth / 2 + 1, originY);
                MaxThrust.SetOrigin(MaxThrust.HalfWidth / 2 + 1, originY);

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

    public class WindFlute
    {
        public float WindAngle = 180f;
        public float WindSpeed = 1.00f;
        public int WindForceState = 1; // -1 = decreasing wind, 1 = increasing wind, 0 = constant
        public int WindAngleState = 0;
        public int WindForceX => (int)Util.PolarX(WindAngle, WindSpeed);
        public int WindForceY => (int)Util.PolarY(WindAngle, WindSpeed);
        public int Updates = 0;

        public void Update()
        {
            Updates++;
            if (Updates > 500)
            {
                /*
             * Called from the thruster movement components' update method
             * 
             * will determine if the direction and force of the wind is changing, up or down, 
             * left and right.
             */
                int windForceRoll = DiceRoller.RollDice(0, 100);
                if (windForceRoll > 70)
                {
                    WindForceState = 1;
                }
                else if (windForceRoll < 70 && windForceRoll > 20)
                {
                    WindForceState = 0;
                }
                else if (windForceRoll < 20)
                {
                    WindForceState = -1;
                }

                int windAngleRoll = DiceRoller.RollDice(1, 100);
                if (20 < windAngleRoll && windAngleRoll < 80)
                {
                    WindAngleState = 0;
                }
                else if (windAngleRoll < 20)
                {
                    WindAngleState = -1;
                }
                else if (windAngleRoll > 80)
                {
                    WindAngleState = 1;
                }
                Updates = 0;
            }
            if (Updates % 50 == 0)
            {
                WindSpeed += WindForceState * 20f;
                if (WindSpeed < 0)
                {
                    WindSpeed = 0;
                }
                if (WindSpeed > 250)
                {
                    WindSpeed = 250;
                }
                WindAngle += WindAngleState * 10f;
                if (WindAngle > 360)
                {
                    WindAngle = 0;
                }
                if (WindAngle < 0)
                {
                    WindAngle = 360;
                }
            }
        }
    }
}