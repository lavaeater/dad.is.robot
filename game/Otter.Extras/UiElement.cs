using System;

namespace Otter.Extras
{
    public class UiElement : Entity
    {
        public bool Dirty { get; set; }
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public virtual string Tag { get; set; }
    }

    public class SimpleDialog : UiElement
    {
        public Action<DialogResult> DialogAction { get; set; }

        public void Close()
        {
            if (Scene != null)
            {
                Scene.Remove(this);
            }
        }
    }

    public enum DialogResult
    {
        Ok,
        Cancel
    }
}