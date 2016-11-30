using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using ca.axoninteractive.Geometry.Hex;
using Otter;
using Otter.Custom;

namespace robot.dad.game
{
    public class HexBackGround : Entity
    {
        private readonly int _boundRadius;
        private readonly int _viewPortRadius;
        private HexTileMap _hexMap;
        private CubicHexCoord _previousPosition;
        private CubicHexCoord[] _previousArea;

        public override void Added()
        {
            base.Added();
            CreateInitialVisibleMap();
        }

        private void CreateInitialVisibleMap()
        {
            _hexMap.CreateInitialHexes(Scene.CameraCenterX, Scene.CameraCenterY);

            _previousPosition = CurrentPosition;
            _previousArea = _previousPosition.AreaAround(_boundRadius);
        }

        private CubicHexCoord CurrentPosition
            => _hexMap.HexGrid.PointToCubic(new Vec2D(Scene.CameraCenterX, Scene.CameraCenterY));

        public HexBackGround(string atlasFile, float hexRadius, int boundRadius, int viewPortRadius)
        {
            _boundRadius = boundRadius;
            _viewPortRadius = viewPortRadius;
            _hexMap = new HexTileMap(hexRadius, _viewPortRadius, 1f, new HexAtlas(atlasFile), new TerrainEngine(12, 0.01f, 0.1f));

            Graphic = _hexMap;
        }

        public override void Update()
        {
            base.Update();

            //Check if camera has left some bounded area
            if (!_previousArea.Contains(CurrentPosition))
            {
                _hexMap.UpdateVisibleTiles(CurrentPosition);
                _previousPosition = CurrentPosition;
                _previousArea = _previousPosition.AreaAround(_boundRadius);
            }

            //If true, tell hextilemap to get new list of visible tiles - and make sure they exist...

            //Only update map, sometimes, not all the time
            int left = Scene.CameraBounds.Left;
            int top = Scene.CameraBounds.Top;


        }

        public void SaveMap(string fileName)
        {
            string json = _hexMap.ToJson();
            File.WriteAllText(fileName, json);

        }
    }
}