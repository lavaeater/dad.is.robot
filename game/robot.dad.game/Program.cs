using Otter;
using System.Runtime.CompilerServices;
using Otter.Custom;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game("Dad is a Robot", 3200, 1800, 60, true);
            string atlasFile = "Terrain\\terrain.json";
            var hexAtlas = new HexAtlas(atlasFile);
            var hexMap = new HexTileMap(3200, 1800, 65, hexAtlas);
            var terrainEngine = new TerrainEngine(12, 0.01f, 0.1f);

            var width = 3200/120;
            var height = 1800/140;
            var tiles = width*height;

            int x = 0;
            int y = 0;
            for (int i = 0; i < tiles; i++)
            {
                if (i%width == 0)
                {
                    y++;
                    if (y%2 == 0)
                    {
                        x = 0 - y ;
                    }
                    else
                    {
                        x = 0 - y / 2;
                    }
                }
                var terrainType = terrainEngine.GetTerrainTypeForCoord(x, y);
                string textureName = Terrain.GetTextureName(terrainType);

                hexMap.AddTile(x, y, textureName);
                x++;
            }

            var scene = new Scene();
            scene.AddGraphic(hexMap);


            game.Start(scene);

            //var demo = new CombatDemo();
            //demo.StartGame();

            //var noise = new TerrainEngine();
            //noise.OtherNoiseTest();
            //Console.ReadKey();
        }
    }

    public class ImageEntity : Entity
    {
        public ImageEntity(float x, float y, string imagePath) : base(x, y)
        {
            // Create an Image using the path passed in with the constructor
            var image = new Image(imagePath);
            // Center the origin of the Image
            image.CenterOrigin();
            // Add the Image to the Entity's Graphic list.
            AddGraphic(image);
        }

    }
}
