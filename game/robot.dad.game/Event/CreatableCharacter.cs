using System.Collections.Generic;
using rds;
using robot.dad.common;

namespace robot.dad.game.Event
{
    public class CreatableCharacter : CreatableThing, ICharacter
    {
        public CreatableCharacter()
        {
            
        }

        public override string ToString()
        {
            return $"Name: {Name}, Description: {Description} - {GetType()}";
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Armor { get; set; }
        public int CurrentStrength => Strength;

        public int CurrentMaxHealth => MaxHealth;
        public int CurrentAttack => Attack;
        public int CurrentDefense => Defense;
        public int CurrentArmor => Armor;
        public int Initiative { get; set; }
        public int CurrentInitiative => Initiative;
        public Dictionary<IITem, int> Inventory { get; set; } = new Dictionary<IITem, int>();
        public IEnumerable<ICharacterComponent> PlayerComponents => new List<ICharacterComponent>();
        public IEnumerable<ICharacterComponent> ActiveComponents => new List<ICharacterComponent>();
    }
}