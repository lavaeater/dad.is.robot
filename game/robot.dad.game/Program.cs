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
            StartInventory();
            //StartManager();
            //StartLoot();
        }

        private static void StartInventory()
        {

            var inventoryList = new ItemInventoryList("MyStuff",
                new IItem[] {new Money(12, 100), new BasicWeapon(12, "LEif"),});
           
            var inventoryManager = new InventoryManager(inventoryList, null, ItemUpdated);

            while (true)
            {
                var keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.P)
                {
                    PrintLists(inventoryManager);
                }
            }
            //var game = new Game("Inventory", 1800, 900, 60, true);

            //var inventory = new Dictionary<string, InventoryItem>()
            //{
            //    { "ExoSkeleton", new InventoryItem("ExoSkeleton", 2, new BasicItem("ExoSkeleton", "Exoskelett", "Styrkeskelett")) },
            //    {"RayGun", new InventoryItem("RayGun", 1, new BasicItem("RayGun", "Strålpistol", "En pistol som skjuter strålar")) }
            //};

            //var scene = new NewInventoryScene();

            //game.Start(scene);
        }

        public static void PrintLists(InventoryManager manager)
        {
            Console.Clear();
            foreach (var list in manager.Lists)
            {
                Console.WriteLine($"{list.ListKey}");
                foreach (var item in list.Inventory)
                {
                    Console.WriteLine($"{item.Name}");
                }
            }
        }

        private static void ItemUpdated(IItem item, ItemAction itemAction)
        {
            Console.WriteLine($"{item.Name} was {itemAction}");
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

    public enum ItemAction
    {
        Added,
        Removed,
        Selected,
        Unselected
    }

    public class InventoryManager
    {
        public InventoryManager(ItemInventoryList primaryList, ItemInventoryList secondaryList = null, Action<IItem, ItemAction> itemUpdated = null)
        {
            PrimaryList = primaryList;
            SecondaryList = secondaryList;
            Lists.Add(PrimaryList);
            if(SecondaryListExists)
                Lists.Add(SecondaryList);
            Lists.Add(TrashCan);
            if (itemUpdated != null)
            {
                ItemUpdated = itemUpdated;
                foreach (var list in Lists)
                {
                    list.ItemUpdated = ItemUpdated;
                }
            }
            UpdateCurrentList();
        }

        public bool SecondaryListExists => SecondaryList != null;
        public int CurrentListIndex { get; set; }= 0;

        public List<ItemInventoryList> Lists { get; set; } = new List<ItemInventoryList>();
        public ItemInventoryList PrimaryList { get; set; } //Always the users list
        public ItemInventoryList SecondaryList { get; set; } //Loot or other player or store list
        public ItemInventoryList TrashCan { get; set; } = new ItemInventoryList("Trashcan"); //There is always a trashcan

        public ItemInventoryList CurrentList { get; set; }

        public Action<IItem, ItemAction> ItemUpdated { get; set; }

        public void PreviousList()
        {
            CurrentListIndex--;
            UpdateCurrentList();
        }

        private void UpdateCurrentList()
        {
            if (CurrentListIndex < 0)
                CurrentListIndex = Lists.Count - 1;
            if (CurrentListIndex >= Lists.Count)
                CurrentListIndex = 0;

            CurrentList = Lists[CurrentListIndex];
        }

        public void TrashCurrentItem()
        {
            if (CurrentList != TrashCan)
            {
                //Remove item from list
                var item = CurrentList.PopCurrentItem(); //Take it from current list
                TrashCan.Add(item);
            }
        }

        public void TakeCurrentItem()
        {
            if (CurrentList != PrimaryList) //Cannot TAKE stuff from owns list
            {
                var item = CurrentList.PopCurrentItem();
                PrimaryList.Add(item);
            }
        }

        public void ReturnCurrentItem()
        {
            if (CurrentList == PrimaryList && SecondaryListExists)
            {
                var item = PrimaryList.PopCurrentItem();
                SecondaryList.Add(item);
            }
        }

        public void NextList()
        {
            CurrentListIndex++;
            UpdateCurrentList();
        }

        public void IndexUp()
        {
            CurrentList.IndexUp();
        }

        public void IndexDown()
        {
            CurrentList.IndexDown();
        }
    }
}
