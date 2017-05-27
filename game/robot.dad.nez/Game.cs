using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

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
        public override void initialize()
        {
            addRenderer(new DefaultRenderer());
            var airshipTexture = content.Load<Texture2D>("Images/airshipfinal");

            var ship = createEntity("airship")
                .addComponent(new Sprite(airshipTexture))
                .transform.setPosition(Screen.center);
        }
    }
}
