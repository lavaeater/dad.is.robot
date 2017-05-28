using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace robot.dad.nez.Scenes
{
    internal class TestScene : Scene
    {
        public override void initialize()
        {
            addRenderer(new DefaultRenderer());

            var airshipTexture = content.Load<Texture2D>(Content.Images.airshipfinal);

            var ship = createEntity("airship")
                .addComponent(new Sprite(airshipTexture))
                .transform.setPosition(Screen.center);
        }
    }
}