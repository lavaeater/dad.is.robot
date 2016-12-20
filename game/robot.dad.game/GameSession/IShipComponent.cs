namespace robot.dad.game.GameSession
{
    public interface IShipComponent : IITem
    {
        int MaxItems { get; set; }
        int MaxSpeed { get; set; }
        bool Active { get; set; }
    }
}