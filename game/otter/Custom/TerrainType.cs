namespace Otter.Custom
{
    public enum TerrainType
    {
        Ocean,
        Beach,
        Scorched,
        Bare,
        Tundra,
        Snow,
        TemperateDesert,
        ShrubLand,
        Taiga,
        GrassLand,
        TemperateForest,
        TemperateRainForest,
        SubTropicalDesert,
        TropicalSeasonalForest,
        TropicalRainForest
    }

    public static class Terrain
    {
        public static string GetTextureName(TerrainType terrainType)
        {
            return $"{terrainType.ToString().ToLower()}.png";
        }
    }
}