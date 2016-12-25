using rds;

namespace robot.dad.common
{
    public class CharacterWeapon : Thing, IWeapon
    {
        public string ItemKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Armor { get; set; }
        public bool Active { get; set; }
        public int Initiative { get; set; }
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }

        public virtual ICombatMove CombatMove => new WeaponCombatMove(this);

        public string Verbified { get; set; }
    }
}