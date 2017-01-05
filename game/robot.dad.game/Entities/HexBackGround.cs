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
        public HashSet<TileEventInfo> Events;

        public override void Added()
        {
            base.Added();
            CreateInitialVisibleMap();
        }

        public void CreateInitialVisibleMap()
        {
            _hexMap.CreateInitialHexes(Scene.CameraCenterX, Scene.CameraCenterY);

            _previousPosition = CurrentPosition;
            _previousArea = _previousPosition.AreaAround(_boundRadius);
        }

        public void AddEvents(IEnumerable<TileEventInfo> eventsToAdd)
        {
            foreach (var eventInfo in eventsToAdd)
            {
                Events.Add(eventInfo);
            }
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

            Events = new HashSet<TileEventInfo>();
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

            var notVisibleCoords = Events.Select(e => e.Hex).Except(visibleMapTileCoords);
            foreach (CubicHexCoord notVisibleCoord in notVisibleCoords)
            {

                Events.Single(e => e.Hex == notVisibleCoord).Events.ForEach(e =>
                {
                    e.Entity.Visible = false;
                    e.ShouldUpdate = false;
                });
            }

            foreach (var missingTileCoord in visibleMapTileCoords)
            {
                if(Events.All(e => e.Hex != missingTileCoord))
                { var mapEvent = _eventEngine.GetEventForTile(missingTileCoord,
                    _hexMap.Hexes[missingTileCoord].TerrainInfo);

                    if (mapEvent != null)
                    {
                        Events.Add(new TileEventInfo(missingTileCoord) {
                            Events = new List<TileEvent> { mapEvent }
                            });
                        Scene.Add(mapEvent.Entity);
                    }
                    else
                    {
                        //We only add mapevents ONCE for every tile, to begin with
                        Events.Add(new TileEventInfo(missingTileCoord)
                        {
                          Events  = new List<TileEvent>()
                        }); //This gives an empty list too loop over, which makes everything better
                    }
                }
            }

            var identifiCationRangeCoords = CurrentPosition.AreaAround(3);

            foreach (CubicHexCoord coord in Events.Select(e => e.Hex).Intersect(visibleMapTileCoords))
            {
                bool identified = identifiCationRangeCoords.Contains(coord);
                Events.Single(e => e.Hex ==coord).Events.ForEach(e =>
                {
                    if (e is ScavengerEvent)
                    {
                        e.ShouldUpdate = true;
                    }
                    else
                    {
                        e.ShouldUpdate = true;
                        e.Entity.Visible = true;
                    }
                    if(identified)
                        e.Identify();
                });
            }
        }
    }
}