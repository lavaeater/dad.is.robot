using System;
using robot.dad.combat.Interfaces;

namespace robot.dad.combat.EffectAppliers
{
    public abstract class ApplyMoveEffectsBase: IApplyMoveEffects
    {
        protected ApplyMoveEffectsBase(int min, int max, int lastRound, EffectType effectType)
        {
            Min = min;
            Max = max;
            LastRound = lastRound;
            EffectType = effectType;
        }

        public string EffectName { get; set; }
        public string AffectedProperty { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int LastRound { get; set; }
        public EffectType EffectType { get; set; }
        public abstract void ApplyEffects(Combattant target);

        public virtual void EffectsEnded(Combattant target)
        {
            target.CombatEffects.Remove(this);
        }
    }
}