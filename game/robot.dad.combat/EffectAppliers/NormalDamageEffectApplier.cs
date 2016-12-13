using System;

namespace robot.dad.combat.EffectAppliers
{
    public class NormalDamageEffectApplier : ApplyEffectsBase 
    {
        public NormalDamageEffectApplier(int min, int max) : base(min, max, 0, EffectType.Immediate)
        {
        }

        public override void ApplyEffects(Combattant target)
        {
            target.ApplyDamage(DiceRoller.RollDice(Min, Max));
        }
    }
}