using ca.axoninteractive.Geometry.Hex;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.SceneManager;
using robot.dad.graphics;

namespace robot.dad.game.Scenes
{
    public class MainScene : Scene
    {
        public void MovePlayerToHex(CubicHexCoord coord)
        {
            var pos = Hex.Grid.CubicToPoint(coord);
            Player.Position = new Vector2(pos.x, pos.y);
        }

        public MainScene(PlayerEntity playerEntity)
        {
            Player = playerEntity;
            Add(playerEntity);
            CameraFocus = playerEntity;
        }

        public PlayerEntity Player { get; set; }

        public void AddBackGround(HexBackGround background)
        {
            base.Add(background);
            BackGround = background;
        }

        public HexBackGround BackGround { get; set; }

        public void ReturnToMain()
        {
            Game.SwitchScene(this);
        }

        public override void Update()
        {
            if (Input.KeyReleased(Key.I))
            {
                Manager.Instance.GotoInventory();
            }
        }
    }
}