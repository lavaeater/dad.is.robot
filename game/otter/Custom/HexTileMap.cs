using System.Collections.Generic;
using System.IO;
using System.Xml;
using ca.axoninteractive.Geometry.Hex;
using SFML.Graphics;
using SFML.System;

namespace Otter.Custom
{
    public class HexTileMap : Graphic
    {
        private readonly int _viewPortWidth;
        private readonly int _viewPortHeight;
        private readonly float _hexRadius;
        private HexGrid _hexGrid;
        private Dictionary<CubicHexCoord, HexTileInfo> _hexes;
        private HexAtlas _hexAtlas;

        public HexTileMap(int viewPortWidth, int viewPortHeight, float hexRadius, string atlasFile)
        {
            _viewPortWidth = viewPortWidth;
            _viewPortHeight = viewPortHeight;
            _hexRadius = hexRadius;
            _hexGrid = new HexGrid(_hexRadius);
            _hexAtlas = new HexAtlas(atlasFile);
            SetTexture(_hexAtlas.Texture);
            
            _hexes = new Dictionary<CubicHexCoord, HexTileInfo>();
        }

        public void AddTile(float x, float y)
        {
            CubicHexCoord coord = _hexGrid.PointToCubic(new Vec2D(x, y));
            _hexes.Add(coord, new HexTileInfo(x,y, coord, _hexAtlas.GetAtlasTexture("dirt_01.png")));
            NeedsUpdate = true;
        }

        private IEnumerable<HexTileInfo> VisibleTiles()
        {
            //TODO: Figure out which hexes are showing on screen riiight now.
            return _hexes.Values;
        }

        protected override void UpdateDrawable()
        {
            base.UpdateDrawable();
            SFMLVertices.Clear();

            foreach (var tile in VisibleTiles())
            {
                //tile.tilemapColor.R = Color.R;
                //tile.tilemapColor.G = Color.G;
                //tile.tilemapColor.B = Color.B;
                //tile.tilemapColor.A = Color.A;
                tile.AppendVertices(SFMLVertices);
            }
        }
    }

    public class HexTileInfo
    {
        private readonly float _x;
        private readonly float _y;
        private readonly float _w;
        private readonly float _h;
        public CubicHexCoord HexCoord { get; private set; }
        public HexAtlasTexture Texture { get; private set; }

        public HexTileInfo(float x, float y, CubicHexCoord hexCoord, HexAtlasTexture texture)
        {
            _x = x;
            _y = y;
            HexCoord = hexCoord;
            Texture = texture;
        }

        internal Vertex CreateVertex(int x = 0, int y = 0, int tx = 0, int ty = 0)
        {
            var tileColor = new Color(SFML.Graphics.Color.White);
            return new Vertex(new Vector2f(_x + x, _y + y), tileColor.SFMLColor, new Vector2f(Texture.X + tx, Texture.Y + ty));
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

        /// <summary>
        /// Add another atlas to the collection of textures.  Duplicate names will destroy this.
        /// </summary>
        /// <param name="source">The relative path to the data file.  The png should be in the same directory.</param>
        public HexAtlas Add(string source)
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

