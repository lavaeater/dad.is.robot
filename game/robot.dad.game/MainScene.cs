using Otter;

namespace robot.dad.game
{
    public class MainScene : Scene
    {
        private readonly Session _player;
        private Speed _speed;

        public MainScene(Session player)
        {
            _player = player;
            _speed = new Speed(5);
            ApplyCamera = true;
        }

        public override void Update()
        {
            base.Update();
            if (_player.Controller.Button(Controls.Down).Down)
            {
                _speed.Y++;
            }
            if (_player.Controller.Button(Controls.Up).Down)
            {
                _speed.Y--;
            }
            if (_player.Controller.Button(Controls.Right).Down)
            {
                _speed.X++;
            }
            if (_player.Controller.Button(Controls.Left).Down)
            {
                _speed.X--;
                string something = "debug break;";
            }
            CameraX += _speed.X;
            CameraY += _speed.Y;
        }
    }
}