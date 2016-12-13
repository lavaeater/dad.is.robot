using System;
using System.Collections.Generic;
using System.Linq;

namespace robot.dad.combat.EffectAppliers
{
    public class HypnosisEffectApplier: RecurringEffectApplierBase
    {
        private bool _hasBeenApplied = false;
        public HypnosisEffectApplier(int lastRound) : base(0, 0, lastRound)
        {
            EffectName = "Hypnotiserad";
        }

        public override void ApplyEffects(Combattant target)
        {
            if (!_hasBeenApplied)
            {
                OriginalPicker = target.MovePicker;
                target.MovePicker = MovePickers.GetRandomReversePicker();
                _hasBeenApplied = true;
            }
        }

        public IPickMoves OriginalPicker { get; set; }

        public override void EffectsEnded(Combattant target)
        {
            target.MovePicker = OriginalPicker;
            base.EffectsEnded(target);
        }
    }
}