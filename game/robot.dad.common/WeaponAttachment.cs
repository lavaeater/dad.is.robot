namespace robot.dad.common
{
    public class WeaponAttachment : CharacterComponent
    {
        public WeaponAttachment(string name, int level)
        {
            Name = name;
            Attack = level*3;
            ItemKey = $"{Name}{level}{Attack}";
        }
    }
}