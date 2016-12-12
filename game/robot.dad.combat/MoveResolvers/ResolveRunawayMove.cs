using System;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveRunawayMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            //Console.WriteLine();

            int targetValue = attacker.DefenseSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            //Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //vad gör perfekt slag och hur?
                //Console.WriteLine($"och slår {diceRoll} och {move.Verbified}!");
                attacker.Runaway();
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                //Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        }
    }
}