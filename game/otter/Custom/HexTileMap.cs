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
        private readonly Dictionary<CubicHexCoord, HexTileInfo> _hexes;
        private readonly HexAtlas _hexAtlas;
        private readonly TerrainEngine _terrainEngine;
        private HexTileInfo[] _visibleTiles;
        private readonly int _visibleRadius;
        private EventEngine _eventEngine;

        public HexTileMap(float hexRadius, int visibleRadius, float scale, HexAtlas atlas, TerrainEngine terrainEngine)
        {
            _visibleRadius = visibleRadius;
            HexGrid = new HexGrid(hexRadius);
            _hexAtlas = atlas;
            _terrainEngine = terrainEngine;
            SetTexture(_hexAtlas.Texture);
            _eventEngine = new EventEngine();
            _hexes = new Dictionary<CubicHexCoord, HexTileInfo>();
            Scale = scale;
        }

        public void AddTile(CubicHexCoord coord, string textureName, TerrainInfo terrainInfo)
        {
            _hexes.Add(coord, new HexTileInfo(coord, HexGrid, _hexAtlas.GetAtlasTexture(textureName), terrainInfo));
            NeedsUpdate = true;
        }

        public void AddTile(int x, int y, string textureName, TerrainInfo terrainInfo)
        {
            CubicHexCoord hexCoord = new AxialHexCoord(x, y).ToCubic();
            _hexes.Add(hexCoord, new HexTileInfo(hexCoord, HexGrid, _hexAtlas.GetAtlasTexture(textureName), terrainInfo));
            NeedsUpdate = true;
        }

        protected override void UpdateDrawable()
        {
            base.UpdateDrawable();
            SFMLVertices.Clear();
            foreach (var tile in _visibleTiles)
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
                    if (!_hexes.ContainsKey(hexcoord))
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
                if (!_hexes.ContainsKey(cubicHexCoord))
                {
                    CreateAndAddTileAt(cubicHexCoord);
                }
            }
            _visibleTiles = visibleHexCoords.Select(coord => _hexes[coord]).ToArray();
            NeedsUpdate = true;
        }

        private void CreateAndAddTileAt(CubicHexCoord cubicHexCoord)
        {
            var terrainInfo = _terrainEngine.GetTerrainTypeForCoord(cubicHexCoord.x, cubicHexCoord.y);
            string textureName = Terrain.GetTextureName(terrainInfo.TerrainType);

            var tileEvent = _eventEngine.GetEventForTile(cubicHexCoord.x, cubicHexCoord.y, terrainInfo);

            AddTile(cubicHexCoord, textureName, terrainInfo);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_hexes);
        }
    }
}

