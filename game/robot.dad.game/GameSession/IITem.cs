using rds;

namespace robot.dad.game.GameSession
{
    public interface IITem
    {
        string ItemKey { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }

    public class WeaponAttachment : CharacterComponent
    {
        public WeaponAttachment(string name, int level)
        {
            Name = name;
            Attack = level*3;
            ItemKey = $"{Name}{level}{Attack}";
        }
    }

    public class BasicWeapon : CharacterComponent
    {
        public BasicWeapon(int level, string name)
        {
            MinDamage = level*2;
            MaxDamage = level*3;
            Attack = level*4;
            Name = name;
            ItemKey = $"BasicWeapon {level} {Name} {Attack}";
        }
    }

    public class BasicArmor : CharacterComponent
    {
        public BasicArmor(int level)
        {
            Armor = level*3;
            Name = $"Protective gear Level {level}";
            ItemKey = $"Armor {Armor}";
        }
    }

    public class CharacterComponent : Thing, ICharacterComponent
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
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public int Initiative { get; set; }
    }

    public class BasicItem : Thing, IITem
    {
        public string ItemKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BasicItem(string name, string description)
        {
            ItemKey = name;
            Name = name;
            Description = description;
        }
        public BasicItem(string itemKey, string name, string description)
        {
            ItemKey = itemKey;
            Name = name;
            Description = description;
        }
    }
}