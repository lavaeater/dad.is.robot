using robot.dad.combat.Interfaces;

namespace robot.dad.combat
{
    public interface ICombatMove
    {
        IApplyEffects EffectApplier { get; set; }
        int MaxDamage { get; set; }
        int MinDamage { get; set; }
        int Modifier { get; set; }
        IResolveMove MoveResolver { get; set; }
        CombatMoveType MoveType { get; set; }
        string Name { get; set; }
        string Verbified { get; set; }

        bool Apply(ICombattant attacker, ICombattant target);
    }
}