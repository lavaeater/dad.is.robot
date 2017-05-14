namespace Otter.Extras
{
    public class SimpleUiScene : Scene
    {
        public string Title { get; }

        public SimpleUiScene(string title) : base()
        {
            Title = title;
        }

        public override void Update()
        {
        }
    }
}