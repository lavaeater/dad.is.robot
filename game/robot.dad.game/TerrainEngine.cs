using System;
using Simplex;

namespace robot.dad.game
{
    public class TerrainEngine
    {
        private readonly float _terrainScale;
        private readonly float _moistureScale;

        public TerrainEngine(int seed, float terrainScale, float moistureScale)
        {
            _terrainScale = terrainScale;
            _moistureScale = moistureScale;
            Noise.Seed = seed;
        }

        public float ForceRange(float value, float newMin, float newMax)
        {
            float oldMin = 14;
            float oldMax = 241;
            float newValue = ((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
            return newValue;
        }

        public void OtherNoiseTest()
        {
            Noise.Seed = 12;
            int xMax = 80;
            int yMax = 800;
            float scale = 0.01f;
            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    int elevation = (int)ForceRange(Noise.CalcPixel2D(x, y, scale), 0, 100);
                    int moisture = (int)ForceRange(Noise.CalcPixel2D(x, y, 0.1f), 0, 100);
                    TerrainType terrainType = GetTerrainType(elevation, moisture);
                    Console.BackgroundColor = GetTerrainBackColor(terrainType);
                    Console.ForegroundColor = GetTerrainForeColor(terrainType);
                    Console.Write('X');
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }

        public TerrainType GetTerrainTypeForCoord(int x, int y)
        {
            int elevation = (int)ForceRange(Noise.CalcPixel2D(x, y, _terrainScale), 0, 100);
            int moisture = (int)ForceRange(Noise.CalcPixel2D(x, y, _moistureScale), 0, 100);
            TerrainType terrainType = GetTerrainType(elevation, moisture);
            return terrainType;
        }

        private ConsoleColor GetTerrainForeColor(TerrainType terrainType)
        {
            switch (terrainType)
            {
                case TerrainType.Ocean:
                    return ConsoleColor.Blue;
                case TerrainType.Beach:
                    return ConsoleColor.DarkYellow;
                case TerrainType.GrassLand:
                    return ConsoleColor.DarkGreen;
                case TerrainType.Scorched:
                    return ConsoleColor.Gray;
                case TerrainType.ShrubLand:
                    return ConsoleColor.DarkGray;
                case TerrainType.SubTropicalDesert:
                    return ConsoleColor.White;
                case TerrainType.TemperateDesert:
                    return ConsoleColor.DarkYellow;
                case TerrainType.TemperateForest:
                    return ConsoleColor.DarkRed;
                case TerrainType.TemperateRainForest:
                    return ConsoleColor.Magenta;
                case TerrainType.TropicalRainForest:
                    return ConsoleColor.Yellow;
                case TerrainType.TropicalSeasonalForest:
                    return ConsoleColor.DarkRed;
                case TerrainType.Bare:
                    return ConsoleColor.DarkGray;
                case TerrainType.Snow:
                    return ConsoleColor.White;
                case TerrainType.Taiga:
                    return ConsoleColor.DarkYellow;
                case TerrainType.Tundra:
                    return ConsoleColor.DarkGreen;
                default:
                    return ConsoleColor.Black;
            }
        }

        private ConsoleColor GetTerrainBackColor(TerrainType terrainType)
        {
            switch (terrainType)
            {
                case TerrainType.Ocean:
                    return ConsoleColor.DarkBlue;
                case TerrainType.Beach:
                    return ConsoleColor.Yellow;
                case TerrainType.GrassLand:
                    return ConsoleColor.Green;
                case TerrainType.Scorched:
                    return ConsoleColor.DarkYellow;
                case TerrainType.ShrubLand:
                    return ConsoleColor.DarkGreen;
                case TerrainType.SubTropicalDesert:
                    return ConsoleColor.DarkYellow;
                case TerrainType.TemperateDesert:
                    return ConsoleColor.Yellow;
                case TerrainType.TemperateForest:
                    return ConsoleColor.DarkGreen;
                case TerrainType.TemperateRainForest:
                    return ConsoleColor.Green;
                case TerrainType.TropicalRainForest:
                    return ConsoleColor.Green;
                case TerrainType.TropicalSeasonalForest:
                    return ConsoleColor.DarkGreen;
                case TerrainType.Bare:
                    return ConsoleColor.Gray;
                case TerrainType.Snow:
                    return ConsoleColor.White;
                case TerrainType.Taiga:
                    return ConsoleColor.Gray;
                case TerrainType.Tundra:
                    return ConsoleColor.Gray;
                default:
                    return ConsoleColor.Black;
            }
        }

        public TerrainType GetTerrainType(int elevation, int moisture)
        {
            TerrainType terrain = TerrainType.Ocean;
            if (15 < elevation && elevation <= 20)
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
}