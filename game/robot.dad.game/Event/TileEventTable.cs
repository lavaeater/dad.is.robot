using rds;

namespace robot.dad.game.Event
{
    public sealed class TileEventTable : ThingTable
    {
        public TileEventTable()
        {
            AddEntry(new ScavengerTable(), 100);
            //AddEntry(new FlyingMonsterTable(), 50);
            //AddEntry(new ThingNullValue(100));
            Count = 1;
        }
    }
}