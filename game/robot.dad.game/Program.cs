using System.IO;
using Otter;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            //var terrainConfig = TerrainConfigBuilder.BuildTerrainConfig();

            //string json = terrainConfig.ToJson();

            //File.WriteAllText("TerrainConfig.json", json);

            var game = new Game("Dad is a Robot", 1600, 900, 60, false);
            string atlasFile = "Terrain\\terrain.json";

            Global.PlayerOne = game.AddSession("playerone");
            Global.PlayerOne.Controller.AddButton(Controls.Up);
            Global.PlayerOne.Controller.AddButton(Controls.Down);
            Global.PlayerOne.Controller.AddButton(Controls.Left);
            Global.PlayerOne.Controller.AddButton(Controls.Right);

            Global.PlayerOne.Controller.Button(Controls.Up).AddKey(Key.Up);
            Global.PlayerOne.Controller.Button(Controls.Down).AddKey(Key.Down);
            Global.PlayerOne.Controller.Button(Controls.Left).AddKey(Key.Left);
            Global.PlayerOne.Controller.Button(Controls.Right).AddKey(Key.Right);


            var terrainData = File.ReadAllText("Terrain\\TerrainConfig.json");
            var background = new HexBackGround(atlasFile, terrainData, 3, 12);
            var player = new Player(800, 450, "Sprites\\spaceShips_003.png", Global.PlayerOne);
            var scene = new MainScene(player);
            scene.AddBackGround(background);
            game.OnEnd = () => background.SaveMap("map.json");
            game.Start(scene);
        }
    }
}
