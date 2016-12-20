namespace robot.dad.game.GameSession
{
    public interface ICharacterComponent : IITem
    {
        int Strength { get; set; }
        int MaxHealth { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }
        int Armor { get; set; }
        bool Active { get; set; }
    }
}