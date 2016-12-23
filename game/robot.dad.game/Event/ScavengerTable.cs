using rds;

namespace robot.dad.game.Event
{
    public sealed class ScavengerTable : ThingTable
    {
        public ScavengerTable()
        {
            AddEntry(new Scavenger(1),300);
            AddEntry(new Scavenger(2),150);
            AddEntry(new Scavenger(3),75);
            AddEntry(new Scavenger(4),30);
            AddEntry(new Scavenger(5),15);

            Count = 3;
        }
    }
}