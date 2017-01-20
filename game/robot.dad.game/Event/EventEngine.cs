using System;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using rds;
using robot.dad.graphics;
using Simplex;

namespace robot.dad.game.Event
{
    public class EventEngine
    {
        private readonly Noise _eventNoise;
        private readonly Random _rand;

        public EventEngine()
        {
            _eventNoise = new Noise(42);
            _rand = new Random(32);
        }
        
        public IEvent GetEventForTile(CubicHexCoord coord, TerrainInfo terrainInfo)
        {
            var table = new TileEventTable();
            //Add code for managing terrainbased-probabilities
            //If event occurs, then what?
            if (table.Result.OfType<ThingValue<EventType>>().Any())
            {
                var result = table.Result.OfType<ThingValue<EventType>>().FirstOrDefault()?.Value;
                switch (result)
                {
                    case EventType.Ruin:
                        return new RuinEvent(coord);
                    case EventType.Scavenger:
                        return new ScavengerEvent(coord);
                    case EventType.Settlement:
                        return new SettlementEvent(coord);
                }
            }
            return null;
        }
    }
}