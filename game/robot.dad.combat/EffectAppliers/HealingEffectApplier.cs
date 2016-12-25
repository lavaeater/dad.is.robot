using System;
using robot.dad.common;

namespace robot.dad.combat.EffectAppliers
{
    public class HealingEffectApplier : ApplyEffectsBase
    {
        public HealingEffectApplier(int min, int max) : base(min, max, 0, EffectType.Immediate)
        {
            EffectName = "Hela";
        }

        public override void ApplyEffects(ICombattant target)
        {
            int damageRoll = DiceRoller.RollDice(Min, Max);
            target.CurrentHealth += damageRoll;
            if (target.CurrentHealth > target.Health) target.CurrentHealth = target.Health;
            ////Console.WriteLine($", lyckas och helar {damageRoll} hälsa!");
            ////Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i hälsa.");
        }
    }
}