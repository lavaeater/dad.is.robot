using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using robot.dad.combat;
using robot.dad.combat.MoveResolvers;
using Simplex;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            //var demo = new CombatDemo();
            //demo.StartGame();

            var noise = new NoiseTest();
            noise.OtherNoiseTest();
            Console.ReadKey();
        }
    }

    public class OtterTest
    {
        
    }

    public enum TerrainType
    {
        Ocean,
        Beach,
        Scorched,
        Bare,
        Tundra,
        Snow,
        TemperateDesert,
        ShrubLand,
        Taiga,
        GrassLand,
        TemperateForest,
        TemperateRainForest,
        SubTropicalDesert,
        TropicalSeasonalForest,
        TropicalRainForest
    }

    public class NoiseTest
    {
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

    public class CombatDemo
    {

        List<Combattant> _participants = new List<Combattant>
            {
                new Human("Tommie", "nygren", MovePickers.RandomPicker),
                new Human("Lisa", "nygren", MovePickers.RandomPicker),
                new Human("Freja", "nygren", MovePickers.RandomPicker),
                new Human("Anja", "nygren", MovePickers.RandomPicker, new List<CombatMove>()
                {
                    new CombatMove("Läka sår", CombatMoveType.Healing, 10, 5, 15, "helar", Resolvers.HealingResolver)
                }),
                new Monster("Snarfor", 30, 90, 10, 5, "nygren", new List<CombatMove>()
                {
                    new CombatMove("Vattenförmåga", CombatMoveType.Attack, 0, 5, 10, "vattenspruta", Resolvers.AttackResolver)
                }, MovePickers.RandomPicker),
                new Monster("Gargelbarg", 200, 60, 0, 5, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("GargelBett", CombatMoveType.Attack, 15, 12, 25, "gargelbita", Resolvers.AttackResolver)
                }, MovePickers.RandomPicker),
                new Monster("Fyrkantsmonster", 100, 40, 30, 10, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("Hypno", CombatMoveType.Special, -10, "hypnotisera", Resolvers.HypnosisResolver)
                }, MovePickers.RandomPicker)
            };

        public void StartGame()
        {
            var ce = new CombatEngine(_participants);
            ce.StartCombat();
            Console.ReadKey();
        }
    }
}
