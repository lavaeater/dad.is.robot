namespace robot.dad.common
{
    public interface ICharacterComponent : IItem
    {
        int Strength { get; set; }
        int MaxHealth { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }
        int Armor { get; set; }
        bool Active { get; set; }
        int Initiative { get; set; }
    }
}