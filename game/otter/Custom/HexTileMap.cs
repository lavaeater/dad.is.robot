using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;

namespace Otter.Custom
{
    public static class Hex
    {
        public static float HexRadius = 69f;
        public static HexGrid Grid => new HexGrid(HexRadius);
    }

    public class HexTileMap : Graphic
    {
        public readonly Dictionary<CubicHexCoord, HexTileInfo> Hexes;
        private readonly HexAtlas _hexAtlas;
        private readonly TerrainEngine _terrainEngine;
        public HexTileInfo[] VisibleTiles;
        private readonly int _visibleRadius;
        
        public HexTileMap(int visibleRadius, float scale, HexAtlas atlas, TerrainEngine terrainEngine)
        {
            _visibleRadius = visibleRadius;
            _hexAtlas = atlas;
            _terrainEngine = terrainEngine;
            SetTexture(_hexAtlas.Texture);
            Hexes = new Dictionary<CubicHexCoord, HexTileInfo>();
            Scale = scale;
        }

        public void AddTile(CubicHexCoord coord, string textureName, TerrainInfo terrainInfo)
        {
            Hexes.Add(coord, new HexTileInfo(coord, _hexAtlas.GetAtlasTexture(textureName), terrainInfo));
            NeedsUpdate = true;
        }

        public void AddTile(int x, int y, string textureName, TerrainInfo terrainInfo)
        {
            CubicHexCoord hexCoord = new AxialHexCoord(x, y).ToCubic();
            Hexes.Add(hexCoord, new HexTileInfo(hexCoord, _hexAtlas.GetAtlasTexture(textureName), terrainInfo));
            NeedsUpdate = true;
        }

        protected override void UpdateDrawable()
        {
            base.UpdateDrawable();
            SFMLVertices.Clear();
            foreach (var tile in VisibleTiles)
            {
                tile.AppendVertices(SFMLVertices);
            }
        }

        public void CreateInitialHexes(float x, float y)
        {
            UpdateVisibleTiles(Hex.Grid.PointToCubic(new Vec2D(x, y)));
        }

        public void UpdateVisibleTiles(CubicHexCoord currentPosition)
        {
            var visibleHexCoords = currentPosition.AreaAround(_visibleRadius);
            foreach (var cubicHexCoord in visibleHexCoords)
            {
                if (!Hexes.ContainsKey(cubicHexCoord))
                {
                    CreateAndAddTileAt(cubicHexCoord);
                }
            }
            VisibleTiles = visibleHexCoords.Select(coord => Hexes[coord]).ToArray();
            NeedsUpdate = true;
        }

        private void CreateAndAddTileAt(CubicHexCoord cubicHexCoord)
        {
            var terrainInfo = _terrainEngine.GetTerrainTypeForCoord(cubicHexCoord.x, cubicHexCoord.y);
            string textureName = Terrain.GetTextureName(terrainInfo.TerrainType);

            AddTile(cubicHexCoord, textureName, terrainInfo);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Hexes);
        }
    }
}

