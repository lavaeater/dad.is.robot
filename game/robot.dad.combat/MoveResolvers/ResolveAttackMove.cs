using System;
using robot.dad.combat.EffectAppliers;
using robot.dad.common;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveAttackMove : ResolveMoveBase
    {
        public override bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            int targetValue = attacker.CurrentAttack + move.Modifier - target.CurrentDefense;
            if (target.CurrentMove?.MoveType == CombatMoveType.Defend ||
                target.CurrentMove?.MoveType == CombatMoveType.Runaway)
            {
                targetValue += target.CurrentMove.Modifier;
            }
            int perfectRollValue = targetValue / 10;

            int diceRoll = DiceRoller.RollHundredSided();
            bool result = false;
            if (diceRoll <= targetValue)
            {
                result = true;
                //1 == perfekt slag!

                //Effect applier comes from the attack, not from here, right?

                IApplyEffects applier = diceRoll <= perfectRollValue ? new NormalDamageEffectApplier(move.MaxDamage, move.MaxDamage) : new NormalDamageEffectApplier(move.MinDamage, move.MaxDamage);
                applier.ApplyEffects(target);
            }
            return result;
        }
    }
}