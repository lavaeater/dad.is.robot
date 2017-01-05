using System.Collections.Generic;
using System.IO;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using Otter;
using robot.dad.game.Event;
using robot.dad.graphics;

namespace robot.dad.game.Entities
{
    public class HexBackGround : Entity
    {
        private readonly int _boundRadius;
        private readonly HexTileMap _hexMap;
        private CubicHexCoord _previousPosition;
        private CubicHexCoord[] _previousArea;
        private readonly EventEngine _eventEngine;
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

        public CubicHexCoord CurrentPosition
            => Hex.Grid.PointToCubic(new Vec2D(Scene.CameraCenterX, Scene.CameraCenterY));

        public HexBackGround(string atlasFile, string terrainData, int boundRadius, int viewPortRadius)
        {
            _boundRadius = boundRadius;
            _eventEngine = new EventEngine();
            _hexMap = new HexTileMap(viewPortRadius, 1f, new HexAtlas(atlasFile),
//                new TerrainEngine(12, 0.05f, 0.07f, terrainData));
                new TerrainEngine(568, 0.01f, 0.01f, terrainData));

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
                MapEntities[notVisibleMapEntity].ForEach(e => e.Entity.Visible = false);
            }

            foreach (var missingTileCoord in visibleMapTileCoords)
            {
                if(!MapEntities.ContainsKey(missingTileCoord))
                { var mapEvent = _eventEngine.GetEventForTile(missingTileCoord,
                    _hexMap.Hexes[missingTileCoord].TerrainInfo);
                    AddEventForCoord(missingTileCoord, mapEvent);
                }
            }

            var identifiCationRangeCoords = CurrentPosition.AreaAround(3);

            foreach (CubicHexCoord coord in MapEntities.Keys.Intersect(visibleMapTileCoords))
            {
                bool identified = identifiCationRangeCoords.Contains(coord);
                MapEntities[coord].ForEach(e =>
                {
                    if (e is ScavengerEvent)
                    {
                        e.ShouldUpdate = true;
                        e.Entity.Visible = false;
                    } else
                    {
                        e.ShouldUpdate = true;
                        e.Entity.Visible = true;
                    }
                    if (identified)
                        e.Identify();
                });
            }
        }

        public void SaveMap(string fileName)
        {
            string json = _hexMap.ToJson();
            File.WriteAllText(fileName, json);
        }

        public void AddEventForCoord(CubicHexCoord hex, TileEvent mapEvent)
        {
            if (mapEvent != null)
            {
                MapEntities.Add(hex, new List<TileEvent> { mapEvent });
                Scene.Add(mapEvent.Entity);
            }
            else
            {
                //We only add mapevents ONCE for every tile, to begin with
                MapEntities.Add(hex, new List<TileEvent>()); //This gives an empty list too loop over, which makes everything better
            }
        }

        public void AddEvents(List<TileEvent> events)
        {
            foreach (var tileEvent in events)
            {
                AddEventForCoord(tileEvent.Hex, tileEvent);
            }
        }
    }
}