namespace robot.dad.combat.EffectAppliers
{
    public abstract class RecurringEffectApplierBase : ApplyEffectsBase
    {
        protected RecurringEffectApplierBase(int min, int max, int lastRound) : base(min, max, lastRound, EffectType.Recurring)
        {
        }
    }
}