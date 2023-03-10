using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace robot.dad.common
{
    public class Character : ICharacter
    {
        public Character()
        {
            
        }
        public Character(string name, string description, int strength, int maxHealth, int attack, int defense, int armor, IList<IItem> inventory)
        {
            Name = name;
            Description = description;
            Strength = strength;
            MaxHealth = maxHealth;
            Attack = attack;
            Defense = defense;
            Armor = armor;
            Inventory = inventory;
        }
        public override string ToString()
        {
            return $"Name: {Name}, Description: {Description}, Strength: {Strength}, MaxHealth: {MaxHealth}, Attack: {Attack}, Defense: {Defense}, Armor: {Armor}, CurrentStrength: {CurrentStrength}, CurrentMaxHealth: {CurrentMaxHealth}, CurrentAttack: {CurrentAttack}, CurrentDefense: {CurrentDefense}, CurrentArmor: {CurrentArmor}, Inventory: {Inventory}, PlayerComponents: {PlayerComponents}, ActiveComponents: {ActiveComponents}";
        }

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

        public int Initiative { get; set; }

        public int CurrentInitiative
        {
            get { return Initiative + ActiveComponents.Sum(pc => pc.Initiative); }
        }

        public IList<IItem> Inventory { get; set; }
        public IEnumerable<ICharacterComponent> PlayerComponents => Inventory.OfType<ICharacterComponent>();
        public IEnumerable<ICharacterComponent> ActiveComponents => PlayerComponents.Where(pc => pc.Active);
        public IEnumerable<IWeapon> Weapons => Inventory.OfType<IWeapon>();
        public IEnumerable<IWeapon> ActiveWeapons => Weapons.Where(w => w.Active);
    }
}