using rds;

namespace robot.dad.game.Event
{
    public sealed class TileEventTable : ThingTable
    {
        public TileEventTable()
        {
            AddEntry(new ThingValue<EventType>(EventType.Ruin, 10));
            AddEntry(new ThingValue<EventType>(EventType.Scavenger, 10));
            AddEntry(new ThingValue<EventType>(EventType.Monster, 10));
            AddEntry(new ThingValue<EventType>(EventType.Cave, 10));
            AddEntry(new ThingValue<EventType>(EventType.Settlement, 10));
            AddEntry(new ThingNullValue(1000));
            Count = 1;
        }    
    }
}