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
        }
    }
}