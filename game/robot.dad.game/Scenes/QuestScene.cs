using Otter;
using Otter.Extras;

namespace robot.dad.game.Scenes
{
    public class QuestScene : SimpleUiScene
    {
        public QuestScene() : this("Quest Scene")
        {

        }

        public void Clicked(UiElement item)
        {
            var something = item.Tag;
        }

        public QuestScene(string title) : base(title)
        {
            DynamicText = new TextItem("Again");
            MainContainer = new DynamicItemGrid(6,6)
                .AddChildAt(0,0, new TextItem("Just some static text"))
                .AddChildAt(5,5, DynamicText)
                .AddChildAt(5,0, new ClickableTextItem("Click me", 16, ShowDialog));

            Add(MainContainer);

            //var entityList = new ItemGrid(2, 20, 20);
            //Add(entityList);
            //for (int i = 0; i < 10; i++)5m5
            //{
            //    var item = new ClickableTextItem($"Item {i}", Clicked);
            //    entityList.Add(item);
            //}
        }

        public TextItem DynamicText { get; set; }

        public DynamicItemGrid MainContainer { get; set; }

        public void ShowDialog(Clickable clicked)
        {
            MainContainer.AddChildAt(1, 1, new SimpleDialog(DialogAction, 100, 100, "A dialog"));
        }

        public void DialogAction(DialogResult result)
        {
            if (result == DialogResult.Ok)
            {
                DynamicText.UpdateText("OK Was clicked");
            }
            else
            {
                DynamicText.UpdateText("Cancel was clicked");
            }
        }
    }

    public class TextItem : UiElement
    {
        public TextItem(string text)
        {
            UpdateText(text);
        }

        public void UpdateText(string text)
        {
            ClearGraphics();
            AddGraphic(new RichText(text, new RichTextConfig()
            {
                CharColor = Color.White,
                FontSize = 16,
                TextAlign = TextAlign.Center
            }));
        }
    }

    /*
     * En till roman i sammanhanget... vi har börjat få till en halvfungerande ui-implementation som vi förmodligen
     * kan bygga vidare på för att göra olika delar av ui:t i spelet.
     * 
     * Men var ska vi börja? 
     * 
     * Våra förmågor att åstadkomma saker är väldigt avhängiga att vi faktiskt löser problem. Problemlösning är vårt
     * game. Jag ville göra ett text-baserat ui för att testa questmotorn, här är vi nu vid ett mer grafiskt ui, men målet
     * var att testa quest-motorn. Så då får vi utgå från den problemsfären.
     * 
     * Jag hade några tankar om att ett ui kan kontrollera en state machine. Det är lite najs för då kan vi 
     * definiera en state machine för Quest-hanteringen... man kan tänka sig att en quest, eller själva questandet är 
     * en statemachine som kan befinna sig i lite olika states som vi därför kan återuppta närhelst vi vill.
     * 
     * Kommer allt sluta som statemachines? 
     * 
     * Vad behöver vi kunna göra för att testa quest-motorn? Vi behöver ha ett ui-element som presenterar spelarens position
     * i koordinater. I det systemet kan vi sen köra hela ui-grejen, målet kanske tom är att betrakta alltihop som ett lager 
     * så att det kan agera HUD i själva spelet. Detta ger oss möjlighet att presentera dialoger etc.
     * 
     * SÅ olika ui-element vi behöver idag:
     * 
     * CustomUi-element för att rendera spelarposition (ingen generell implementation utan väldigt specifik)
     * Popup-dialog eller liknande för att ställa frågor - perfekt för "gå in i staden" och "Undersök?"-frågor
     * 
     * Jag tror dialogen är den kraftfullaste.
     * 
     * Sen behöver vi ui för inventory vilket naturligt segway:ar in i character scene / editor / manager
     * 
     */

    //Should be clickable?

    /*
     * Time for a small novel here, then.
     * 
     * We need UI-elements, simplest possible implementation of the same...
     * 
     * They  need to be able to somehow figure out if something has been clicked, how do we do that?
     * The UI should only worry about
     * 1. Displaying ui stuff
     * 2. Passing on the fact that something has happened. 
     * 
     * Also, every quest can actually be seen as dynamically created finite state machine.
     * 
     * Like, either we are working on a quest or it is paused, these are valid states. So, when doing our ui we 
     * want to be able to, very simply, define a ui. This could easily be made with some kind of json holding 
     * the ui data. 
     * 
     * Also, every ui element will have some  kind of abstract notion of "a thing" happening. 
     * 
     * This thing should be handle by the state machine, I think. So logic for displayin is in one place
     * and then the logic for handling events is somewhere else. The more generic and reusable we can make
     * it the better...
     * 
     */


}
