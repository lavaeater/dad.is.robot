using rds;

namespace robot.dad.game.Event
{
    public sealed class FlyingMonsterTable : ThingTable
    {
        public FlyingMonsterTable()
        {
            AddEntry(new FlyingMonster("J�ttemal"), 100);
            AddEntry(new FlyingMonster("Megatordyvel"), 100);
            AddEntry(new FlyingMonster("J�ttesyrsesv�rm"), 100);
            AddEntry(new FlyingMonster("Slaktarbagge"), 100);
            Count = 1;
        }
    }
}