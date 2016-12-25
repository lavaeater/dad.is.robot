using robot.dad.common;

namespace robot.dad.combat
{
    public class CombatMove : ICombatMove
    {
        public CombatMove(string name, CombatMoveType moveType, int modifier, int minDamage, int maxDamage, string verbified, IResolveMove moveResolver)
        {
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Verbified = verbified;
            MoveResolver = moveResolver;
        }

        public bool Apply(ICombattant attacker, ICombattant target)
        {
            return MoveResolver.ResolveMove(this, attacker, target);
        }

        public string Name { get; set; }
        public CombatMoveType MoveType { get; set; }
        public int Modifier { get; set; }
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public string Verbified { get; set; }
        public IResolveMove MoveResolver { get; set; }
        public IApplyEffects EffectApplier { get; set; }
    }
}