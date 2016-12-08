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
            CameraFocus = _player;
        }

        public void AddBackGround(HexBackGround background)
        {
            base.Add(background);
            BackGround = background;
        }

        public HexBackGround BackGround { get; set; }

        public void StartChase()
        {
            Game.SwitchScene(new ChaseScene(ReturnToMe));
        }

        public void ReturnToMe()
        {
            //Do we still have our data?
            
            Game.SwitchScene(this);
        }
    }
}