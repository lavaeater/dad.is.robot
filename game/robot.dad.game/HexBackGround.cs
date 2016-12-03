using System.Collections.Generic;
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
        private EventEngine _eventEngine;
        public Dictionary<CubicHexCoord, List<Entity>> MapEntities;

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
            _eventEngine = new EventEngine();
            _hexMap = new HexTileMap(hexRadius, _viewPortRadius, 1f, new HexAtlas(atlasFile),
                new TerrainEngine(12, 0.05f, 0.07f));

            MapEntities = new Dictionary<CubicHexCoord, List<Entity>>();
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
                UpdateMapEntities();
            }

            //If true, tell hextilemap to get new list of visible tiles - and make sure they exist...

            //Only update map, sometimes, not all the time
            int left = Scene.CameraBounds.Left;
            int top = Scene.CameraBounds.Top;
        }

        private void UpdateMapEntities()
        {
            var visibleMapTileCoords = _hexMap.VisibleTiles.Select(t => t.HexCoord).ToList();
            var notVisibleCoords = MapEntities.Keys.Except(visibleMapTileCoords);
            foreach (CubicHexCoord notVisibleMapEntity in notVisibleCoords)
            {
                MapEntities[notVisibleMapEntity].ForEach(e => e.Visible = false);
            }

            var missingTileCoords = visibleMapTileCoords.Except(MapEntities.Keys);
            foreach (var missingTileCoord in missingTileCoords)
            {
                var mapEvent = _eventEngine.GetEventForTile(missingTileCoord.x, missingTileCoord.y,
                    _hexMap.Hexes[missingTileCoord].TerrainInfo);
                if (mapEvent != default(TileEvent))
                {
                    MapEntities.Add(missingTileCoord, new List<Entity> {mapEvent});
                    Scene.Add(mapEvent);
                }
            }
            foreach (CubicHexCoord coord in MapEntities.Keys.Intersect(visibleMapTileCoords))
            {
                MapEntities[coord].ForEach(e => e.Visible = true);
            }
        }

        public void SaveMap(string fileName)
        {
            string json = _hexMap.ToJson();
            File.WriteAllText(fileName, json);

        }
    }
}