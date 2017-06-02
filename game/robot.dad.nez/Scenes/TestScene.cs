using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.UI;
using robot.dad.quest;

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

    /// <summary>
    /// Used as a test-bed for displayin a ui. Later we might want to do the ui as an overlay
    /// over the currently displayed scene, for Inventory and such
    /// </summary>
    internal class QuestUiScene : Scene
    {
        public UICanvas UiCanvas;
        private Table _table;
        private IQuest _quest;
        private Skin _skin;

        public override void initialize()
        {
            _quest = new QuestContext("Hitta din familj", "Hitta din mamma", "Hitta rövaren som sålde halsbandet",
                "Halsbandet i din ägo kom från en plundrare. Han stal den från en gruvpatrons fru.",
                new StateQuesting(), UpdateUi);

            addRenderer(new DefaultRenderer());
            UiCanvas = createEntity("ui")
                .addComponent(new UICanvas());
            UiCanvas.isFullScreen = true;
            UiCanvas.renderLayer = 999;
            _skin = Skin.createDefaultSkin();
            _table = UiCanvas.stage.addElement(new Table());
            _table.setFillParent(true);
            UpdateUi();
        }

        private void UpdateUi()
        {
            _table.clearChildren();

            //Use the _quest object to display the UI.
            //Layoutidea
            /*
             * *****************************
             * In excel?
             * 
             * 
             */

            var dialog = new Dialog(_quest.Title, _skin);
            dialog.addText(
                _quest.CurrentStepTitle);
            dialog.addText(_quest.CurrentStepDescription);

            foreach (var choice in _quest.Choices)
            {
                var b = new Button(_skin);
                b.add(choice.Title);
                
                    b.onClicked += button =>
                    {
                        FireQuestEvent(choice.QuestEvent, dialog);
                    };
                dialog.addButton(b);
            }
        }

        private void FireQuestEvent(IQuestEvent questEvent, Dialog dialog)
        {
            if (questEvent != null)
            {
                //Fire it
            }
            dialog.hide();
            //DO nothing right now!
        }
    }

    
}