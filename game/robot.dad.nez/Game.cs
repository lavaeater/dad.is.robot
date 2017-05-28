using Nez;
using robot.dad.nez.Scenes;

namespace robot.dad.nez
{
    class Game : Core
    {
        protected override void Initialize()
        {
            base.Initialize();
            Window.AllowUserResizing = true;
            Scene.setDefaultDesignResolution(1280, 720, Scene.SceneResolutionPolicy.ShowAllPixelPerfect);
            scene = new TestScene();
        }
    }
}
