using System;
using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.common;

namespace robot.dad.game.Scenes
{
    /// <summary>
    /// Basic Inventory has a list (or two) that it displays. Every item has a count (number of instances of 
    /// that item), and a drop or, if two lists are present, a "send to other list"-button, perhaps. 
    /// 
    /// If there are more than one item in a position of the list (items that stack, sort of), you send one at 
    /// a time or there might be an "all" option.
    /// 
    /// The secondary items below could be used as a drop list, if managing the carachters own items.
    /// 
    /// Active items, how do we use them? Huh?
    /// </summary>
    public class InventoryScene : Scene
    {
        public Action Done { get; set; }

        public InventoryScene(Action done, IEnumerable<IItem> primaryItems, IEnumerable<IItem> secondaryItems = null)
        {
            Done = done;
            ItemManager = new InventoryManager(this, primaryItems, secondaryItems);

        }

        public InventoryManager ItemManager { get; set; }

        public bool NeedUpdate { get; set; }

        public override void Update()
        {
            if (Input.KeyPressed(Key.Down))
            {
                ItemManager.IndexUp();
            }
            if (Input.KeyPressed(Key.Up))
            {
                ItemManager.IndexDown();
            }
            if (Input.KeyPressed(Key.Left))
            {
                ItemManager.PreviousList();
            }
            if (Input.KeyPressed(Key.Right))
            {
                ItemManager.NextList();
            }
            if (Input.KeyPressed(Key.Return))
            {
                ItemManager.TakeCurrentItem();
                //NeedUpdate = true;
            }
            if (Input.KeyPressed(Key.T))
            {
                ItemManager.TrashCurrentItem();
                //NeedUpdate = true;
            }
            if (Input.KeyPressed(Key.R))
            {
                ItemManager.ReturnCurrentItem();
                //NeedUpdate = true;
            }

            if (Input.KeyPressed(Key.K))
            {
                Global.PlayerOne.PlayerCharacter.Inventory =
                    ItemManager.PrimaryList.Inventory.Select(ie => ie.Item).ToList();
                Done?.Invoke();
            }

        }

        private void TakeAllItems()
        {
            NeedUpdate = true;
        }
    }
}
