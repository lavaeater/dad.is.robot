using Nez;

namespace robot.dad.nez
{
    class Game : Core
    {
        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            scene = new TestScene();
        }
    }

    internal class TestScene : Scene
    {

    }
}
