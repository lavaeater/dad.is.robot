using System;
using System.Collections.Generic;
using System.Linq;
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
            var game = new Game("InventoryTest", 1800, 900);
            var inventoryList = new List<IItem> {new CountableItem("Money", "Mynt",12, 100), new BasicWeapon(12, "LEif"),};

            var table = new ScavengerLootTable(3);
            var secondaryList = new List<IItem>(table.Result.OfType<IItem>());

            game.Start(new InventoryScene(() => Console.WriteLine("Done!"),inventoryList, secondaryList));
        }

        private static void StartManager()
        {
            Manager.Instance.StartGame();
        }
    }


}
