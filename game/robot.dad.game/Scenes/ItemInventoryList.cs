using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Otter;
using rds;
using robot.dad.common;

namespace robot.dad.game.Scenes
{
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
            var valueItem = item as CountableItem;
            if (valueItem != null)
            {
                //We need to add it or find it and add the values together, this is good.
                var existingValue = Inventory.SingleOrDefault(m => m.ItemKey == item.ItemKey) as CountableItem;
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
}