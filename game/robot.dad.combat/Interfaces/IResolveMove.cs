namespace robot.dad.combat.Interfaces
{
    public interface IResolveMove
    {
        bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target);
    }
}