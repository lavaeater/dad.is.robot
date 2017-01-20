using System;
using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.common;

namespace robot.dad.game.Scenes
{
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