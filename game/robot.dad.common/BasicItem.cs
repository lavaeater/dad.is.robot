using rds;

namespace robot.dad.common
{
    public class BasicItem : Thing, IItem
    {
        public string ItemKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BasicItem()
        {
            
        }

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

        public override string ToString()
        {
            return $"ItemKey: {ItemKey}, Name: {Name}";
        }
    }
}