using System;

namespace robot.dad.combat
{
    public static class DiceRoller
    {
        private static readonly Random Rnd = new Random(DateTime.Now.Second);

        public static int RollDice(int minVal, int maxVal)
        {
            return Rnd.Next(minVal, maxVal + 1);
        }

        public static int RollHundredSided()
        {
            return RollDice(1, 100);
        }
    }
}