using System;

namespace Otter.Extras
{
    public class ClickableTextItem : Clickable
    {
        public string Caption { get; }
        public int TextSize { get; }
        public override string Tag => Caption;

        public ClickableTextItem(string caption, int textSize, Action<Clickable> clicked) : base(clicked)
        {
            Caption = caption;
            TextSize = textSize;
            RichText itemText = new RichText(Caption, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = TextSize,
                TextAlign = TextAlign.Center
            });
            Width = itemText.Width;
            Height = itemText.Height;
            AddGraphics(itemText);
        }

        public override void OnMouseOver()
        {
            base.OnMouseOver();
            ClearGraphics();
            RichText itemText = new RichText(Caption, new RichTextConfig()
            {
                CharColor = Color.Green,
                FontSize = TextSize,
                TextAlign = TextAlign.Center
            });
            Width = itemText.Width;
            Height = itemText.Height;
            AddGraphics(itemText);

        }

        public override void OnMouseOut()
        {
            base.OnMouseOut();
            ClearGraphics();
            RichText itemText = new RichText(Caption, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = TextSize,
                TextAlign = TextAlign.Center
            });
            Width = itemText.Width;
            Height = itemText.Height;
            AddGraphics(itemText);
        }
    }
}