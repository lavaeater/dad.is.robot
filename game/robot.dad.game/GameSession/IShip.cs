using System.Collections.Generic;

namespace robot.dad.game.GameSession
{
    public interface IShip
    {
        int CurrentMaxItems { get; }
        int CurrentMaxSpeed { get; }
        List<IITem> ShipInventory { get; set; }
        IEnumerable<IShipComponent> ShipComponents { get; }
        IEnumerable<IShipComponent> ActiveComponents { get; }
        string Name { get; set; }
        int MaxItems { get; set; }
        int MaxSpeed { get; set; }
    }
}