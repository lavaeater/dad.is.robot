using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.Event;

namespace robot.dad.game.Scenes
{
    public class MainScene : Scene
    {
        public MainScene(PlayerEntity playerEntity)
        {
            Add(playerEntity);
            CameraFocus = playerEntity;
        }

        public void AddBackGround(HexBackGround background)
        {
            base.Add(background);
            BackGround = background;
        }

        public HexBackGround BackGround { get; set; }

        public void StartChase(TileEvent tileEvent)
        {
        }

        public void ReturnToMain()
        {
            Game.SwitchScene(this);
        }
    }
}