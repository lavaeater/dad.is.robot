using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Otter;
using rds;
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

    public class ItemEntity : Entity
    {
        public IItem Item { get; set; }
        public string InventoryText => $"{Item.Name}";

        public ItemEntity(IItem item, float x, float y) : base(x, y)
        {
            Item = item;
            RichText itemText = new RichText(InventoryText, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = 32
            });
            AddGraphics(itemText);
        }

        public void Unselect()
        {
            Graphics.Clear();
            Graphics.Add(new RichText(InventoryText, new RichTextConfig() { CharColor = Color.White, FontSize = 32 }));
        }

        public void Select()
        {
            Graphics.Clear();
            Graphics.Add(new RichText(InventoryText, new RichTextConfig() { CharColor = Color.Green, FontSize = 32 }));
        }
    }

    public interface ISelectableEntity
    {
        void Select();
        void Unselect();
    }

    public class ItemInventoryList : IList<IItem>
    {
        public string ListKey { get; set; }
        public Action<IItem, ItemAction> ItemUpdated { get; set; }
        public List<IItem> Inventory { get; set; } = new List<IItem>();
        private int _selectedIndex = 0;

        public ItemInventoryList(string listKey, IEnumerable<IItem> items = null, Action<IItem, ItemAction> itemUpdated = null)
        {
            ListKey = listKey;
            ItemUpdated = itemUpdated;
            if (items != null)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }
            UpdateSelectedItem();
        }

        public void IndexUp()
        {
            _selectedIndex++;
            UpdateSelectedItem();
        }

        public void IndexDown()
        {
            _selectedIndex--;
            UpdateSelectedItem();
        }

        private void UpdateSelectedItem()
        {
            if (_selectedIndex < 0)
                _selectedIndex = Inventory.Count - 1;
            if (_selectedIndex >= Inventory.Count)
                _selectedIndex = 0;
            var previousItem = CurrentItem;
            if (previousItem != null)
                ItemUpdated?.Invoke(previousItem, ItemAction.Unselected);
            if (Inventory.IsNotEmpty())
            {
                CurrentItem = this[_selectedIndex];
                ItemUpdated?.Invoke(CurrentItem, ItemAction.Selected);
            }
        }

        public void Add(IItem item)
        {
            var valueItem = item as ThingValue<int>;
            if (valueItem != null)
            {
                //We need to add it or find it and add the values together, this is good.
                var existingValue = Inventory.SingleOrDefault(m => m is ThingValue<int> && m.ItemKey == item.ItemKey) as ThingValue<int>;
                if (existingValue != null)
                    existingValue.Value += valueItem.Value;
                else
                    Inventory.Add(item);
            }
            else
            {
                Inventory.Add(item);
            }
            ItemUpdated?.Invoke(item, ItemAction.Added);
        }

        public void Clear()
        {
            Inventory.Clear();
        }

        public bool Contains(IItem item)
        {
            return Inventory.Contains(item);
        }

        public void CopyTo(IItem[] array, int arrayIndex)
        {
            Inventory.CopyTo(array, arrayIndex);
        }

        public bool Remove(IItem item)
        {
            var removed = Inventory.Remove(item);
            if (removed)
                UpdateSelectedItem();
            ItemUpdated?.Invoke(item, ItemAction.Removed);
            return removed;
        }

        public IItem PopCurrentItem()
        {
            var item = CurrentItem;
            Remove(item);
            return item;
        }

        public int Count => Inventory.Count;

        public bool IsReadOnly => false;
        public int IndexOf(IItem item)
        {
            return Inventory.IndexOf(item);
        }

        public void Insert(int index, IItem item)
        {
            Inventory.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Inventory.RemoveAt(index);
        }

        public IItem this[int index]
        {
            get { return Inventory[index]; }
            set { Inventory[index] = value; }
        }

        public IItem CurrentItem { get; set; }
        public IEnumerator<IItem> GetEnumerator()
        {
            return Inventory.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
        private int _columnWidth;

        public InventoryManager(Scene scene, IEnumerable<IItem> primaryList, IEnumerable<IItem> secondaryList = null, Action<IItem, ItemAction> itemUpdated = null, int columnWidth = 500)
        {
            _columnWidth = columnWidth;
            PrimaryList = new ItemEntityList("Primary", 20, scene, primaryList);
            if (secondaryList != null)
            {
                SecondaryList = new ItemEntityList("Secondary", columnWidth + 20, scene, secondaryList);
            }
            Lists.Add(PrimaryList);
            if (SecondaryListExists)
                Lists.Add(SecondaryList);
            TrashCan = new ItemEntityList("Trashcan", columnWidth * 2 + 20, scene);
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
        public int CurrentListIndex { get; set; } = 0;

        public List<ItemEntityList> Lists { get; set; } = new List<ItemEntityList>();
        public ItemEntityList PrimaryList { get; set; } //Always the users list
        public ItemEntityList SecondaryList { get; set; } //Loot or other player or store list
        public ItemEntityList TrashCan { get; set; } //There is always a trashcan

        public ItemEntityList CurrentList { get; set; }

        public Action<IItem, ItemAction> ItemUpdated { get; set; }
        public IEnumerable<ItemEntity> AllEntities => Lists.SelectMany(list => list.Inventory);

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

    public class ItemEntityList
    {
        public string ListKey { get; set; }
        public Scene Scene { get; set; }
        public Action<IItem, ItemAction> ItemUpdated { get; set; }
        public List<ItemEntity> Inventory { get; set; } = new List<ItemEntity>();
        private int _selectedIndex = 0;
        private readonly float _baseX;
        public IItem CurrentItem { get; set; }
        public ItemEntity CurrentEntity { get; set; }
        public ItemEntityList(string listKey, float baseX, Scene scene, IEnumerable<IItem> items = null, Action<IItem, ItemAction> itemUpdated = null)
        {
            ListKey = listKey;
            Scene = scene;
            _baseX = baseX;
            ItemUpdated = itemUpdated;
            if (items != null)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }
            UpdateCurrentItem();
        }

        public void IndexUp()
        {
            _selectedIndex++;
            UpdateCurrentItem();
        }

        public void IndexDown()
        {
            _selectedIndex--;
            UpdateCurrentItem();
        }

        private void UpdateCurrentItem()
        {
            if (_selectedIndex < 0)
                _selectedIndex = Inventory.Count - 1;
            if (_selectedIndex >= Inventory.Count)
                _selectedIndex = 0;
            var previousItem = CurrentItem;
            if (previousItem != null)
            {
                CurrentEntity.Unselect(); //This is also not null right now
                ItemUpdated?.Invoke(previousItem, ItemAction.Unselected);
            }
            if (Inventory.IsNotEmpty())
            {
                CurrentItem = this[_selectedIndex].Item;
                CurrentEntity = this[_selectedIndex];
                CurrentEntity.Select();
                ItemUpdated?.Invoke(CurrentItem, ItemAction.Selected);
            }
        }

        public void Add(IItem item)
        {
            var countableItem = item as CountableItem;
            if (countableItem != null)
            {
                //We need to add it or find it and add the values together, this is good.
                var existingCountableItem = Inventory.SingleOrDefault(m => m.Item is CountableItem && m.Item.ItemKey == countableItem.ItemKey)?.Item as CountableItem;
                if (existingCountableItem != null)
                    existingCountableItem.Value += countableItem.Value;
                else
                    AddNewEntity(item);
            }
            else
            {
                AddNewEntity(item);
            }
            ItemUpdated?.Invoke(item, ItemAction.Added);
        }

        private void AddNewEntity(IItem item)
        {
            var entity = new ItemEntity(item, _baseX, Inventory.Count * 40 + 40);
            Inventory.Add(entity);
            Scene.Add(entity);
        }

        public void Clear()
        {
            Inventory.Clear();
        }

        public bool Remove(ItemEntity entity)
        {
            var removed = Inventory.Remove(entity);
            if (removed)
            {
                Scene.Remove(entity);
                UpdateCurrentItem();
            }
            ItemUpdated?.Invoke(entity.Item, ItemAction.Removed);
            return removed;
        }

        public IItem PopCurrentItem()
        {
            var entity = CurrentEntity;
            Remove(entity);
            return entity.Item;
        }

        public int Count => Inventory.Count;

        public bool IsReadOnly => false;

        public ItemEntity this[int index]
        {
            get { return Inventory[index]; }
            set { Inventory[index] = value; }
        }
    }
}
