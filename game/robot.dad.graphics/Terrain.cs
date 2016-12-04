namespace robot.dad.graphics
{
    public static class Terrain
    {
        public static string GetTextureName(TerrainType terrainType)
        {
            return $"{terrainType.ToString().ToLower()}.png";
        }
    }
}