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
            return result; // man kan tänka sig ett mer avancerat returobjekt istället för bara true / false, får se hur vi löser det..
        }
    }
}