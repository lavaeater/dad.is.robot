using System.Collections.Generic;
using System.IO;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.GameSession;
using robot.dad.game.Scenes;

namespace robot.dad.game.SceneManager
{
    /// <summary>
    /// My own custom class for managing scenes and transitions.
    /// </summary>
    public class Manager
    {
        public Scene CurrentScene { get; set; }
        public MainScene MainScene { get; set; }
        public static Game GameInstance { get; set; }
 
        public Manager()
        {
            CreateGame();
            CreateSession();
            CreateMainScene();
        }

        private void CreateGame()
        {
            var game = new Game("Dad is a Robot", 1600, 900, 60, false);

            GameInstance = game;
        }

        private void CreateSession()
        {
            Global.PlayerOne = CustomSession.AddSession(GameInstance, "playerone");

            Global.PlayerOne.Controller.AddButton(Controls.Up);
            Global.PlayerOne.Controller.AddButton(Controls.Down);
            Global.PlayerOne.Controller.AddButton(Controls.Left);
            Global.PlayerOne.Controller.AddButton(Controls.Right);

            Global.PlayerOne.Controller.Button(Controls.Up).AddKey(Key.Up);
            Global.PlayerOne.Controller.Button(Controls.Down).AddKey(Key.Down);
            Global.PlayerOne.Controller.Button(Controls.Left).AddKey(Key.Left);
            Global.PlayerOne.Controller.Button(Controls.Right).AddKey(Key.Right);

            Global.PlayerOne.AddCharacter(new Character("Analusia", "", 10, 100, 70, 60, 10, new Dictionary<IITem, int>()));
        }

        public void StartGame()
        {
            var intro = new IntroScene(GotoMainScene);
            GameInstance.Start(intro);
        }

        public void GotoMainScene()
        {
            GameInstance.SwitchScene(MainScene);
        }


        private void CreateMainScene()
        {
            string atlasFile = "Terrain\\terrain.json";
            var terrainData = File.ReadAllText("Terrain\\TerrainConfig.json");
            var background = new HexBackGround(atlasFile, terrainData, 3, 12);
            var player = new PlayerEntity(0.5f, 800, 450, true);
            var scene = new MainScene(player);
            scene.AddBackGround(background);
            MainScene = scene;
        }
    }
}
