namespace robot.dad.common
{
    public class BasicArmor : CharacterComponent
    {
        public BasicArmor(int level)
        {
            Armor = level*3;
            Name = $"Protective gear Level {level}";
            ItemKey = $"Armor {Armor}";
        }
    }
}