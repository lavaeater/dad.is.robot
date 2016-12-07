using System;
using ca.axoninteractive.Geometry.Hex;
using robot.dad.graphics;
using Simplex;

namespace robot.dad.game
{
    public class EventEngine
    {
        private readonly Noise _eventNoise;
        private readonly float _scale;
        private readonly Random _rand;
        private int _maxVal;

        public EventEngine()
        {
            _eventNoise = new Noise(42);
            _scale = 0.015f;
            _rand = new Random(32);
            _maxVal = 0;
        }

        public TileEvent GetEventForTile(CubicHexCoord coord, TerrainInfo terrainType)
        {
            int diceRoll = _rand.Next(1, 101);//
            //int diceRoll = (int)_eventNoise.CalcPixel3D(coord.x, coord.y, 0, _scale).ForceRange(100, 1);
            if (diceRoll > _maxVal)
                _maxVal = diceRoll;
            if (terrainType.TerrainType == TerrainType.TemperateDesert || terrainType.TerrainType == TerrainType.SubTropicalDesert || terrainType.TerrainType == TerrainType.Scorched)
            {
                if (99 < diceRoll && diceRoll <= 100)
                {
                    return new TileEvent("Ruin", coord); //More thinking required!
                }
            }

            return null; // null means nothing happens here. Deal with it!
        }


        /*
         *         private void RenderEvents(VertexArray array)
        {
            if (TerrainInfo.TerrainType == TerrainType.Beach)
            {
                //Create vertices from hardcoded data!
                //{beigeBuilding.png  X: 240 Y: 0 Width: 52 Height: 60 FrameX: 0 FrameY: 0 FrameWidth: 0 FrameHeight: 0}
                var buildingTexture = new HexAtlasTexture()
                {
                    X = 240,
                    Y = 0,
                    Width = 52,
                    Height = 60
                };
//                return CreateVertex(new Vector2f(_position.x + x, _position.y + y), tileColor, new Vector2f(Texture.X + tx, Texture.Y + ty));

                var tileColor = new Color(SFML.Graphics.Color.White);
                float buildingX = _position.x + (Texture.Width - buildingTexture.Width)/2;
                float buildingY = _position.y + (Texture.Height - buildingTexture.Height) / 2;
                array.Append(CreateVertex(new Vector2f(buildingX, buildingY), tileColor, new Vector2f(buildingTexture.X, buildingTexture.Y))); //upper-left
                array.Append(CreateVertex(new Vector2f(buildingX + buildingTexture.Width, buildingY), tileColor, new Vector2f(buildingTexture.X + buildingTexture.Width, buildingTexture.Y))); //upper-right
                array.Append(CreateVertex(new Vector2f(buildingX + buildingTexture.Width, buildingY + buildingTexture.Height), tileColor, new Vector2f(buildingTexture.X + buildingTexture.Width, buildingTexture.Y + buildingTexture.Height))); //lower-right
                array.Append(CreateVertex(new Vector2f(buildingX, buildingY + buildingTexture.Height), tileColor, new Vector2f(buildingTexture.X, buildingTexture.Y + buildingTexture.Height))); //lower-left
            }
            //Should render a completely different texture, so let's add one from texturepacker
        }
         */
    }
}