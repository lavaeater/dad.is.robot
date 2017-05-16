using System;

namespace Otter.Extras
{
    public class Container : UiElement
    {
        public void MoveItemUp(UiElement item)
        {
            if (Children.Contains(item))
            {
                int index = Children.IndexOf(item);
                Children.RemoveAt(index);
                index--;
                if (index < 0)
                {
                    Children.Add(item); //the entity is already in the scene...remember this!
                }
                else
                {
                    Children.Insert(index, item); //Item already in the scene...
                }
                Dirty = true;
            }
        }

        public void MoveItemDown(UiElement item)
        {
            if (Children.Contains(item))
            {
                int index = Children.IndexOf(item);
                Children.RemoveAt(index);
                index++;
                if (index > Children.Count - 1)
                {
                    index = 0;
                }
                Children.Insert(index, item);
                Dirty = true;
            }
        }

        public void CopyTo(UiElement[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly => false;

        public int IndexOf(UiElement item)
        {
            return Children.IndexOf(item);
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}