using rds;

namespace robot.dad.game.Event
{
    public class Scavenger : CreatableCharacter
    {
        public int Level { get; set; }

        public Scavenger(int level)
        {
            Level = level;
            Attack = 40 + level*10;
            Defense = 10 + level*10;
            Strength = 10 + level*10;
            MaxHealth = 30 + level*5;
            Armor = 5 + level*5;
        }

        public override IThing CreateInstance()
        {
            return new Scavenger(Level);
        }
    }
}