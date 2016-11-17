using System;

namespace robot.dad.combat.EffectAppliers
{
    public class HealingEffectApplier : ApplyMoveEffectsBase
    {
        public HealingEffectApplier(int min, int max) : base(min, max, 0, EffectType.Immediate)
        {
        }

        public override void ApplyEffects(Combattant target)
        {
            int damageRoll = DiceRoller.RollDice(Min, Max);
            target.CurrentHealth += damageRoll;
            if (target.CurrentHealth > target.Health) target.CurrentHealth = target.Health;
            Console.WriteLine($", lyckas och helar {damageRoll} h�lsa!");
            Console.WriteLine($"{target.Name} har {target.CurrentHealth} kvar i h�lsa.");
        }
    }
}