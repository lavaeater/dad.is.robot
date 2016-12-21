namespace robot.dad.game.GameSession
{
    public interface IITem
    {
        string ItemKey { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }

    public class BasicItem : IITem
    {
        public string ItemKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public BasicItem()
        {
            
        }
        public BasicItem(string itemKey, string name, string description)
        {
            ItemKey = itemKey;
            Name = name;
            Description = description;
        }
    }
}