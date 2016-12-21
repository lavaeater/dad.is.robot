using System.Collections.Generic;
using System.Linq;

namespace robot.dad.game.GameSession
{
    public class Character : ICharacter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Armor { get; set; }

        public int CurrentStrength
        {
            get { return Strength + ActiveComponents.Sum(pc => pc.Strength); }
        }

        public int CurrentMaxHealth
        {
            get { return MaxHealth + ActiveComponents.Sum(pc => pc.MaxHealth); }
        }

        public int CurrentAttack
        {
            get { return Attack + ActiveComponents.Sum(pc => pc.Attack); }
        }

        public int CurrentDefense
        {
            get { return Defense + ActiveComponents.Sum(pc => pc.Defense); }
        }

        public int CurrentArmor
        {
            get { return Armor + ActiveComponents.Sum(pc => pc.Armor); }
        }

        public Dictionary<IITem, int> Inventory { get; set; }
        public IEnumerable<ICharacterComponent> PlayerComponents => Inventory.OfType<ICharacterComponent>();
        public IEnumerable<ICharacterComponent> ActiveComponents => PlayerComponents.Where(pc => pc.Active);
    }
}