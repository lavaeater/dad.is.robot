using rds;

namespace robot.dad.game.Event
{
    public sealed class TileEventTable : ThingTable
    {
        public TileEventTable()
        {
            AddEntry(new RuinEventTable(), 10);
            AddEntry(new ThingNullValue(990));
            Count = 1;
        }    
    }

    public sealed class RuinEventTable : ThingTable
    {
        public RuinEventTable()
        {
            AddEntry(new ScavengerTable(2), 100);
            //AddEntry(new FlyingMonsterTable(), 50);
            //AddEntry(new ThingNullValue(100));
            Count = 1;
        }
    }
}