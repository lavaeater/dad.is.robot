using rds;
using robot.dad.common;

namespace robot.dad.game.Event
{
    public sealed class WeaponsTable : ThingTable
    {
        public WeaponsTable(int count)
        {
            AddEntry(new BasicWeapon(1, "Hemmagjord b�ssa"), 300);
            AddEntry(new BasicWeapon(2, "Femskjutare"), 150);
            AddEntry(new BasicWeapon(2, "Avs�gat hagelgev�r"), 75);
            AddEntry(new BasicWeapon(3, "Sj�lvborrat gev�r"), 30);
            AddEntry(new BasicWeapon(4, "Dunderobrakpistol"), 15);
            AddEntry(new BasicWeapon(5, "Rostigt automatgev�r"), 7);
            AddEntry(new BasicWeapon(6, "Laser"), 3);
            AddEntry(new BasicWeapon(7, "Maser"), 2);
            AddEntry(new BasicWeapon(8, "Plasma"), 1);
            AddEntry(new BasicWeapon(9, "BFG"), 1);
            Count = count;
        }
    }
}