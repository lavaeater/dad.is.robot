namespace robot.dad.common
{
    public interface IApplyEffects
    {
        string EffectName { get; set; }
        int Min { get; set; }
        int Max { get; set; }
        int LastRound { get; set; }
        EffectType EffectType { get; set; }
        void ApplyEffects(ICombattant target);
        void EffectsEnded(ICombattant target);
        void UpdateMinAndMax(int min, int max);
    }
}