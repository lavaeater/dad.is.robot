namespace robot.dad.common
{
    public interface IWeapon : ICharacterComponent
    {
        int MaxDamage { get; set; }
        int MinDamage { get; set; }
        ICombatMove CombatMove { get; }
        string Verbified { get; set; }
    }
}