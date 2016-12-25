using System;
using robot.dad.common;

namespace robot.dad.combat.EffectAppliers
{
    public abstract class ApplyEffectsBase: IApplyEffects
    {
        protected ApplyEffectsBase(int min, int max, int lastRound, EffectType effectType)
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
        public int RecentRound { get; set; }
        public abstract void ApplyEffects(ICombattant target);

        public virtual void EffectsEnded(ICombattant target)
        {
            target.CurrentCombatEffects.Remove(this);
        }
    }
}