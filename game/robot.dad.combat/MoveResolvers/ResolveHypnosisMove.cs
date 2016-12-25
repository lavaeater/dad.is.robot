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
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            int diceRoll = DiceRoller.RollHundredSided();
            if (diceRoll <= targetValue)
            {
                result = true;
                int perfectRoll = targetValue/10;
                var applier = new HypnosisEffectApplier(diceRoll <= perfectRoll ? CombatEngine.Round + move.MaxDamage : CombatEngine.Round + DiceRoller.RollDice(move.MinDamage, move.MaxDamage));
                if (target.CombatEffects.Any(item => item.GetType() == typeof(HypnosisEffectApplier)))
                {
                    var item = target.CombatEffects.Single(ef => ef.GetType() == typeof(HypnosisEffectApplier));
                    item.LastRound = applier.LastRound;
                }
                else
                {
                    target.CombatEffects.Add(applier);
                }
            }
            return result;
        }
    }
}