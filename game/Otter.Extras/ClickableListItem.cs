using System;

namespace Otter.Extras
{
    public class ClickableListItem : Clickable
    {
        public string Caption { get; }

        public ClickableListItem(string caption, Action<Clickable> clicked) : base(clicked)
        {
            Caption = caption;
            RichText itemText = new RichText(Caption, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = 32
            });
            AddGraphics(itemText);
        }
    }
}