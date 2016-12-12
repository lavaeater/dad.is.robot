using System;
using System.Linq;
using robot.dad.combat.EffectAppliers;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveHypnosisMove : ResolveMoveBase
    {
        public override void ResolveMove(CombatMove move, Combattant attacker, Combattant target)
        {
            //Console.WriteLine();

            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            int diceRoll = DiceRoller.RollHundredSided();
            //Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //Console.Write($"och sl�r {diceRoll}");
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
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                //Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        }
    }
}