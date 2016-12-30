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

    public class Money : ThingValue<int>, IItem
    {
        public Money(int value, double probability) : base(value, probability)
        {
        }

        public Money(int value, double probability, bool unique, bool always, bool enabled) : base(value, probability, unique, always, enabled)
        {
        }

        public string ItemKey => "Coin";
        public string Name => "Mynt";
        public string Description { get; set; }
    }
}