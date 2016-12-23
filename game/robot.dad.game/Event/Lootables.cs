using rds;

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
        public static ThingTable TileEventTable => new TileEventTable();

    }
}