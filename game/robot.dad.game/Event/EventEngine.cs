using System;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using robot.dad.game.Entities;
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

        public TileEvent GetEventForTile(CubicHexCoord coord, TerrainInfo terrainType)
        {
            var table = new TileEventTable();
            //Add code for managing terrainbased-probabilities
            //If event occurs, then what?
            if (table.Result.OfType<RuinEventTable>().Any())
            {
                return new TileEvent("Ruin", coord);
            }

            //int diceRoll = _rand.Next(1, 1001);//
            ////int diceRoll = (int)_eventNoise.CalcPixel3D(coord.x, coord.y, 0, _scale).ForceRange(100, 1);
            //if (diceRoll > _maxVal)
            //    _maxVal = diceRoll;
            //if (terrainType.TerrainType == TerrainType.TemperateDesert || terrainType.TerrainType == TerrainType.SubTropicalDesert || terrainType.TerrainType == TerrainType.Scorched)
            //{
            //    if (990< diceRoll && diceRoll <= 1000)
            //    {
            //        return new TileEvent("Ruin", coord); //More thinking required!
            //    }
            //}

            return null;
        }
    }
}