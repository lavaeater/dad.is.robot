using Otter;
using Otter.Custom;

namespace robot.dad.game
{
    public class HexBackGround : Entity
    {
        private HexTileMap _hexMap;

        public override void Added()
        {
            base.Added();
            CreateInitialVisibleMap();
        }

        private Rectangle CalculateMapBounds()
        {
            int left = (int)(Scene.CameraX - Scene.CameraWidth / 2);
            int top = (int)(Scene.CameraY - Scene.CameraHeight/ 2);
            return new Rectangle(left, top, (int)Scene.CameraWidth * 2, (int)Scene.CameraHeight * 2);
        }

        private void CreateInitialVisibleMap()
        {
//            var bounds = CalculateMapBounds();
//            _hexMap.CreateHexes(bounds);
            _hexMap.CreateHexes(Scene.CameraCenterX, Scene.CameraCenterY);
        }

        public HexBackGround(string atlasFile)
        {
            _hexMap = new HexTileMap(69, new HexAtlas(atlasFile), new TerrainEngine(12, 0.01f, 0.1f));

            Graphic = _hexMap;
        }

        public override void Update()
        {
            base.Update();
            //Only update map, sometimes, not all the time
            int left = Scene.CameraBounds.Left;
            int top = Scene.CameraBounds.Top;


        }
    }
}