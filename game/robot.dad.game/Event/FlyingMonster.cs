using rds;

namespace robot.dad.game.Event
{
    public class FlyingMonster : CreatableCharacter
    {
        public FlyingMonster(string name)
        {
            Name = name;
        }

        public override IThing CreateInstance()
        {
            return new FlyingMonster(Name);
        }
    }
}