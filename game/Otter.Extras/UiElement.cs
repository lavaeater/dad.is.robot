namespace Otter.Extras
{
    public class UiElement : Entity
    {
        public bool Dirty { get; set; }
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public virtual string Tag { get; set; }
    }
}