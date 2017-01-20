using Otter;
using robot.dad.common;

namespace robot.dad.game.Scenes
{
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
}