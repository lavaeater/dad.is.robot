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
            noise.TestNoise();
            Console.ReadKey();
        }
    }

    public class NoiseTest
    {
        public void TestNoise()
        {
            int maxX = 80, maxY = 80;
            float[,] noiseMap = Simplex.Noise.Calc2D(maxX, maxY, 0.05f);
            int maxNoise = 0;
            int minNoise = 255;
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    int noiseVal = (int) noiseMap[x, y];
                    if (noiseVal > maxNoise)
                        maxNoise = noiseVal;

                    if (noiseVal < minNoise)
                        minNoise = noiseVal;
                    char terrain = GetTerrainType(noiseVal);
                    Console.BackgroundColor = GetTerrainColor(terrain);
                    Console.Write(terrain);

                }
                Console.WriteLine();
            }
            Console.WriteLine(maxNoise);
            Console.WriteLine(minNoise);
        }

        public ConsoleColor GetTerrainColor(char terrain)
        {
            ConsoleColor returnColor = ConsoleColor.Blue;
            switch (terrain)
            {
                case 'b':
                    returnColor = ConsoleColor.Yellow;
                    break;
                case 'g':
                    returnColor = ConsoleColor.Green;
                    break;
                case 'f':
                    returnColor = ConsoleColor.DarkGreen;
                    break;
                case 'j':
                    returnColor = ConsoleColor.DarkMagenta;
                    break;
                case 'd':
                    returnColor = ConsoleColor.DarkYellow;
                    break;
                case 'm':
                    returnColor = ConsoleColor.Gray;
                    break;
                case 'w':
                    returnColor = ConsoleColor.Blue;
                    break;
            }
            return returnColor;
        }



        public char GetTerrainType(int noise)
        {
            char terrain = 'w';
            if (75 < noise && noise <= 100)
            {
                terrain = 'b';
            }
            if (100 < noise && noise <= 175)
            {
                terrain = 'g';
            }
            if (175 < noise && noise <= 200)
            {
                terrain = 'f';
            }
            if (200 < noise && noise <= 215)
            {
                terrain = 'j';
            }
            if (215 < noise && noise <= 225)
            {
                terrain = 'd';
            }
            if (225 < noise && noise <= 255)
            {
                terrain = 'm';
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
