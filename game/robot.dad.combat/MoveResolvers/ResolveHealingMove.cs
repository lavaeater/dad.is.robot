using System;
using robot.dad.combat.EffectAppliers;
using robot.dad.common;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveHealingMove : ResolveMoveBase
    {
        public override bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            //Console.WriteLine();
            bool result = false;
            int targetValue = attacker.AttackSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            //Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //Console.Write($"sl�r {diceRoll}");
                var applier = diceRoll <= 5 ? new HealingEffectApplier(move.MaxDamage, move.MaxDamage) : new HealingEffectApplier(move.MinDamage, move.MaxDamage);
                applier.ApplyEffects(target);
                result = true;
            }
            return result;
        }
    }
}