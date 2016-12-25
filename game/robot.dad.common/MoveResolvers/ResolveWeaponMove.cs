namespace robot.dad.common.MoveResolvers
{
    public class ResolveWeaponMove : IResolveMove
    {
        public static IResolveMove WeaponeMoveResolver => new ResolveWeaponMove();

        public bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            int targetValue = attacker.CurrentAttack + move.Modifier - target.CurrentDefense;
            if (target.CurrentMove?.MoveType == CombatMoveType.Defend ||
                target.CurrentMove?.MoveType == CombatMoveType.Runaway)
            {
                targetValue += target.CurrentMove.Modifier;
            }
            int perfectRollValue = targetValue / 10;

            int diceRoll = DiceRoller.RollHundredSided();
            bool result = false;
            if (diceRoll <= targetValue)
            {
                result = true;
                //1 == perfekt slag!

                //Effect applier comes from the attack, not from here, right?

                move.EffectApplier.UpdateMinAndMax(diceRoll <= perfectRollValue ? 
                    move.MaxDamage : move.MinDamage,
                    move.MaxDamage);

                move.EffectApplier.ApplyEffects(target);
            }
            return result;
        }
    }
}