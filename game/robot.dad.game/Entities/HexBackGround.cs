using System.Collections.Generic;
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
        public Dictionary<CubicHexCoord, List<IEvent>> MapEvents { get; set; } = new Dictionary<CubicHexCoord, List<IEvent>>();

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
                new TerrainEngine(568, 0.01f, 0.01f, terrainData));

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

            UpdateTileEvents();
        }

        private void UpdateTileEvents()
        {
            var visibleMapTileCoords = _hexMap.VisibleTiles.Select(t => t.HexCoord).ToList();

            var notVisibleCoords = MapEvents.Keys.Except(visibleMapTileCoords);
            foreach (CubicHexCoord notVisibleMapEntity in notVisibleCoords)
            {
                MapEvents[notVisibleMapEntity].ForEach(e => e.Hide());
            }

            foreach (var missingTileCoord in visibleMapTileCoords)
            {
                if (!MapEvents.ContainsKey(missingTileCoord))
                {
                    var mapEvent = _eventEngine.GetEventForTile(missingTileCoord,
                      _hexMap.Hexes[missingTileCoord].TerrainInfo);
                    AddEventForCoord(missingTileCoord, mapEvent);
                }
            }

            foreach (CubicHexCoord coord in visibleMapTileCoords)
            {
                MapEvents[coord].ForEach(e => e.Show()); //Makes the entity visible if relevant
            }

            var identifiCationRangeCoords = CurrentPosition.AreaAround(3);

            foreach (CubicHexCoord coord in MapEvents.Keys.Intersect(identifiCationRangeCoords))
            {
                MapEvents[coord].ForEach(e =>
                {
                    e.Identify();
                });
            }
        }

        private void AddEventForCoord(CubicHexCoord hex, IEvent mapEvent)
        {
            if (mapEvent != null)
            {
                MapEvents.Add(hex, new List<IEvent> { mapEvent });
                Scene.Add(mapEvent.TileEntity);
            }
            else
            {
                //We only add mapevents ONCE for every tile, to begin with
                MapEvents.Add(hex, new List<IEvent>()); //This gives an empty list too loop over, which makes everything better
            }

        }

        public void AddEvents(List<IEvent> events)
        {
            foreach (var tileEvent in events)
            {
                AddEventForCoord(tileEvent.Hex, tileEvent);
            }
        }
    }
}