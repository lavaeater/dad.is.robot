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

            var dialog = new Dialog("Quest Motherfucker, do you do it", Skin.createDefaultSkin());
            dialog.addText(
                "Get this text from the Quest-context, I guess? Yes yes! The context is for generating the UI - makes PERFECT sense!");


        }
    }

    /// <summary>
    /// Used as a test-bed for displayin a ui. Later we might want to do the ui as an overlay
    /// over the currently displayed scene, for Inventory and such
    /// </summary>
    internal class QuestUiScene : Scene
    {
        
    }
}