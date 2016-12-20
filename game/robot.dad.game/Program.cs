using System.IO;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.GameSession;
using robot.dad.game.SceneManager;
using robot.dad.game.Scenes;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            //StartGame();
            //StartGame2();

            StartGame3();
            //StartCombat();

            //StartIntro();
        }

        private static void StartGame3()
        {
            //var terrainConfig = TerrainConfigBuilder.BuildTerrainConfig();

            //string json = terrainConfig.ToJson();

            //File.WriteAllText("TerrainConfig.json", json);

            var game = new Game("Dad is a Robot", 1600, 900, 60, true);

            string atlasFile = "Terrain\\terrain.json";

            Global.PlayerOne = CustomSession.AddSession(game, "playerone");
            game.Sessions.Add(Global.PlayerOne);
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
            var player = new PlayerEntity(0.5f, 800, 450, true);
            var scene = new MainScene(player);
            scene.AddBackGround(background);
            game.OnEnd = () => background.SaveMap("map.json");
            var intro = new IntroScene(() => { game.SwitchScene(scene); }, Global.PlayerOne);
            game.Start(intro);
        }

        //private static void StartIntro()
        //{
        //    var game = new Game("IntroScene", 1600, 900, 60, false);
        //    game.DrawInactiveScenes = false;
        //    var scene = new IntroScene(() => { });

        //    game.Start(scene);
        //}

        private static void StartCombat()
        {
            var game = new Game("CombatTest", 1600, 900, 60, false);
            game.DrawInactiveScenes = false;
            var scene = new CombatScene(() =>
            {
                string leif = "";
            });

            game.Start(scene);
        }

        //private static void StartGame()
        //{
        //    //var terrainConfig = TerrainConfigBuilder.BuildTerrainConfig();

        //    //string json = terrainConfig.ToJson();

        //    //File.WriteAllText("TerrainConfig.json", json);

        //    var game = new Game("Dad is a Robot", 1600, 900, 60, false);

        //    string atlasFile = "Terrain\\terrain.json";

        //    Global.PlayerOne = game.AddSession("playerone");
        //    Global.PlayerOne.Controller.AddButton(Controls.Up);
        //    Global.PlayerOne.Controller.AddButton(Controls.Down);
        //    Global.PlayerOne.Controller.AddButton(Controls.Left);
        //    Global.PlayerOne.Controller.AddButton(Controls.Right);

        //    Global.PlayerOne.Controller.Button(Controls.Up).AddKey(Key.Up);
        //    Global.PlayerOne.Controller.Button(Controls.Down).AddKey(Key.Down);
        //    Global.PlayerOne.Controller.Button(Controls.Left).AddKey(Key.Left);
        //    Global.PlayerOne.Controller.Button(Controls.Right).AddKey(Key.Right);

        //    var terrainData = File.ReadAllText("Terrain\\TerrainConfig.json");
        //    var background = new HexBackGround(atlasFile, terrainData, 3, 12);
        //    var player = new PlayerEntity(0.5f, 800, 450, Global.PlayerOne);
        //    var scene = new MainScene(player);
        //    scene.AddBackGround(background);
        //    game.OnEnd = () => background.SaveMap("map.json");
        //    game.Start(scene);
        //}

        //private static void StartGame2()
        //{

        //    var game = new Game("Dad is a Robot", 1600, 900, 60, false);
        //    var manager = new Manager(game);

        //    manager.StartGame();
        //}
    }
}
