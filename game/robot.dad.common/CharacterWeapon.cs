using rds;

namespace robot.dad.common
{
    public class CharacterWeapon : Thing, IWeapon
    {
        public CharacterWeapon()
        {
            
        }
        public CharacterWeapon(string name, string description,int attack, bool active, int initiative, int maxDamage, int minDamage, string verbified)
        {
            Name = name;
            Description = description;
            Attack = attack;
            Active = active;
            Initiative = initiative;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
            Verbified = verbified;
            ItemKey = $"{Name}";
        }

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