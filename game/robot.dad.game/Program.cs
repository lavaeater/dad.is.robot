using System;
using System.Collections.Generic;
using Otter;
using robot.dad.common;
using robot.dad.game.Event;
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
            //StartLoot();
        }

        private static void StartInventory()
        {
            var game = new Game("Inventory", 1800, 900, 60, true);

            var inventory = new Dictionary<string, InventoryItem>()
            {
                { "ExoSkeleton", new InventoryItem("ExoSkeleton", 2, new BasicItem("ExoSkeleton", "Exoskelett", "Styrkeskelett")) },
                {"RayGun", new InventoryItem("RayGun", 1, new BasicItem("RayGun", "Strålpistol", "En pistol som skjuter strålar")) }
            };

            //var scene = new InventoryScene(inventory);

            //game.Start(scene);
        }

        private static void StartLoot()
        {
            for (int i = 0; i < 20; i++)
            {
                var table = new RuinEventTable();
                Console.WriteLine($"Iteration {i}");
                foreach (var thing in table.Result)
                {
                    Console.WriteLine(thing);
                }
                Console.WriteLine($"Iteration {i} done!");
                Console.WriteLine();
                Console.WriteLine();
                Console.ReadKey();
            }
        }

        private static void StartManager()
        {
            Manager.Instance.StartGame();
        }
    }
}
