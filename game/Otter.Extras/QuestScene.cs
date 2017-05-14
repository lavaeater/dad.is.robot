using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otter.Extras
{
    public class QuestScene : SimpleUiScene
    {
        public QuestScene() : base("Quest Scene")
        {
            
        }

        public QuestScene(string title): base(title)
        {
            
        }
    }

    public class SimpleUiScene : Scene
    {
        public string Title { get; }

        public SimpleUiScene(string title) :base()
        {
            Title = title;
        }

        public override void Update()
        {
            //Poll the mouse input!
            if (Input.MouseButtonUp(MouseButton.Any)) //We don't actually care about which button was pressed
            {
                
            }
        }
    }

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
