namespace robot.dad.combat.Interfaces
{
    public interface IResolveMove
    {
        bool ResolveMove(CombatMove move, Combattant attacker, Combattant target);
    }
}