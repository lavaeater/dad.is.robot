namespace robot.dad.world
{
    public class MapEvent
    {
        public MapEvent(MapEventType eventType)
        {
            EventType = eventType;
        }
        public readonly MapEventType EventType;

    }
}