namespace robot.dad.common
{
    public class BasicWeapon : CharacterWeapon
    {
        public BasicWeapon(int level, string name, string verbified = "skjuter")
        {
            Verbified = verbified;
            MinDamage = level*2;
            MaxDamage = level*3;
            Attack = level*4;
            Name = name;
            ItemKey = $"BasicWeapon {level} {Name} {Attack}";
        }
    }
}