using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.UI;

namespace robot.dad.nez.Scenes
{
    internal class TestScene : Scene
    {
        public UICanvas UiCanvas;
        private Table _table;
        public override void initialize()
        {
            addRenderer(new DefaultRenderer());

            UiCanvas = createEntity("ui")
                .addComponent(new UICanvas());
            UiCanvas.isFullScreen = true;
            UiCanvas.renderLayer = 999;
            SetupUi();


            var airshipTexture = content.Load<Texture2D>(Content.Images.airshipfinal);

            var ship = createEntity("airship")
                .addComponent(new Sprite(airshipTexture))
                .transform.setPosition(Screen.center);
        }

        private void SetupUi()
        {
            _table = UiCanvas.stage.addElement(new Table());
            _table.setFillParent(true);
        }
    }
}