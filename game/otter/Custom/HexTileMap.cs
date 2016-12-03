using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;

namespace Otter.Custom
{
    public class HexTileMap : Graphic
    {
        public readonly HexGrid HexGrid;
        public readonly Dictionary<CubicHexCoord, HexTileInfo> Hexes;
        private readonly HexAtlas _hexAtlas;
        private readonly TerrainEngine _terrainEngine;
        public HexTileInfo[] VisibleTiles;
        private readonly int _visibleRadius;
        
        public HexTileMap(float hexRadius, int visibleRadius, float scale, HexAtlas atlas, TerrainEngine terrainEngine)
        {
            _visibleRadius = visibleRadius;
            HexGrid = new HexGrid(hexRadius);
            _hexAtlas = atlas;
            _terrainEngine = terrainEngine;
            SetTexture(_hexAtlas.Texture);
            Hexes = new Dictionary<CubicHexCoord, HexTileInfo>();
            Scale = scale;
        }

        public void AddTile(CubicHexCoord coord, string textureName, TerrainInfo terrainInfo)
        {
            Hexes.Add(coord, new HexTileInfo(coord, HexGrid, _hexAtlas.GetAtlasTexture(textureName), terrainInfo));
            NeedsUpdate = true;
        }

        public void AddTile(int x, int y, string textureName, TerrainInfo terrainInfo)
        {
            CubicHexCoord hexCoord = new AxialHexCoord(x, y).ToCubic();
            Hexes.Add(hexCoord, new HexTileInfo(hexCoord, HexGrid, _hexAtlas.GetAtlasTexture(textureName), terrainInfo));
            NeedsUpdate = true;
        }

        protected override void UpdateDrawable()
        {
            base.UpdateDrawable();
            SFMLVertices.Clear();
            foreach (var tile in VisibleTiles)
            {
                //tile.tilemapColor.R = Color.R;
                //tile.tilemapColor.G = Color.G;
                //tile.tilemapColor.B = Color.B;
                //tile.tilemapColor.A = Color.A;
                tile.AppendVertices(SFMLVertices);
            }
        }

        public void Map(int startQ, int startR, int mapHeight, int mapWidth)
        {
            for (int r = startR; r < mapWidth + startR; r++)
            {
                int r_offset = r >> 1;//(int)Math.Floor((double)r / 2); // or r>>1
                for (int q = startQ -r_offset; q < mapHeight - r_offset + startQ; q++)
                {
                    var hexcoord = new CubicHexCoord(q,r, -q-r);
                    if (!Hexes.ContainsKey(hexcoord))
                    {
                        var terrainInfo = _terrainEngine.GetTerrainTypeForCoord(hexcoord.x, hexcoord.y);
                        string textureName = Terrain.GetTextureName(terrainInfo.TerrainType);

                        AddTile(hexcoord, textureName, terrainInfo);
                    }
                }
            }
        }

        public void CreateInitialHexes(float x, float y)
        {
            UpdateVisibleTiles(HexGrid.PointToCubic(new Vec2D(x, y)));
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

