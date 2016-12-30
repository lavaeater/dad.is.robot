namespace robot.dad.common
{
    public interface IShipComponent : IItem
    {
        int MaxItems { get; set; }
        int MaxSpeed { get; set; }
        bool Active { get; set; }
    }
}