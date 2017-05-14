using System.Collections;

namespace Otter.Extras
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
            var entityList = new ItemGrid(2, 20, 20);
            Add(entityList);
            for (int i = 0; i < 10; i++)
            {
                var item = new ClickableTextItem($"Item {i}", Clicked);
                entityList.Add(item);
            }
        }
    }

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
