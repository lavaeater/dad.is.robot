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
        public Dictionary<CubicHexCoord, List<TileEvent>> MapEntities;

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
            => Hex.Grid.PointToCubic(new Vec2D(Scene.CameraCenterX, Scene.CameraCenterY));

        public HexBackGround(string atlasFile, string terrainFile, int boundRadius, int viewPortRadius)
        {
            _boundRadius = boundRadius;
            _viewPortRadius = viewPortRadius;
            _eventEngine = new EventEngine();
            _hexMap = new HexTileMap(_viewPortRadius, 1f, new HexAtlas(atlasFile),
                new TerrainEngine(12, 0.05f, 0.07f, terrainFile));

            MapEntities = new Dictionary<CubicHexCoord, List<TileEvent>>();
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
            UpdateMapEntities();
        }

        private void UpdateMapEntities()
        {
            var visibleMapTileCoords = _hexMap.VisibleTiles.Select(t => t.HexCoord).ToList();

            var notVisibleCoords = MapEntities.Keys.Except(visibleMapTileCoords);
            foreach (CubicHexCoord notVisibleMapEntity in notVisibleCoords)
            {
                MapEntities[notVisibleMapEntity].ForEach(e => e.Visible = false);
            }

            foreach (var missingTileCoord in visibleMapTileCoords)
            {
                if(!MapEntities.ContainsKey(missingTileCoord))
                { var mapEvent = _eventEngine.GetEventForTile(missingTileCoord,
                    _hexMap.Hexes[missingTileCoord].TerrainInfo);

                    if (mapEvent != null)
                    {
                        MapEntities.Add(missingTileCoord, new List<TileEvent> { mapEvent });
                        Scene.Add(mapEvent);
                    }
                    else
                    {
                        //We only add mapevents ONCE for every tile, to begin with
                        MapEntities.Add(missingTileCoord, new List<TileEvent>()); //This gives an empty list too loop over, which makes everything better
                    }
                }
            }

            var identifiCationRangeCoords = CurrentPosition.AreaAround(3);

            foreach (CubicHexCoord coord in MapEntities.Keys.Intersect(visibleMapTileCoords))
            {
                bool identified = identifiCationRangeCoords.Contains(coord);
                MapEntities[coord].ForEach(e =>
                {
                    e.Visible = true;
                    if(identified)
                        e.Identify();
                });
            }
        }

        public void SaveMap(string fileName)
        {
            string json = _hexMap.ToJson();
            File.WriteAllText(fileName, json);

        }
    }
}