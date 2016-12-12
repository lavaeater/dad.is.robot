using System;
using robot.dad.combat.EffectAppliers;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveHealingMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            //Console.WriteLine();

            int targetValue = attacker.AttackSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            //Console.Write($"{attacker.Name} måste slå under {targetValue} för att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //Console.Write($"slår {diceRoll}");
                var applier = diceRoll <= 5 ? new HealingEffectApplier(move.MaxDamage, move.MaxDamage) : new HealingEffectApplier(move.MinDamage, move.MaxDamage);
                applier.ApplyEffects(target);
            }
            else
            {
                //100 == perfekt fail! Vad händer? Nåt kul!
                //Console.WriteLine($"men slår {diceRoll} och missar!");
            }
        }
    }
}