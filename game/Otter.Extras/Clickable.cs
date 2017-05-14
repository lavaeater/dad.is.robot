using System;

namespace Otter.Extras
{
    public class Clickable : UiElement
    {
        protected Action<Clickable> Clicked { get; }
        protected Rectangle EntityArea;

        protected MouseOverState MouseState { get; private set; } = MouseOverState.MouseOut;

        public Clickable(Action<Clickable> clicked)
        {
            Clicked = clicked;
        }

        private void UpdateEntityArea()
        {
            EntityArea = new Rectangle((int) X, (int) Y, Width, Height); //The rectangle will be updated every update cycle
        }

        public virtual void OnMouseOver()
        {
            MouseOver?.Invoke(this);
        }

        public virtual void OnMouseOut()
        {
            MouseOut?.Invoke(this);
        }

        public Action<Clickable> MouseOver { get; set; }
        public Action<Clickable> MouseOut { get; set; }

        public override void Update()
        {
            UpdateEntityArea();
            UpdateMouseOverState();
            if (Input.MouseButtonReleased(MouseButton.Any))
            {
                if (MouseIsOver)
                {
                    Clicked?.Invoke(this); //We might want something else as a parameter, get back to it later.
                }
            }
        }

        public bool MouseIsOver => EntityArea.Contains((int) Input.MouseScreenX, (int) Input.MouseScreenY);

        private void UpdateMouseOverState()
        {
            var currentState = MouseIsOver ? MouseOverState.MouseOver : MouseOverState.MouseOut;
            if (currentState != MouseState)
            {
                UpdateMouseOverState(currentState);
            }
        }

        private void UpdateMouseOverState(MouseOverState newState)
        {
            if(newState == MouseOverState.MouseOver)
                OnMouseOver();
            if (newState == MouseOverState.MouseOut)
                OnMouseOut();
            MouseState = newState;
        }
    }

    public enum MouseOverState
    {
        MouseOver,
        MouseOut
    }
}