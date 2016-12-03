using ca.axoninteractive.Geometry.Hex;
using SFML.Graphics;
using SFML.System;

namespace Otter.Custom
{
    public class HexTileInfo
    {
        public readonly TerrainInfo TerrainInfo;
        private Vec2D _position;
        public CubicHexCoord HexCoord { get; private set; }
        public HexAtlasTexture Texture { get; private set; }

        public HexTileInfo(CubicHexCoord hexCoord, HexAtlasTexture texture, TerrainInfo terrainInfo)
        {
            TerrainInfo = terrainInfo;
            _position = Hex.Grid.CubicToPoint(hexCoord);
            HexCoord = hexCoord;
            Texture = texture;
        }

        internal Vertex CreateVertex(int x = 0, int y = 0, int tx = 0, int ty = 0)
        {
            var tileColor = new Color(SFML.Graphics.Color.White);
            return CreateVertex(new Vector2f(_position.x + x, _position.y + y), tileColor, new Vector2f(Texture.X + tx, Texture.Y + ty));
        }

        internal Vertex CreateVertex(Vector2f position, Color color, Vector2f textureArea)
        {
            return new Vertex(position, color.SFMLColor, textureArea);
        }

        internal void AppendVertices(VertexArray array)
        {
            array.Append(CreateVertex(0, 0, 0, 0)); //upper-left
            array.Append(CreateVertex(Texture.Width, 0, Texture.Width, 0)); //upper-right
            array.Append(CreateVertex(Texture.Width, Texture.Height, Texture.Width, Texture.Height)); //lower-right
            array.Append(CreateVertex(0, Texture.Height, 0, Texture.Height)); //lower-left
        }
    }
}