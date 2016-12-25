using System.Collections.Generic;
using System.Linq;

namespace robot.dad.common
{
    public class Ship :  IShip
    {
        public IEnumerable<IShipComponent> ActiveComponents => ShipComponents.Where(pc => pc.Active);
        public string Name { get; set; }
        public int MaxItems { get; set; }
        public int MaxSpeed { get; set; }

        public IEnumerable<IShipComponent> ShipComponents => ShipInventory.OfType<IShipComponent>();

        public int CurrentMaxItems
        {
            get { return MaxItems + ActiveComponents.Sum(sc => sc.MaxItems); }
        }

        public int CurrentMaxSpeed
        {
            get { return MaxSpeed + ActiveComponents.Sum(sc => sc.MaxSpeed); }
        }

        public List<IITem> ShipInventory { get; set; }
    }
}