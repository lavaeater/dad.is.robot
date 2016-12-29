namespace robot.dad.common
{
    public interface IItem
    {
        string ItemKey { get; }
        string Name { get; }
        string Description { get; }
    }
}