using Otter;
using robot.dad.game.Entities;

namespace robot.dad.game.Scenes
{
    public class MainScene : Scene
    {
        public MainScene(Player player)
        {
            Add(player);
            CameraFocus = player;
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
            Game.SwitchScene(this);
        }
    }
}