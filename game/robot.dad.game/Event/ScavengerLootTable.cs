using rds;
using robot.dad.common;

namespace robot.dad.game.Event
{
    public sealed class ScavengerLootTable : ThingTable
    {
        public ScavengerLootTable(int numberOfScavengers)
        {
            Count = numberOfScavengers;
            AddEntry(new WeaponsTable(1), 300);
            AddEntry(new ArmorTable(1), 200);
            AddEntry(new BasicItem("Elektroniska komponenter", "Delar av datorer och annan utrustning från den gamla tiden"), 25);
            AddEntry(new BasicItem("Mat", "Mat, helt enkelt"), 100);
            Count = 3*numberOfScavengers;
        }
    }
}