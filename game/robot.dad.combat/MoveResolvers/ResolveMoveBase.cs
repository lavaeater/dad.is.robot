using robot.dad.common;

namespace robot.dad.combat.MoveResolvers
{
    public abstract class ResolveMoveBase : IResolveMove
    {
        public abstract bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target);
    }
}