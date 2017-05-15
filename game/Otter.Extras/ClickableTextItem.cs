using System;

namespace Otter.Extras
{
    public class ClickableCardItem : Clickable
    {
        public ClickableCardItem(Action<Clickable> clicked, Graphic graphic) : base(clicked)
        {
            AddGraphic(graphic);
        }
    }

    public class ClickableTextItem : Clickable
    {
        public string Caption { get; }
        public override string Tag => Caption;

        public ClickableTextItem(string caption, Action<Clickable> clicked) : base(clicked)
        {
            Caption = caption;
            RichText itemText = new RichText(Caption, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = 32,
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
                FontSize = 32,
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
                FontSize = 32,
                TextAlign = TextAlign.Center
            });
            Width = itemText.Width;
            Height = itemText.Height;
            AddGraphics(itemText);
        }
    }
}