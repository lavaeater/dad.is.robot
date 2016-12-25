namespace robot.dad.combat.EffectAppliers
{
    public class HypnosisEffectApplier: RecurringEffectApplierBase
    {
        private bool _hasBeenApplied = false;
        public HypnosisEffectApplier(int lastRound) : base(0, 0, lastRound)
        {
            EffectName = "Hypnotiserad";
        }

        public override void ApplyEffects(ICombattant target)
        {
            if (!_hasBeenApplied)
            {
                OriginalPicker = target.MovePicker;
                target.MovePicker = MovePickers.GetRandomReversePicker();
                _hasBeenApplied = true;
            }
        }

        public IPickMove OriginalPicker { get; set; }

        public override void EffectsEnded(ICombattant target)
        {
            target.MovePicker = OriginalPicker;
            base.EffectsEnded(target);
        }
    }
}