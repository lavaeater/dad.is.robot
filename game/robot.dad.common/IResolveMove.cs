namespace robot.dad.common
{
    public interface IResolveMove
    {
        bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target);
    }
}