namespace robot.dad.combat.Interfaces
{
    public interface IResolveMove
    {
        void ResolveMove(CombatMove move, Combattant attacker, Combattant target);
    }
}