using System;
using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.game.GameSession;

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
        public Dictionary<string, InventoryItem> PrimaryItems { get; set; }
        public Dictionary<string, InventoryItem> SecondaryItems { get; set; }
        public bool SecondaryItemsVisible => SecondaryItems != null;

        public InventoryScene(Dictionary<string, InventoryItem> primaryItems, Dictionary<string, InventoryItem> secondaryItems = null)
        {
            PrimaryItems = primaryItems;
            SecondaryItems = secondaryItems ?? new Dictionary<string, InventoryItem>();
            NeedUpdate = true;
            //Start with primary items only? Or both?
        }

        public void ItemClicked(string itemKey, bool primary)
        {
            //Hmm, clicking an item is easy, but different chocies?
        }

        public void MoveItemClicked(string itemKey, bool primary)
        {
            if (primary)
            {
                MoveItemFromTo(itemKey, PrimaryItems, SecondaryItems);
            }
            else
            {
                MoveItemFromTo(itemKey, SecondaryItems, PrimaryItems);
            }
            NeedUpdate = true;
        }

        public bool NeedUpdate { get; set; }

        public void MoveItemFromTo(string itemKey, Dictionary<string, InventoryItem> fromList,
            Dictionary<string, InventoryItem> toList)
        {
            if (!fromList.ContainsKey(itemKey)) return;
            var item = fromList[itemKey];
            if (item.Count == 1)
            {
                fromList.Remove(itemKey);
            }
            else
            {
                item.Count--;
            }
            if (!toList.ContainsKey(itemKey))
            {
                toList.Add(itemKey, item);
            }
            else
            {
                fromList[itemKey].Count++;
            }
        }

        public override void Update()
        {
            if (NeedUpdate)
            {
                /*
                 * Redraw both lists. What to do with entities? Just throw them away? If they have
                 * references to actions etc, does that work? Do it stupidly 
                 */   
            }
        }
    }

    public class InventoryItem
    {
        public string ItemKey { get; set; }
        public int Count { get; set; }
        public IITem Item { get; set; }
    }

    public class ItemEntity : Entity
    {
        public InventoryItem InventoryItem { get; set; }
        public bool Primary { get; set; }

        public ItemEntity(InventoryItem inventoryItem, bool primary, Action<string, bool> clicked, Action<string, bool> removeClicked)
        {
            InventoryItem = inventoryItem;
            Primary = primary;
            Text itemText = new Text($"{InventoryItem.Item.Name}, {InventoryItem.Count}", 20);
            AddGraphics(itemText);
            AddComponent(new ClickableComponent());
        }


    }

    public class ClickableComponent : Component
    {
    }
}
