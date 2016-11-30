using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Xml;
using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFML.Graphics;
using SFML.System;

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

        public HexTileMap(float hexRadius, int visibleRadius, float scale, HexAtlas atlas, TerrainEngine terrainEngine)
        {
            _visibleRadius = visibleRadius;
            HexGrid = new HexGrid(hexRadius);
            _hexAtlas = atlas;
            _terrainEngine = terrainEngine;
            SetTexture(_hexAtlas.Texture);

            _hexes = new Dictionary<CubicHexCoord, HexTileInfo>();
            Scale = scale;
        }

        public void AddTile(CubicHexCoord coord, string textureName)
        {
            _hexes.Add(coord, new HexTileInfo(coord, HexGrid, _hexAtlas.GetAtlasTexture(textureName)));
            NeedsUpdate = true;
        }

        public void AddTile(int x, int y, string textureName)
        {
            CubicHexCoord hexCoord = new AxialHexCoord(x, y).ToCubic();
            _hexes.Add(hexCoord, new HexTileInfo(hexCoord, HexGrid, _hexAtlas.GetAtlasTexture(textureName)));
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
                        var terrainType = _terrainEngine.GetTerrainTypeForCoord(hexcoord.x, hexcoord.y);
                        string textureName = Terrain.GetTextureName(terrainType);

                        AddTile(hexcoord, textureName);
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
                    CreateAndAtTileAt(cubicHexCoord);
                }
            }
            _visibleTiles = visibleHexCoords.Select(coord => _hexes[coord]).ToArray();
            NeedsUpdate = true;
        }

        private void CreateAndAtTileAt(CubicHexCoord cubicHexCoord)
        {
            var terrainType = _terrainEngine.GetTerrainTypeForCoord(cubicHexCoord.x, cubicHexCoord.y);
            string textureName = Terrain.GetTextureName(terrainType);

            AddTile(cubicHexCoord, textureName);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_hexes);
        }
    }

    public class HexTileInfo
    {
        private readonly HexGrid _hexGrid;
        private readonly float _w;
        private readonly float _h;
        private Vec2D _position;
        public CubicHexCoord HexCoord { get; private set; }
        public HexAtlasTexture Texture { get; private set; }

        public HexTileInfo(CubicHexCoord hexCoord, HexGrid hexGrid, HexAtlasTexture texture)
        {
            _position = hexGrid.CubicToPoint(hexCoord);
            _hexGrid = hexGrid;
            HexCoord = hexCoord;
            Texture = texture;
        }

        internal Vertex CreateVertex(int x = 0, int y = 0, int tx = 0, int ty = 0)
        {
            var tileColor = new Color(SFML.Graphics.Color.White);
            return new Vertex(new Vector2f(_position.x + x, _position.y + y), tileColor.SFMLColor, new Vector2f(Texture.X + tx, Texture.Y + ty));
        }

        internal void AppendVertices(VertexArray array)
        {
            array.Append(CreateVertex(0, 0, 0, 0)); //upper-left
            array.Append(CreateVertex(Texture.Width, 0, Texture.Width, 0)); //upper-right
            array.Append(CreateVertex(Texture.Width, Texture.Height, Texture.Width, Texture.Height)); //lower-right
            array.Append(CreateVertex(0, Texture.Height, 0, Texture.Height)); //lower-left
        }
    }


    public class HexAtlas
    { 
        #region Private Fields

        readonly Dictionary<string, HexAtlasTexture> _subtextures = new Dictionary<string, HexAtlasTexture>();
        public Texture Texture { get; private set; }
        public IEnumerable<string> TextureNames => _subtextures.Keys;

        #endregion

        #region Constructors

        /// <summary>
        /// Designed for Sparrow/Starling exporting from TexturePacker http://www.codeandweb.com/texturepacker
        /// </summary>
        /// <param name="source">The reltive path to the atlas data file.  The png should also be in the same directory.</param>
        public HexAtlas(string source = "")
        {
            if (source != "")
            {
                Add(source);
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Get an AtlasTexture by name.
        /// </summary>
        /// <param name="name">The name of the image in the atlas data.</param>
        /// <returns>An AtlasTexture.</returns>
        public HexAtlasTexture this[string name] => GetTexture(name);


        #endregion

        #region Public Methods

        public HexAtlas AddXml(string source)
        {
            var xml = new XmlDocument();
            xml.Load(source);

            foreach (XmlElement a in xml.GetElementsByTagName("TextureAtlas"))
            {
                string imagePath = a.AttributeString("imagePath");
                imagePath = Path.Combine(Path.GetDirectoryName(source), imagePath);
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    Texture = new Texture(imagePath, true);
                }
                foreach (XmlElement e in xml.GetElementsByTagName("SubTexture"))
                {
                    var name = e.AttributeString("name");

                    if (!_subtextures.ContainsKey(name))
                    {
                        var atext = new HexAtlasTexture
                        {
                            X = e.AttributeInt("x"),
                            Y = e.AttributeInt("y"),
                            Width = e.AttributeInt("width"),
                            Height = e.AttributeInt("height"),
                            Name = name
                        };
                        //atext.FrameHeight = e.AttributeInt("frameHeight", atext.Height);
                        //atext.FrameWidth = e.AttributeInt("frameWidth", atext.Width);
                        //atext.FrameX = e.AttributeInt("frameX", 0);
                        //atext.FrameY = e.AttributeInt("frameY", 0);
                        _subtextures.Add(e.AttributeString("name"), atext);
                    }
                }
            }
            return this;

        }

        /// <summary>
        /// Add another atlas to the collection of textures.  Duplicate names will destroy this.
        /// </summary>
        /// <param name="source">The relative path to the data file.  The png should be in the same directory.</param>
        public HexAtlas Add(string source)
        {
            if (Path.GetExtension(source).Contains("xml"))
            {
                return AddXml(source);
            }
            else if (Path.GetExtension(source).Contains("json"))
            {
                return AddJson(source);
            }
            throw new NotSupportedException("The file format is not supported");
        }

        private HexAtlas AddJson(string source)
        {
            var json = File.ReadAllText(source);
            string folder = Path.GetDirectoryName(source);
            JObject data = JObject.Parse(json);
            var textureFilePath = (string) data["meta"]["image"];
            Texture = new Texture(Path.Combine(folder, textureFilePath), true);
            var frames = data["frames"];
            foreach (JProperty frame in frames)
            {
                var name = frame.Name;
                var textureData = frame.Children()["frame"].First();
                if (!_subtextures.ContainsKey(name))
                {
                    var atext = new HexAtlasTexture
                    {
                        Name = name,
                        X = (int)textureData["x"],
                        Y = (int)textureData["y"],
                        Width = (int)textureData["w"],
                        Height= (int)textureData["h"]
                    };
                    _subtextures.Add(name, atext);
                }
            }
            return this;
        }

        /// <summary>
        /// Add multiple sources to the Atlas.
        /// </summary>
        /// <param name="sources">The file path to the sources.</param>
        /// <returns>The Atlas.</returns>
        public HexAtlas AddMultiple(params string[] sources)
        {
            foreach (string s in sources)
            {
                Add(s);
            }
            return this;
        }

        /// <summary>
        /// Add multiple atlases from a set created by texture packer.
        /// Note: This only supports up to 10 atlases (0 - 9)
        /// </summary>
        /// <param name="source">The path until the number.  For example: "assets/atlas" if the path is "assets/atlas0.xml"</param>
        /// <param name="extension">The extension of the source without a dot</param>
        /// <returns>The Atlas.</returns>
        public HexAtlas AddNumbered(string source, string extension = "xml")
        {
            var i = 0;

            while (File.Exists(source + i + "." + extension))
            {
                Add(source + i + "." + extension);
                i++;
            }

            return this;
        }

        /// <summary>
        /// Get an AtlasTexture by name.
        /// </summary>
        /// <param name="name">The name of the image in the atlas data.</param>
        /// <returns>An AtlasTexture.</returns>
        public HexAtlasTexture GetAtlasTexture(string name)
        {
            return GetTexture(name);
        }

        /// <summary>
        /// Tests if a texture by the specified name exists in the atlas data.
        /// </summary>
        /// <param name="name">The name of the texture to test.</param>
        /// <returns>True if the atlas data contains a texture by the specified name.</returns>
        public bool Exists(string name)
        {
            return _subtextures.ContainsKey(name);
        }

        #endregion

        #region Internal

        internal HexAtlasTexture GetTexture(string name)
        {
            var a = _subtextures[name];

            return a;
        }

        #endregion
    }

    public class HexAtlasTexture
    {

        #region Public Fields

        /// <summary>
        /// The X position of the texture in the atlas.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y position of the texture in the atlas.
        /// </summary>
        public int Y;

        /// <summary>
        /// The width of the texture in the atlas.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of the texture in the atlas.
        /// </summary>
        public int Height;

        /// <summary>
        /// The frame width of the texture in the atlas (used in Trim mode.)
        /// Trim is not supported yet.
        /// </summary>
        public int FrameWidth;

        /// <summary>
        /// The frame height of the texture in the atlas (used in Trim mode.)
        /// Trim is not supported yet.
        /// </summary>
        public int FrameHeight;

        /// <summary>
        /// The frame X position of the texture in the atlas (used in Trim mode.)
        /// Trim is not supported yet.
        /// </summary>
        public int FrameX;

        /// <summary>
        /// The frame Y position of the texture in the atlas (used in Trim mode.)
        /// Trim is not supported yet.
        /// </summary>
        public int FrameY;

        /// <summary>
        /// The name of the texture in the atlas.
        /// </summary>
        public string Name;

        #endregion

        #region Public Properties

        /// <summary>
        /// The rectangle region of the texture in the atlas.
        /// </summary>
        public Rectangle Region
        {
            get
            {
                return new Rectangle(X, Y, Width, Height);
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name + " " + " X: " + X + " Y: " + Y + " Width: " + Width + " Height: " + Height + " FrameX: " + FrameX + " FrameY: " + FrameY + " FrameWidth: " + FrameWidth + " FrameHeight: " + FrameHeight;
        }

        #endregion

    }

}

