using Otter;

namespace robot.dad.game
{
    public class MainScene : Scene
    {
        private readonly Player _player;

        public MainScene(Player player)
        {
            _player = player;
            Add(player);
            CameraFocus= _player;
        }
        
        public void AddBackGround(HexBackGround background)
        {
            base.Add(background);
            BackGround = background;
        }

        public HexBackGround BackGround { get; set; }

        public void StartChase()
        {
            Game.AddScene(new ChaseScene());
        }
    }

    public class ChaseScene : Scene
    {
        private Player _player;

        public ChaseScene()
        {
            _player = new Player(Center.X, Center.Y, Global.PlayerOne);
            Add(_player);
            CameraFocus = _player;
            BackGroundColor = new Color(0.85f, 0.85f);
        }
    }
}