using System;
using robot.dad.combat.EffectAppliers;
using robot.dad.common;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveAttackMove : ResolveMoveBase
    {
        public override bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            //Console.WriteLine();
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            if (target.CurrentMove?.MoveType == CombatMoveType.Defend ||
                target.CurrentMove?.MoveType == CombatMoveType.Runaway)
            {
                targetValue += target.CurrentMove.Modifier;
            }
            int perfectRollValue = targetValue / 10;

            int diceRoll = DiceRoller.RollHundredSided();
            bool result = false;
            //Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                result = true;
                //1 == perfekt slag!
                //Console.Write($"sl�r {diceRoll}");
                IApplyEffects applier = diceRoll <= perfectRollValue ? new NormalDamageEffectApplier(move.MaxDamage, move.MaxDamage) : new NormalDamageEffectApplier(move.MinDamage, move.MaxDamage);
                applier.ApplyEffects(target);
            }
            return result;
        }
    }
}