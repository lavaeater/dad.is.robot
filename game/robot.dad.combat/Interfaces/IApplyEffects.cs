using robot.dad.combat.EffectAppliers;

namespace robot.dad.combat.Interfaces
{
    public interface IApplyEffects
    {
        string EffectName { get; set; }
        int Min { get; set; }
        int Max { get; set; }
        int LastRound { get; set; }
        EffectType EffectType { get; set; }
        void ApplyEffects(Combattant target);
        void EffectsEnded(Combattant target);
    }
}