using System.Collections.Generic;

namespace robot.dad.world
{
    /// <summary>
    /// This is a container for all things related to the world. 
    /// At its heart it has a dictionary of tiles!
    /// </summary>
    public class World
    {
        private Dictionary<MapKey, MapTile> Map => new Dictionary<MapKey, MapTile>();

        public MapTile GetMapTileAt(long x, long y)
        {
            return GetMapTileForKey(new MapKey(x, y));
        }

        public MapTile GetMapTileForKey(MapKey key)
        {
            if (!Map.ContainsKey(key))
            {
                Map.Add(key, WorldGenerator.GenerateTileForKey(key));
            }
            return Map[key];
        }
    }

    class WorldGenerator
    {

        public static MapTile GenerateTileForKey(MapKey key)
        {
            return new MapTile(key, MapTileType.Grassland);
        }
    }
}
