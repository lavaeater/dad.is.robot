using System;

namespace Otter.Extras
{
    public class Clickable : UiElement
    {
        protected Action<Clickable> Clicked { get; }
        protected Rectangle EntityArea;

        public Clickable(Action<Clickable> clicked)
        {
            Clicked = clicked;
        }

        public override void Update()
        {
            if (Input.MouseButtonReleased(MouseButton.Any))
            {
                EntityArea = new Rectangle((int)X, (int)Y, Width, Height); //The rectangle will be updated every update cycle
                if (EntityArea.Contains((int)Input.MouseScreenX, (int)Input.MouseScreenY))
                {
                    Clicked?.Invoke(this); //We might want something else as a parameter, get back to it later.
                }
            }
        }
    }
}