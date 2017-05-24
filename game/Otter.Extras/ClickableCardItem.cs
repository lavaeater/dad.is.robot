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
}