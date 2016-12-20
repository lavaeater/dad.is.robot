using System.Collections.Generic;
using System.Linq;

namespace robot.dad.game.GameSession
{
    public class Player
    {
        public string Name { get; set; }
        public Inventory Inventory { get; set; }
    }

    public class Inventory
    {

    }

    public interface IShipComponent
    {
        string Name { get; set; }
        int MaxItems { get; set; }
        int MaxSpeed { get; set; }
    }

    public interface IShip
    {
        int CurrentMaxItems { get;}
        int CurrentMaxSpeed { get;}
    }

    public class Ship : IShipComponent, IShip
    {
        public string Name { get; set; }
        public int MaxItems { get; set; }
        public int MaxSpeed { get; set; }
        public List<IShipComponent> ShipComponents { get; set; }
        public int CurrentMaxItems
        {
            get { return MaxItems + ShipComponents.Sum(sc => sc.MaxItems); }
        }

        public int CurrentMaxSpeed
        {
            get { return MaxSpeed + ShipComponents.Sum(sc => sc.MaxSpeed); }
        }
    }

    public class ShipComponent : IShipComponent
    {
        public string Name { get; set; }
        public int MaxItems { get; set; }
        public int MaxSpeed { get; set; }
    }
}
