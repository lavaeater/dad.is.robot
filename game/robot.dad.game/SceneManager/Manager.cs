using System;
using System.Data.SqlClient;
using System.IO;
using Appccelerate.StateMachine;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.Scenes;

namespace robot.dad.game.SceneManager
{
    /// <summary>
    /// My own custom class for managing scenes and transitions.
    /// </summary>
    public class Manager : Game
    {
        public Scene CurrentScene { get; set; }
        public MainScene MainScene { get; set; }
        public static Game GameInstance { get; set; }
        public PassiveStateMachine<GameState, GameEvent> StateMachine { get; set; }

        public Manager(Game game)
        {
            GameInstance = game;

            MainScene = CreateMainScene();

            CreateStateMachine();
        }

        public void StartGame()
        {
            StateMachine.Start();
            StateMachine.Fire(GameEvent.StartIntro);
        }

        private void CreateStateMachine()
        {
            StateMachine = new PassiveStateMachine<GameState, GameEvent>();

            StateMachine
                .In(GameState.BeforeStart)
                .On(GameEvent.StartIntro)
                .Goto(GameState.ShowingIntro);

            StateMachine
                .In(GameState.ShowingIntro)
                .ExecuteOnEntry(ShowIntro)
                .On(GameEvent.IntroFinished)
                .Goto(GameState.ExploringTheWorld);

            StateMachine
                .In(GameState.ExploringTheWorld)
                .ExecuteOnEntry(ExploringTheWorld);

            StateMachine.Initialize(GameState.BeforeStart);
        }

        private void ExploringTheWorld()
        {
            string here = "there";
        }

        private void StartGameInstance()
        {
            throw new NotImplementedException();
        }

        private void SetCurrentScene(Scene scene)
        {
            CurrentScene = scene;
            GameInstance.SwitchScene(CurrentScene);
        }

        private void ShowIntro()
        {
            CurrentScene = new IntroScene(IntroFinished);
            GameInstance.Start(CurrentScene);
        }
        
        private void IntroFinished()
        {
            SetCurrentScene(MainScene);
            StateMachine.Fire(GameEvent.IntroFinished);
        }

        private MainScene CreateMainScene()
        {
            string atlasFile = "Terrain\\terrain.json";

            Global.PlayerOne = GameInstance.AddSession("playerone");
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
            var player = new Player(0.5f, 800, 450, Global.PlayerOne);
            var scene = new MainScene(player);
            scene.AddBackGround(background);
            return scene;
        }
    }
}
