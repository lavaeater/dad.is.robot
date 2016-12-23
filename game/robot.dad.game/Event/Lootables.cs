using System.Collections.Generic;
using System.Linq;
using rds;
using robot.dad.game.GameSession;

namespace robot.dad.game.Event
{
    public static class Lootables
    {
        //Basic table for ruin scavengers - they are not always there, the scavengers, in the ruins
        //First, a table for encounters - what is found on the ground?
        
        //Two, dependending on the encounter, what does that encounter actually consist of?
        
        /*
         * It will be like this, the player flies close to something. This triggers a table 
         * 
         * Close to tile => Ruin => With Scavengers => Combat => Loot Drop => Also, access to ruin
         * Close to tile => Ruin => No Scavengers in air => Explorable ruin (next step)
         * Close to tile => Ruin => Flying monster => Explorable ruin (next step)
         */
        //THis is obviously for a tile that yes, has an event
        public static IEnumerable<IThing> GetEventsForATile()
        {
            var table = new TileEventTable();
            return table.Result;
        }

        public static IEnumerable<IITem> GetLootFromScavengers(int numberOfScavengers)
        {
            var table = new ScavengerLootTable(numberOfScavengers);
            return table.Result.OfType<IITem>();
        }

    }

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

    public sealed class WeaponsTable : ThingTable
    {
        public WeaponsTable(int count)
        {
            AddEntry(new BasicWeapon(1, "Hemmagjord bössa"), 300);
            AddEntry(new BasicWeapon(2, "Femskjutare"), 150);
            AddEntry(new BasicWeapon(2, "Avsågat hagelgevär"), 75);
            AddEntry(new BasicWeapon(3, "Självborrat gevär"), 30);
            AddEntry(new BasicWeapon(4, "Dunderobrakpistol"), 15);
            AddEntry(new BasicWeapon(5, "Rostigt automatgevär"), 7);
            AddEntry(new BasicWeapon(6, "Laser"), 3);
            AddEntry(new BasicWeapon(7, "Maser"), 2);
            AddEntry(new BasicWeapon(8, "Plasma"), 1);
            AddEntry(new BasicWeapon(9, "BFG"), 1);
            Count = count;
        }
    }
}