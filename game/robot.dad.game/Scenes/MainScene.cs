using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.Event;

namespace robot.dad.game.Scenes
{
    public class MainScene : Scene
    {
        public MainScene(PlayerEntity playerEntity)
        {
            Add(playerEntity);
            CameraFocus = playerEntity;
        }

        public void AddBackGround(HexBackGround background)
        {
            base.Add(background);
            BackGround = background;
        }

        public HexBackGround BackGround { get; set; }

        public void StartChase()
        {
            Game.SwitchScene(new ChaseScene(GetLoot));
        }

        public void GetLoot()
        {
            var playerInventory =
                Global.PlayerOne.PlayerCharacter.Inventory.Select(
                    kvp => new InventoryItem(kvp.Key.ItemKey, kvp.Value, kvp.Key))
                    .ToDictionary(item => item.ItemKey);

            var loot = Lootables.GetLootFromScavengers(3)
                .Select(tem => new InventoryItem(tem.ItemKey, 1, tem));
            Dictionary<string, InventoryItem> loots = new Dictionary<string, InventoryItem>();
            foreach (var inventoryItem in loot)
            {
                if (!loots.ContainsKey(inventoryItem.ItemKey))
                {
                    loots.Add(inventoryItem.ItemKey, inventoryItem);
                }
                else
                {
                    loots[inventoryItem.ItemKey].Count++;
                }
            } 
            Game.SwitchScene(new InventoryScene(ReturnToMain, playerInventory, loots));
        }

        public void ReturnToMain()
        {
            Game.SwitchScene(this);
        }
    }
}