using System.Collections.Generic;

namespace robot.dad.common
{
    public interface ICharacter
    {
        string Name { get; set; }
        string Description { get; set; }
        int Strength { get; set; }
        int MaxHealth { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }
        int Armor { get; set; }
        int CurrentStrength { get; }
        int CurrentMaxHealth { get; }
        int CurrentAttack { get; }
        int CurrentDefense { get; }
        int CurrentArmor { get; }
        int Initiative { get; }
        int CurrentInitiative { get; }
        Dictionary<IITem, int> Inventory { get; set; }
        IEnumerable<ICharacterComponent> PlayerComponents { get; }
        IEnumerable<ICharacterComponent> ActiveComponents { get; }
        IEnumerable<IWeapon> Weapons { get; }
        IEnumerable<IWeapon> ActiveWeapons { get; }
    }
}