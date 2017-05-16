using System;

namespace Otter.Extras
{
    public class SimpleDialog : StaticItemGrid
    {
        public SimpleDialog(Action<DialogResult> dialogAction, int width, int height, string caption) : base(3,3, 50, 5,5)
        {
            DialogAction = dialogAction;
            Caption = caption;
            Width = width;
            Height = height;

            AddChildAt(2,2, new ClickableTextItem("OK",16, clickable => OnOkClicked()));
            AddChildAt(0,2, new ClickableTextItem("Cancel",16, clickable => OnCancelClicked()));
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
}