using rds;

namespace robot.dad.common
{
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
        public int Initiative { get; set; }
    }

    public class CountableItem : ThingValue<int>, IItem
    {
        public CountableItem() : base(0,0)
        {
            
        }
        public CountableItem(string itemKey, string name, int value, double probability) : base(value, probability)
        {
            ItemKey = itemKey;
            Name = name;
        }

        public CountableItem(string itemKey, string name, int value, double probability, bool unique, bool always, bool enabled) : base(value, probability, unique, always, enabled)
        {
            ItemKey = itemKey;
            Name = name;
        }

        public string ItemKey { get; set; }
        public string Name {get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"ItemKey: {ItemKey}, Name: {Name}";
        }
    }
}