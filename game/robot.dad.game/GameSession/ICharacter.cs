using System.Collections.Generic;

namespace robot.dad.game.GameSession
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
        List<IITem> Inventory { get; set; }
        IEnumerable<ICharacterComponent> PlayerComponents { get; }
        IEnumerable<ICharacterComponent> ActiveComponents { get; }
    }
}