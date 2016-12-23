using rds;

namespace robot.dad.game.Event
{
    public class Scavenger : CreatableCharacter
    {
        public int Level { get; set; }

        public Scavenger(int level)
        {
            Level = level;
        }

        public override IThing CreateInstance()
        {
            return new Scavenger(Level);
        }
    }
}