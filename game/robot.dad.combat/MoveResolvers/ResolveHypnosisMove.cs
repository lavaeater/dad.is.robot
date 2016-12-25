using System;
using System.Linq;
using robot.dad.combat.EffectAppliers;
using robot.dad.common;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveHypnosisMove : ResolveMoveBase
    {
        public override bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            bool result = false;
            int targetValue = attacker.CurrentAttack + move.Modifier - target.CurrentDefense;
            int diceRoll = DiceRoller.RollHundredSided();
            if (diceRoll <= targetValue)
            {
                result = true;
                int perfectRoll = targetValue/10;
                var applier = new HypnosisEffectApplier(diceRoll <= perfectRoll ? CombatEngine.Round + move.MaxDamage : CombatEngine.Round + DiceRoller.RollDice(move.MinDamage, move.MaxDamage));
                if (target.CurrentCombatEffects.Any(item => item.GetType() == typeof(HypnosisEffectApplier)))
                {
                    var item = target.CurrentCombatEffects.Single(ef => ef.GetType() == typeof(HypnosisEffectApplier));
                    item.LastRound = applier.LastRound;
                }
                else
                {
                    target.CurrentCombatEffects.Add(applier);
                }
            }
            return result;
        }
    }
}