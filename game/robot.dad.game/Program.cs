using System.Collections.Generic;
using Otter;
using robot.dad.game.GameSession;
using robot.dad.game.SceneManager;
using robot.dad.game.Scenes;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            //StartInventory();
            StartManager();
        }

        private static void StartInventory()
        {
            var game = new Game("Inventory", 1800, 900, 60, true);

            var inventory = new Dictionary<string, InventoryItem>()
            {
                { "ExoSkeleton", new InventoryItem("ExoSkeleton", 2, new BasicItem("ExoSkeleton", "Exoskelett", "Styrkeskelett")) },
                {"RayGun", new InventoryItem("RayGun", 1, new BasicItem("RayGun", "Strålpistol", "En pistol som skjuter strålar")) }
            };

            var scene = new InventoryScene(inventory);

            game.Start(scene);
        }

        private static void StartLoot()
        {
            
        }

        private static void StartManager()
        {
            var manager = new Manager();
            manager.StartGame();
        }
    }
}
