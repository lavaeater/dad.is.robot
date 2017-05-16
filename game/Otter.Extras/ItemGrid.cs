using System.Linq;

namespace Otter.Extras
{
    public class ItemGrid : Container
    {
        public int Rows { get; set; }
        public int Cols { get; set; }


        public ItemGrid(int cols, int cellSpacing, int rowSpacing)
        {
            Cols = cols;
            CellSpacing = cellSpacing;
            RowSpacing = rowSpacing;
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

        public int CellSpacing { get; set; }

        public int RowSpacing { get; set; }
    }
}