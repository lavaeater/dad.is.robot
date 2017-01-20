using rds;

namespace robot.dad.game.Event
{
    public sealed class RuinEventTable : ThingTable
    {
        public RuinEventTable()
        {
            AddEntry(new ScavengerTable(2), 100);
            Count = 1;
        }
    }
}