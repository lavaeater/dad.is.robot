using System;
using ca.axoninteractive.Geometry.Hex;
using robot.dad.game.Entities;
using robot.dad.graphics;
using Simplex;

namespace robot.dad.game.World
{
    public class EventEngine
    {
        private readonly Noise _eventNoise;
        private readonly float _scale;
        private readonly Random _rand;
        private int _maxVal;

        public EventEngine()
        {
            _eventNoise = new Noise(42);
            _scale = 0.01f;
            _rand = new Random(32);
            _maxVal = 0;
        }

        public TileEvent GetEventForTile(CubicHexCoord coord, TerrainInfo terrainType)
        {
            int diceRoll = _rand.Next(1, 1001);//
            //int diceRoll = (int)_eventNoise.CalcPixel3D(coord.x, coord.y, 0, _scale).ForceRange(100, 1);
            if (diceRoll > _maxVal)
                _maxVal = diceRoll;
            if (terrainType.TerrainType == TerrainType.TemperateDesert || terrainType.TerrainType == TerrainType.SubTropicalDesert || terrainType.TerrainType == TerrainType.Scorched)
            {
                if (990< diceRoll && diceRoll <= 1000)
                {
                    return new TileEvent("Ruin", coord); //More thinking required!
                }
            }

            return null;
        }
    }
}