using robot.dad.combat.Interfaces;

namespace robot.dad.combat.MoveResolvers
{
    public abstract class ResolveMoveBase : IResolveMove
    {
        public abstract void ResolveMove(CombatMove move, Combattant attacker, Combattant target);
    }
}