using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplex;

namespace Otter.Custom
{
    public class TerrainEngine
    {
        private readonly Noise _terrainNoise;
        private readonly Noise _moistureNoise;
        private readonly float _terrainScale;
        private readonly float _moistureScale;

        public TerrainEngine(int seed, float terrainScale, float moistureScale)
        {
            _terrainScale = terrainScale;
            _moistureScale = moistureScale;
            _terrainNoise = new Noise(seed);
            _moistureNoise = new Noise();
        }

        public TerrainInfo GetTerrainTypeForCoord(int x, int y)
        {
            int elevation = (int)_terrainNoise.CalcPixel3D(x, y, 0, _terrainScale).ForceRange(100, 1);
            int moisture = (int)_moistureNoise.CalcPixel3D(x, y, 0, _moistureScale).ForceRange(100, 1);
            TerrainType terrainType = GetTerrainType(elevation, moisture);
            return new TerrainInfo(terrainType, elevation, moisture);
        }

        public TerrainType GetTerrainType(int elevation, int moisture)
        {
            TerrainType terrain = TerrainType.Ocean;
            if (17 < elevation && elevation <= 20)
            {
                terrain = TerrainType.Beach;
            }
            if (20 < elevation && elevation <= 40)
            {
                terrain = TerrainType.TropicalRainForest;
                if (0 < moisture && moisture <= 16)
                    terrain = TerrainType.SubTropicalDesert;
                if (16 < moisture && moisture <= 33)
                    terrain = TerrainType.GrassLand;
                if (33 < moisture && moisture <= 66)
                    terrain = TerrainType.TropicalSeasonalForest;
            }
            if (40 < elevation && elevation <= 75)
            {
                terrain = TerrainType.TemperateRainForest;
                if (0 < moisture && moisture <= 16)
                    terrain = TerrainType.TemperateDesert;
                if (16 < moisture && moisture <= 50)
                    terrain = TerrainType.GrassLand;
                if (50 < moisture && moisture <= 83)
                    terrain = TerrainType.TemperateForest;
            }
            if (75 < elevation && elevation <= 90)
            {
                terrain = TerrainType.Taiga;
                if (0 < moisture && moisture <= 33)
                    terrain = TerrainType.TemperateDesert;
                if (33 < moisture && moisture <= 66)
                    terrain = TerrainType.ShrubLand;
            }
            if (90 < elevation && elevation <= 100)
            {
                terrain = TerrainType.Snow;
                if (0 < moisture && moisture <= 10)
                    terrain = TerrainType.Scorched;
                if (10 < moisture && moisture <= 20)
                    terrain = TerrainType.Bare;
                if (20 < moisture && moisture <= 50)
                    terrain = TerrainType.Tundra;
            }
            return terrain;
        }
    }

    public struct TerrainInfo
    {
        public TerrainInfo(TerrainType terrainType, float elevation, float moisture)
        {
            TerrainType = terrainType;
            Elevation = elevation;
            Moisture = moisture;
        }
        public TerrainType TerrainType;
        public float Elevation;
        public float Moisture;
    }

    public static class RangeForcer
    {
        public static float ForceRange(this float value, float newMax, float newMin)
        {
            float oldMin = 14;
            float oldMax = 241;
            float newValue = ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
            return newValue;
        }
    }
}
