using System;
using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.common;

namespace robot.dad.game.Scenes
{
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
}