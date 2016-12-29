using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Otter;
using robot.dad.common;
using robot.dad.game.Entities;
using robot.dad.game.Event;
using robot.dad.game.Scenes;

namespace robot.dad.game.SceneManager
{
    /// <summary>
    /// My own custom class for managing scenes and transitions.
    /// </summary>
    public sealed class Manager
    {
        public Scene CurrentScene { get; set; }
        public MainScene MainScene { get; set; }
        public Game GameInstance { get; set; }

        private static readonly Lazy<Manager> Lazy =
            new Lazy<Manager>(() => new Manager());

        public static Manager Instance => Lazy.Value;

        private Manager()
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
            var character = new Character("Analusia", "", 10, 100, 100, 60, 10, new List<IItem>());

            //INventory needs to be a simple list, and counts etc will be handled elsewhere...
            character.Inventory.Add(new CharacterWeapon("Bössan", "En klassisk plundrarbössa", 5, true, 2, 80, 30, "skjuter"));
            Global.PlayerOne.AddCharacter(character);
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

        public void StartCombatSceneFromEvent(TileEvent tileEvent)
        {
            //Use tileevent-thingy
            var table = new RuinEventTable();
            if (table.Result.Any()) //Always true with current setup
                GameInstance.SwitchScene(new CombatScene(GetLoot,
                    new ICombattant[] {new CharacterCombattant(Global.PlayerOne.PlayerCharacter)
                    {
                        Team = "Player"
                    },},
                    table.Result.OfType<ICharacter>().Select(c => new CharacterCombattant(c) { Team = "NPC" })));
            else
                GotoMainScene(); //This should instead go to ruin or whatever comes after the combat...
        }

        public void GetLoot()
        {
            var playerInventory =
                Global.PlayerOne.PlayerCharacter.Inventory.Select(
                    kvp => new InventoryItem(kvp.ItemKey, 1, kvp))
                    .ToDictionary(item => item.ItemKey);

            var loot = Lootables.GetLootFromScavengers(3)
                .Select(tem => new InventoryItem(tem.ItemKey, 1, tem));
            Dictionary<string, InventoryItem> loots = new Dictionary<string, InventoryItem>();
            foreach (var inventoryItem in loot)
            {
                if (!loots.ContainsKey(inventoryItem.ItemKey))
                {
                    loots.Add(inventoryItem.ItemKey, inventoryItem);
                }
                else
                {
                    loots[inventoryItem.ItemKey].Count++;
                }
            }
            GameInstance.SwitchScene(new InventoryScene(GotoMainScene, playerInventory, loots));
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

        public void StartChaseScene(TileEvent tileEvent)
        {
            GameInstance.SwitchScene(new ChaseScene(GotoMainScene, () => StartCombatSceneFromEvent(tileEvent)));
        }
    }
}
