using System;
using System.Collections.Generic;
using System.IO;
using Otter;
using System.Runtime.CompilerServices;
using Otter.Custom;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            var terrainConfig = TerrainConfigBuilder.BuildTerrainConfig2();

            string json = terrainConfig.ToJson();

            File.WriteAllText("TerrainConfig.json", json);

            var game = new Game("Dad is a Robot", 1600, 900, 60, false);
            string atlasFile = "Terrain\\terrain.json";

           
            var player = new Player(800, 450, "Sprites\\spaceShips_003.png");

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
            var background = new HexBackGround(atlasFile, terrainData, 5, 15);
            var scene = new MainScene(Global.PlayerOne);
            scene.AddBackGround(background);
            scene.Add(player);
            game.OnEnd = () => background.SaveMap("map.json");
            game.Start(scene);
        }
    }
}
