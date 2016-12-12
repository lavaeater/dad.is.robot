using System;
using robot.dad.combat.EffectAppliers;
using robot.dad.combat.Interfaces;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveAttackMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
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
            //Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //Console.Write($"sl�r {diceRoll}");
                IApplyEffects applier = diceRoll <= perfectRollValue ? new NormalDamageEffectApplier(move.MaxDamage, move.MaxDamage) : new NormalDamageEffectApplier(move.MinDamage, move.MaxDamage);
                applier.ApplyEffects(target);
            }
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                //Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        }
    }
}