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
        public bool PrimarySelected { get; set; } = true;
        public int SelectedItemIndex { get; set; } = 0;

        public InventoryScene(Dictionary<string, InventoryItem> primaryItems, Dictionary<string, InventoryItem> secondaryItems = null)
        {
            PrimaryItems = primaryItems;
            PrimaryEntities = new SelectableList<ItemEntity>();
            SecondaryItems = secondaryItems ?? new Dictionary<string, InventoryItem>();
            SecondaryEntities = new SelectableList<ItemEntity>();
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
                toList.Add(itemKey, item.Copy());
            }
            else
            {
                toList[itemKey].Count++;
            }
        }

        private SelectableList<ItemEntity> PrimaryEntities { get; set; }
        private SelectableList<ItemEntity> SecondaryEntities { get; set; }

        public override void Update()
        {
            if (NeedUpdate)
            {
                RemoveAll();
                PrimaryEntities.Clear();
                SecondaryEntities.Clear();
                int y = 50;
                int x = 100;
                foreach (var inventoryItem in PrimaryItems)
                {
                    var entity = new ItemEntity(inventoryItem.Value, true, ItemClicked, MoveItemClicked, x, y);
                    PrimaryEntities.Add(entity);
                    y += 20;
                }
                x = 800;
                y = 50;
                foreach (var inventoryItem in SecondaryItems)
                {
                    var entity = new ItemEntity(inventoryItem.Value, false, ItemClicked, MoveItemClicked, x, y);
                    SecondaryEntities.Add(entity);
                    y += 20;
                }
                Add(PrimaryEntities.AsEnumerable<Entity>());
                Add(SecondaryEntities.AsEnumerable<Entity>());
                NeedUpdate = false;
                if (PrimarySelected)
                {
                    PrimaryEntities.ActivateList();
                    SecondaryEntities.DeactivateList();
                }
                else
                {
                    PrimaryEntities.ActivateList();
                    SecondaryEntities.DeactivateList();
                }
            }

            if (Input.KeyPressed(Key.Down))
            {
                MoveIndexUp();
            }
            if (Input.KeyPressed(Key.Up))
            {
                MoveIndexDown();
            }
            if (Input.KeyPressed(Key.Left) || Input.KeyPressed(Key.Right))
            {
                SwitchItemList();
            }
            if (Input.KeyPressed(Key.Return))
            {
                MoveItem();
            }
            if (Input.KeyPressed(Key.A))
            {
                TakeAllItems();
            }

        }

        private void TakeAllItems()
        {
            foreach (var item in SecondaryItems)
            {
                if (PrimaryItems.ContainsKey(item.Key))
                {
                    PrimaryItems[item.Key].Count += item.Value.Count;
                }
                else
                {
                    PrimaryItems.Add(item.Key, item.Value);
                }
            }
            SecondaryItems.Clear();
            NeedUpdate = true;
        }

        private void MoveItem()
        {
            if (PrimarySelected)
            {
                MoveItemClicked(PrimaryEntities.SelectedItem.InventoryItem.ItemKey, true);
                if(PrimaryItems.IsEmpty())
                    SwitchItemList();
            }
            else
            {
                MoveItemClicked(SecondaryEntities.SelectedItem.InventoryItem.ItemKey, false);
                if(SecondaryItems.IsEmpty())
                    SwitchItemList();
            }
        }

        private void MoveIndexDown()
        {
            if (PrimarySelected)
            {
                PrimaryEntities.MoveIndexDown();
            }
            else
            {
                SecondaryEntities.MoveIndexDown();
            }

        }

        private void SwitchItemList()
        {
            if (PrimarySelected && SecondaryEntities.CanActivate)
            {
                PrimaryEntities.DeactivateList();
                SecondaryEntities.ActivateList();
                PrimarySelected = false;
            }
            else if(!PrimarySelected && PrimaryEntities.CanActivate) 
            {
                SecondaryEntities.DeactivateList();
                PrimaryEntities.ActivateList();
                PrimarySelected = true;
            }
        }

        private void MoveIndexUp()
        {
            if (PrimarySelected)
            {
                PrimaryEntities.MoveIndexUp();
            }
            else
            {
                SecondaryEntities.MoveIndexUp();
            }
        }
    }

    public class InventoryItem
    {
        public string ItemKey { get; set; }
        public int Count { get; set; }
        public IITem Item { get; set; }

        public InventoryItem Copy()
        {
            var item = MemberwiseClone() as InventoryItem;
            item.Count = 1;
            return item;
        }


        public InventoryItem()
        {

        }
        public InventoryItem(string itemKey, int count, IITem item)
        {
            ItemKey = itemKey;
            Count = count;
            Item = item;
        }
    }

    public class ItemEntity : Entity, ISelectableEntity
    {
        public InventoryItem InventoryItem { get; set; }
        public bool Primary { get; set; }
        public string InventoryText => $"{InventoryItem.Item.Name}, {InventoryItem.Count}";

        public ItemEntity(InventoryItem inventoryItem, bool primary, Action<string, bool> clicked, Action<string, bool> removeClicked, float x, float y) : base(x, y)
        {
            InventoryItem = inventoryItem;
            Primary = primary;
            RichText itemText = new RichText(InventoryText, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = 16
            });
            AddGraphics(itemText);
        }


        public void Unselect()
        {
            Graphics.Clear();
            Graphics.Add(new RichText(InventoryText, new RichTextConfig() { CharColor = Color.White, FontSize = 16 }));
        }

        public void Select()
        {
            Graphics.Clear();
            Graphics.Add(new RichText(InventoryText, new RichTextConfig() {CharColor = Color.Green, FontSize = 20}));
        }
    }

    public class ClickableComponent : Component
    {
    }

    public class SelectableList<T> : List<T> where T: ISelectableEntity
    {
        public int SelectedIndex { get; set; }
        public T SelectedItem { get; set; }
        public bool CanActivate => this.IsNotEmpty();

        public void RemoveSelectedItem()
        {
            Remove(SelectedItem);
            SetSelectedItem();
        }

        public void DeactivateList()
        {
            SelectedItem?.Unselect();
        }

        public void ActivateList()
        {
            SetSelectedItem();
        }

        private void SetSelectedItem()
        {
            FixIndex();
            if(this.IsNotEmpty())
                SelectedItem = this[SelectedIndex];

            SelectedItem?.Select();

            if (PreviousIndex != -1 && this.IsNotEmpty() && PreviousIndex != SelectedIndex)
                this[PreviousIndex]?.Unselect();
        }

        private void FixIndex()
        {
            if (SelectedIndex >= Count)
                SelectedIndex = 0;

            if (SelectedIndex < 0)
                SelectedIndex = Count - 1;

            if (PreviousIndex >= Count)
                PreviousIndex = 0;
        }

        public void MoveIndexUp()
        {
            PreviousIndex = SelectedIndex;
            SelectedIndex++;
            SetSelectedItem();
        }

        public int PreviousIndex { get; set; } = -1;

        public void MoveIndexDown()
        {
            PreviousIndex = SelectedIndex;
            SelectedIndex--;
            SetSelectedItem();
        }
    }

    public interface ISelectableEntity
    {
        void Select();
        void Unselect();
    }
}
