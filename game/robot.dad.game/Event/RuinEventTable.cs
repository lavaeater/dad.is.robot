using rds;

namespace robot.dad.game.Event
{
    public enum EventType
    {
        Ruin,
        Monster,
        Cave,
        Settlement,
        Scavenger
    }
    public sealed class TileEventTable : ThingTable
    {
        public TileEventTable()
        {
            AddEntry(new ThingValue<EventType>(EventType.Ruin, 50));
            AddEntry(new ThingValue<EventType>(EventType.Scavenger, 100));
            AddEntry(new ThingValue<EventType>(EventType.Monster, 10));
            AddEntry(new ThingValue<EventType>(EventType.Cave, 10));
            AddEntry(new ThingValue<EventType>(EventType.Settlement, 50));
            AddEntry(new ThingNullValue(1000));
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