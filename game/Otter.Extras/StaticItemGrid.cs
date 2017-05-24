using System;
using System.Collections.Generic;

namespace Otter.Extras
{
    public class DynamicItemGrid : Container
    {
        public DynamicItemGrid(int cols, int rows)
        {
            Cols = cols;
            Rows = rows;
        }
        private bool _fill = true;
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int ItemWidth { get; set; }

        private readonly Dictionary<Tuple<int, int>, UiElement> _elements = new Dictionary<Tuple<int, int>, UiElement>();

        public DynamicItemGrid AddChildAt(int x, int y, UiElement child)
        {
            var key = new Tuple<int, int>(x, y);
            if (!_elements.ContainsKey(key))
            {
                _elements.Add(key, child);
            }
            else
            {
                var previousChild = _elements[key];
                previousChild.Remove();
                Children.Remove(previousChild);
                _elements[key] = child;
            }
            Add(child);
            return this;
        }
        
        public override void Update()
        {
            UpdateSize();

            if (Dirty)
            {
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        float x = X + (c * ItemWidth) + CellSpacing;
                        float y = Y + (r * ItemHeight) + RowSpacing;
                        var key = new Tuple<int, int>(c, r);
                        if (_elements.ContainsKey(key))
                        {
                            var element = _elements[key];
                            element.X = x;
                            element.Y = y;
                        }
                    }
                }
            }
        }

        private void UpdateSize()
        {
            if (_fill)
            {
                Width = Scene.Width;
                Height = Scene.Height;
                ItemWidth = Width / Cols - CellSpacing;
                ItemHeight = Height / Rows - RowSpacing;
            }
        }

        public int RowSpacing { get; set; } = 5;

        public int CellSpacing { get; set; } = 5;

        public int ItemHeight { get; set; }
    }

    public class StaticItemGrid : Container
    {
        public int Rows { get; set; }
        public int ItemSize { get; }
        public int Cols { get; set; }
        private UiElement[][] _childGrid;



        public StaticItemGrid(int cols, int rows, int itemSize, int cellSpacing, int rowSpacing)
        {
            Cols = cols;
            Rows = rows;
            ItemSize = itemSize;
            CellSpacing = cellSpacing;
            RowSpacing = rowSpacing;
            Dirty = true;
            _childGrid = new UiElement[Cols][];
        }

        public void AddChildAt(int x, int y, UiElement child)
        {
            if (_childGrid[x] == null)
            {
                _childGrid[x] = new UiElement[Rows];
            }
            _childGrid[x][y] = child;
            Add(child);
        }

        public override void Update()
        {
            //The updated needs to sort out a functioning layout 
            //for all children of this entity. This is complex stuff.
            if (Dirty)
            {
                int i = 0;
                for (int r = 0; r < Rows; r++)
                {
                    for (int c = 0; c < Cols; c++)
                    {
                        float x = X + (c * ItemSize) + CellSpacing;
                        float y = Y + (r * ItemSize) + RowSpacing;
                        if (_childGrid[c] != null && _childGrid[c][r] != null)
                        {
                            _childGrid[c][r].X = x;
                            _childGrid[c][r].Y = y;
                        }
                        //Children[i].X = x;
                        //Children[i].Y = y;
                        i++;
                    }
                }
            }
        }

        public int CellSpacing { get; set; }

        public int RowSpacing { get; set; }
    }
}