using System.Collections.Generic;
using System.Linq;

namespace Otter.Extras
{
    public class Container : UiElement
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public List<UiElement> Children { get; set; } = new List<UiElement>();

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

        public Container(int cols, int cellSpacing, int rowSpacing)
        {
            Cols = cols;
            CellSpacing = cellSpacing;
            RowSpacing = rowSpacing;
            Dirty = true;
        }
        //Contains other UI Elements... how?
        public IEnumerator<UiElement> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        public void Add(UiElement item)
        {
            Scene.Add(item);
            Children.Add(item);
            Dirty = true;
        }

        public void Clear()
        {
            Scene.Remove(Children);
            Children.Clear();
            Dirty = true;
        }

        public bool Contains(UiElement item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(UiElement[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(UiElement item)
        {
            Scene.Remove(item);
            Dirty = true;
            return Children.Remove(item);
        }

        public int Count => Children.Count;

        public bool IsReadOnly => false;

        public int IndexOf(UiElement item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, UiElement item)
        {
            Children.Insert(index, item);
            Dirty = true;
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
            Dirty = true;
        }
       
        public override void Update()
        {
            //The updated needs to sort out a functioning layout 
            //for all children of this entity. This is complex stuff.
            if (Dirty)
            {
                //Something has changed, redo everything... at least the position calculation
                int rowHeight = Children.Select(e => e.Height).Max() + RowSpacing;
                int colWidth = Children.Select(e => e.Width).Max() + CellSpacing;
                Rows = Children.Count / Cols;
                int i = 0;
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        float x = X + (c * colWidth) + CellSpacing;
                        float y = Y + (r * rowHeight) + RowSpacing;
                        Children[i].X = x;
                        Children[i].Y = y;
                        i++;
                    }
                }
            }
        }

        public int CellSpacing { get; set; } = 5;

        public int RowSpacing { get; set; } = 5;
    }
}