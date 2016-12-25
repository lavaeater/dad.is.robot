using rds;
using robot.dad.common;

namespace robot.dad.game.Event
{
    public sealed class ArmorTable : ThingTable
    {
        public ArmorTable(int count)
        {
            AddEntry(new BasicArmor(1), 100);
            AddEntry(new BasicArmor(2), 50);
            AddEntry(new BasicArmor(3), 25);
            AddEntry(new BasicArmor(4), 12);
            AddEntry(new BasicArmor(5), 6);
            Count = count;
        }
    }
}