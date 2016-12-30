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

        public IApplyEffects EffectApplier => new WeaponEffectApplier("Mörderiserar", MinDamage, MaxDamage, 0);
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

    public class WeaponEffectApplier : IApplyEffects
    {
        public WeaponEffectApplier(string effectName, int min, int max, int lastRound)
        {
            EffectName = effectName;
            Min = min;
            Max = max;
        }
        public string EffectName { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int LastRound { get; set; }
        public EffectType EffectType => EffectType.Immediate;

        public void ApplyEffects(ICombattant target)
        {
            target.ApplyDamage(DiceRoller.RollDice(Min, Max));
        }

        public void EffectsEnded(ICombattant target)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateMinAndMax(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}