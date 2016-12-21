namespace robot.dad.game.GameSession
{
    public class ShipComponent : IShipComponent
    {
        public string ItemKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxItems { get; set; }
        public int MaxSpeed { get; set; }
        public bool Active { get; set; }
    }
}