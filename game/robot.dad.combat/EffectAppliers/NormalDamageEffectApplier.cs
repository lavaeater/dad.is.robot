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
            int damageRoll = DiceRoller.RollDice(Min, Max);
            Console.WriteLine($", träffar och gör {target.ApplyDamage(damageRoll)} i skada!");
            Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i hälsa.");
        }
    }
}