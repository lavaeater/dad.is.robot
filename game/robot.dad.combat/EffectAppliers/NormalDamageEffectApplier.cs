using System;
using robot.dad.common;

namespace robot.dad.combat.EffectAppliers
{
    public class NormalDamageEffectApplier : ApplyEffectsBase 
    {
        public NormalDamageEffectApplier(int min, int max) : base(min, max, 0, EffectType.Immediate)
        {
        }

        public override void ApplyEffects(ICombattant target)
        {
            target.ApplyDamage(DiceRoller.RollDice(Min, Max));
        }
    }
}