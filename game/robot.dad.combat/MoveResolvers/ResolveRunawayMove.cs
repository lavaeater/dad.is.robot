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
            //Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //vad g�r perfekt slag och hur?
                //Console.WriteLine($"och sl�r {diceRoll} och {move.Verbified}!");
                attacker.Runaway();
            }
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                //Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        }
    }
}