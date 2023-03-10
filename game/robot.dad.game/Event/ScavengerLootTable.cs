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
            AddEntry(new BasicItem("Elektroniska komponenter", "Delar av datorer och annan utrustning fr?n den gamla tiden"), 25);
            AddEntry(new CountableItem("Food", "Mat",10, 100));
            AddEntry(new CountableItem("Money", "Mynt",10, 100));
            Count = 3*numberOfScavengers;
        }
    }
}