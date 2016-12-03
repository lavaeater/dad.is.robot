namespace Otter.Custom
{
    public struct TerrainInfo
    {
        public TerrainInfo(TerrainType terrainType, float elevation, float moisture)
        {
            TerrainType = terrainType;
            Elevation = elevation;
            Moisture = moisture;
        }
        public TerrainType TerrainType;
        public float Elevation;
        public float Moisture;
    }
}