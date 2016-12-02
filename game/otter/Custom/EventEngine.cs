using System;
using Simplex;

namespace Otter.Custom
{
    internal class EventEngine
    {
        private Noise _eventNoise;
        private float _scale;

        public EventEngine()
        {
            _eventNoise = new Noise(42);
            _scale = 0.01f;
        }

        internal TileEvent GetEventForTile(int x, int y, TerrainInfo terrainType)
        {
            int noiseValue = (int)_eventNoise.CalcPixel3D(x, y, 0, _scale).ForceRange(100,1);

            if (70 < noiseValue && noiseValue <= 90)
            {
                if (terrainType.TerrainType == TerrainType.Beach || terrainType.TerrainType == TerrainType.ShrubLand)
                {
                    return new TileEvent(); //More thinking required!
                }
            }

            return null; // null means nothing happens here. Deal with it!
        }
    }
}