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
        public int Count => Children.Count;

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

        public virtual void Add(UiElement item)
        {
            Scene?.Add(item);
            Children.Add(item);
            Dirty = true;
        }

        public void Clear()
        {
            Scene?.Remove(Children);
            Children.Clear();
            Dirty = true;
        }

        public bool Contains(UiElement item)
        {
            return Children.Contains(item);
        }

        public bool Remove(UiElement item)
        {
            Scene?.Remove(item);
            Dirty = true;
            return Children.Remove(item);
        }

        public void Insert(int index, UiElement item)
        {
            Scene?.Add(item);
            Children.Insert(index, item);
            Dirty = true;
        }

        public void RemoveAt(int index)
        {
            var child = Children[index];
            Remove(child);
        }
    }
}