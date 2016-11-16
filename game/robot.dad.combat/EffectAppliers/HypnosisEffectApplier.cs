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
        }

        public override void ApplyEffects(Combattant target)
        {
            //Switches movepickers!
            Console.WriteLine($"{target.Name} är hypnotiserad!");
            if (!_hasBeenApplied)
            {
                OriginalPicker = target.MovePicker;
                target.MovePicker = MovePickers.RandomReversePicker;
                _hasBeenApplied = true;
            }
        }

        public Action<Combattant, List<Combattant>, List<CombatMove>> OriginalPicker { get; set; }

        public override void EffectsEnded(Combattant target)
        {
            target.MovePicker = OriginalPicker;
            base.EffectsEnded(target);
        }
    }
}