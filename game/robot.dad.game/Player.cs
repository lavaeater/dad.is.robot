using Otter;

namespace robot.dad.game
{
    public class Player : ImageEntity
    {
        public readonly Session Session;
        public readonly Speed Speed;

        public Player(float x, float y, string imagePath, Session session) : base (x, y, imagePath)
        {
            Session = session;
            Speed = new Speed(2);
        }

        public override void Update()
        {
            base.Update();
            Scene.BringToFront(this);
            if (Session.Controller.Button(Controls.Down).Down)
            {
                Speed.Y++;
            }
            if (Session.Controller.Button(Controls.Up).Down)
            {
                Speed.Y--;
            }
            if (Session.Controller.Button(Controls.Right).Down)
            {
                Speed.X++;
            }
            if (Session.Controller.Button(Controls.Left).Down)
            {
                Speed.X--;
            }

            X += Speed.X;
            Y += Speed.Y;
        }
    }
}