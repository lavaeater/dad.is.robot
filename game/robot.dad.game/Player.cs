using System;
using Otter;

namespace robot.dad.game
{
    public class Player : ImageEntity
    {
        public readonly Session Session;
        public readonly Speed Speed;
        private Axis _axis;
        private ThrusterMovement _movement;

        public Player(float x, float y, string imagePath, Session session) : base (x, y, imagePath)
        {
            //hmmm
            _axis = Axis.CreateArrowKeys();
            _movement = new ThrusterMovement(500, 5, 100, 50, _axis);
            AddComponents(_axis, _movement);

            Session = session;
            Speed = new Speed(2);
        }

        public override void Update()
        {
            base.Update();
            Scene.BringToFront(this);
            //if (Session.Controller.Button(Controls.Down).Down)
            //{
            //    Speed.Y++;
            //}
            //if (Session.Controller.Button(Controls.Up).Down)
            //{
            //    Speed.Y--;
            //}
            //if (Session.Controller.Button(Controls.Right).Down)
            //{
            //    Speed.X++;
            //}
            //if (Session.Controller.Button(Controls.Left).Down)
            //{
            //    Speed.X--;
            //}

            //X += Speed.X;
            //Y += Speed.Y;
        }
    }

    public class ThrusterMovement : Movement
    {
        private readonly float MaxSpeed;
        private readonly Speed TargetSpeed;
        private readonly float _rotationSpeed;
        private readonly float _drag;
        private readonly float _roation;
        private readonly Speed Speed;
        private Axis Axis;
        private float Angle;

        public ThrusterMovement(float maxSpeed, float rotationSpeed, float drag, float acceleration, Axis axis)
        {
            Angle = 180;
            Speed = new Speed(maxSpeed);
            TargetSpeed = new Speed(maxSpeed);
            _rotationSpeed = rotationSpeed; //not important right now
            _drag = drag;
            Axis = axis;
            Accel = acceleration;
        }


        public override void Update()
        {
            if (Freeze) return;

            Angle = Util.ApproachAngle(Angle, Util.WrapAngle(Angle + (_rotationSpeed * -Axis.X)),  5f);
            Entity.Graphic.Angle = Angle - 90f;

            TargetSpeed.MaxX = Speed.MaxX;
            TargetSpeed.MaxY = Speed.MaxX;

            if (Axis != null)
            {
                TargetSpeed.X = Util.PolarX(Angle, Axis.Y * TargetSpeed.MaxX);
                TargetSpeed.Y = Util.PolarY(Angle, Axis.Y * TargetSpeed.MaxY);
            }

            Speed.X = Util.Approach(Speed.X, TargetSpeed.X, Accel);
            Speed.Y = Util.Approach(Speed.Y, TargetSpeed.Y, Accel);

            MoveXY((int)Speed.X, (int)Speed.Y, Collider);

            if (OnMove != null)
            {
                OnMove();
            }
        }

        public bool Freeze;
        private float Accel;
    }

    public static class NumericExtensions
    {
        public static double ToRadians(this float val)
        {
            return (Math.PI / 180) * val;
        }
    }
}