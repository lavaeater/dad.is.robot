using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using Newtonsoft.Json;
using Otter;
using robot.dad.common;
using robot.dad.common.Quest;
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

        private JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            };
            settings.Converters.Add(new CubicJsonConverter());
            return settings;
        }

        private Manager()
        {
            JsonConvert.DefaultSettings = GetSerializerSettings;

            CreateGame();

            if (SaveGameFileExists)
            {
                CreateSession();
                LoadSaveGame();
            }
            else
            {
                CreateSession();
                AddPlayerCharacterToSession();
                CreateMainScene();
            }
        }

        private void AddPlayerCharacterToSession()
        {
            var character = new Character("Analusia", "", 10, 100, 100, 60, 10, new List<IItem>());

            //INventory needs to be a simple list, and counts etc will be handled elsewhere...
            character.Inventory.Add(new CharacterWeapon("Bössan", "En klassisk plundrarbössa", 5, true, 2, 80, 30, "skjuter"));
            AddPlayerCharacterToSession(character);
        }

        private void AddPlayerCharacterToSession(Character character)
        {
            Global.PlayerOne.AddCharacter(character);
        }

        private void LoadSaveGame()
        {
            var saveGameData = File.ReadAllText("savegame.json");
            var saveGame = JsonConvert.DeserializeObject<SaveGame>(saveGameData);
            AddPlayerCharacterToSession(saveGame.PlayerCharacter);
            var player = new PlayerEntity(0.5f, 800, 450, true);
            var scene = new MainScene(player); string atlasFile = "Terrain\\terrain.json";
            var terrainData = File.ReadAllText("Terrain\\TerrainConfig.json");
            var background = new HexBackGround(atlasFile, terrainData, 3, 12);
            scene.AddBackGround(background);
            background.AddEvents(saveGame.Events);
            scene.MovePlayerToHex(saveGame.Position);
            MainScene = scene;
        }

        private void CreateGame()
        {
            var game = new Game("Dad is a Robot", 1600, 900, 60, false);
            game.OnEnd = OnGameEnd;

            GameInstance = game;
        }

        public void OnGameEnd()
        {
            //Save game logic!

            //Need to ignore some properties that are just 

            var saveGame = new SaveGame(Global.PlayerOne.PlayerCharacter, MainScene.BackGround);
            string saveGameJson = JsonConvert.SerializeObject(saveGame);
            File.WriteAllText("savegame.json", saveGameJson);
        }

        public bool SaveGameFileExists => File.Exists("savegame.json");

        public void CreateSession()
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
            var loot = Lootables.GetLootFromScavengers(3);
            if (loot.Any(l => l is IQuestItem))
            {
                var cutScene = new IntroScene(() => GoToLootScene(loot.OfType<IItem>()), loot.OfType<IQuestItem>().First().CutSceneText);
                GameInstance.SwitchScene(cutScene);
            }
            else
            {
                GoToLootScene(loot.OfType<IItem>());
            }
        }

        public void GoToLootScene(IEnumerable<IItem> loot)
        {
            GameInstance.SwitchScene(new InventoryScene(GotoMainScene, Global.PlayerOne.PlayerCharacter.Inventory,
                loot));

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

        public void GotoInventory()
        {
            GameInstance.SwitchScene(new InventoryScene(GotoMainScene, Global.PlayerOne.PlayerCharacter.Inventory));
        }
    }

    public class SaveGame
    {
        public Character PlayerCharacter { get; set; }
        public CubicHexCoord Position { get; set; }
        public List<TileEvent> Events { get; set; }

        public SaveGame()
        {
            
        }

        public SaveGame(Character playerCharacter, HexBackGround map)
        {
            PlayerCharacter = playerCharacter;
            Position = map.CurrentPosition;
            Events = map.MapEntities.Where(kv => kv.Value.IsNotEmpty()).SelectMany(pair => pair.Value).ToList();
        }
    }

    public class CubicJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var coord = (CubicHexCoord)value;
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            serializer.Serialize(writer, coord.x);
            writer.WritePropertyName("y");
            serializer.Serialize(writer, coord.y);
            writer.WritePropertyName("z");
            serializer.Serialize(writer, coord.z);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int x = 0, y = 0, z =0;
            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    break;

                var propertyName = (string)reader.Value;
                if (!reader.Read())
                    continue;

                if (propertyName == "x")
                {
                    x = serializer.Deserialize<int>(reader);
                }
                if (propertyName == "y")
                {
                    y = serializer.Deserialize<int>(reader);
                }
                if (propertyName == "z")
                {
                    z = serializer.Deserialize<int>(reader);
                }
            }
            return new CubicHexCoord(x, y, z);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CubicHexCoord) || objectType == typeof(CubicHexCoord?);
        }
    }
}
