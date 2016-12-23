using rds;

namespace robot.dad.game.Event
{
    public sealed class FlyingMonsterTable : ThingTable
    {
        public FlyingMonsterTable()
        {
            AddEntry(new FlyingMonster("Jättemal"), 100);
            AddEntry(new FlyingMonster("Megatordyvel"), 100);
            AddEntry(new FlyingMonster("Jättesyrsesvärm"), 100);
            AddEntry(new FlyingMonster("Slaktarbagge"), 100);
            Count = 1;
        }
    }
}