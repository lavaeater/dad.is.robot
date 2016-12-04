using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using Otter;

namespace robot.dad.graphics
{
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
            foreach (JObject frame in frames)
            {
                var name = (string)frame["filename"];
                var textureData = frame["frame"];
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
}