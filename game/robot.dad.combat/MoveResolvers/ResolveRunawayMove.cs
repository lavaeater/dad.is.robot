using System;
using robot.dad.common;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveRunawayMove : ResolveMoveBase
    {
        public override bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            bool result = false;
            int targetValue = attacker.DefenseSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            if (diceRoll <= targetValue)
            {
                attacker.Runaway();
                result = true;
            }
            return result; // man kan t�nka sig ett mer avancerat returobjekt ist�llet f�r bara true / false, f�r se hur vi l�ser det..
        }
    }
}