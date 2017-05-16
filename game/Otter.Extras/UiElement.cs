using System;
using System.Collections.Generic;

namespace Otter.Extras
{
    public class UiElement : Entity
    {
        public bool Dirty { get; set; }
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public virtual string Tag { get; set; }
        public List<UiElement> Children { get; set; } = new List<UiElement>();

        public override void Added()
        {
            base.Added();
            Scene.Add(Children);
        }

        public void Remove()
        {
            RemoveSelf();
        }

        public override void Removed()
        {
            base.Removed();
            foreach (var child in Children)
            {
                child.Remove();
            }
        }
    }

    public class SimpleDialog : StaticItemGrid
    {
        public SimpleDialog(Action<DialogResult> dialogAction, int width, int height, string caption) : base(3,3, 50, 5,5)
        {
            DialogAction = dialogAction;
            Caption = caption;
            Width = width;
            Height = height;

            AddChildAt(2,2, new ClickableTextItem("OK", clickable => OnOkClicked()));
            AddChildAt(0,2, new ClickableTextItem("Cancel", clickable => OnCancelClicked()));
        }

        private void OnOkClicked()
        {
            DialogAction?.Invoke(DialogResult.Ok);
            Close();
        }

        private void OnCancelClicked()
        {
            DialogAction?.Invoke(DialogResult.Cancel);
            Close();
        }

        public Action<DialogResult> DialogAction { get; set; }
        public string Caption { get; }

        public void Close()
        {
            Scene?.Remove(this);
        }
    }

    //public class SimpleDialog : UiElement
    //{
    //    public SimpleDialog(Action<DialogResult> dialogAction, int width, int height, string caption)
    //    {
    //        DialogAction = dialogAction;
    //        Caption = caption;
    //        Width = width;
    //        Height = height;

    //        //OkButton = new ClickableTextItem("OK", clickable => OnOkClicked());
    //        //CancelBUtton = new ClickableTextItem("Cancel", clickable => OnCancelClicked());
    //    }

    //    private void OnOkClicked()
    //    {
    //        DialogAction?.Invoke(DialogResult.Ok);
    //        Close();
    //    }

    //    private void OnCancelClicked()
    //    {
    //        DialogAction?.Invoke(DialogResult.Cancel);
    //        Close();
    //    }

    //    public Action<DialogResult> DialogAction { get; set; }
    //    public string Caption { get; }

    //    public void Close()
    //    {
    //        Scene?.Remove(this);
    //    }
    //}

    public enum DialogResult
    {
        Ok,
        Cancel
    }
}