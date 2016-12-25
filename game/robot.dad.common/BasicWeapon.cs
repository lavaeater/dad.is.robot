namespace robot.dad.common
{
    public class BasicWeapon : CharacterComponent
    {
        public BasicWeapon(int level, string name)
        {
            MinDamage = level*2;
            MaxDamage = level*3;
            Attack = level*4;
            Name = name;
            ItemKey = $"BasicWeapon {level} {Name} {Attack}";
        }
    }
}