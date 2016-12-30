namespace robot.dad.common
{
    public interface ICombatMove
    {
        IApplyEffects EffectApplier { get; }
        int MaxDamage { get; }
        int MinDamage { get; }
        int Modifier { get; }
        IResolveMove MoveResolver { get; }
        CombatMoveType MoveType { get; }
        string Name { get; }
        string Verbified { get; }

        bool Resolve(ICombattant attacker, ICombattant target);
    }
}