using robot.dad.common.MoveResolvers;

namespace robot.dad.common
{
    public class WeaponCombatMove : ICombatMove
    {
        public IWeapon Weapon { get; set; }

        public WeaponCombatMove(IWeapon weapon)
        {
            Weapon = weapon;
        }

        public IApplyEffects EffectApplier { get; set; }
        public int MaxDamage => Weapon.MaxDamage;
        public int MinDamage => Weapon.MaxDamage;
        public int Modifier { get; set; } //Hmm, modifier - is calculated on combattant level. Not needed here
        public IResolveMove MoveResolver => ResolveWeaponMove.WeaponeMoveResolver;
        public CombatMoveType MoveType => CombatMoveType.Attack;
        public string Name => Weapon.Name;
        public string Verbified => Weapon.Verbified;

        public bool Resolve(ICombattant attacker, ICombattant target)
        {
            return MoveResolver.ResolveMove(this, attacker, target);
        }
    }
}