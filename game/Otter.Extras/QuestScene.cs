using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        }
    }

    public class UiElement : Entity
    {
        
    }

    //Should be clickable?
    public class Container : UiElement, IList<UiElement>
    {
        public int Rows { get; set; }
        public int Cols { get; set; }

        public List<UiElement> Children { get; set; } = new List<UiElement>();
        public Container(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
        }
        //Contains other UI Elements... how?
        public IEnumerator<UiElement> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(UiElement item)
        {
            Scene.Add(item);
            Children.Add(item);
        }

        public void Clear()
        {
            Scene.Remove(Children);
            Children.Clear();
        }

        public bool Contains(UiElement item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(UiElement[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(UiElement item)
        {
            Scene.Remove(item);
            return Children.Remove(item);
        }

        public int Count => Children.Count;

        public bool IsReadOnly => false;

        public int IndexOf(UiElement item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, UiElement item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public UiElement this[int index]
        {
            get => Children[index];
            set => Children[index] = value;
        }

        public override void Update()
        {
            //The updated needs to sort out a functioning layout 
            //for all children of this entity. This is complex stuff.
        }
    }

    public class Clickable : UiElement
    {
        protected Action<Clickable> Clicked { get; }
        protected int Width { get; set; } = 100;
        protected int Height { get; set; } = 100;
        private Rectangle _entityArea;

        public Clickable(Action<Clickable> clicked)
        {
            Clicked = clicked;
        }

        public override void Update()
        {
            if (Input.MouseButtonReleased(MouseButton.Any))
            {
                _entityArea = new Rectangle((int)X, (int)Y, Width, Height ); //The rectangle will be updated every update cycle
                if (_entityArea.Contains((int) Input.MouseScreenX, (int) Input.MouseScreenY)) ;
                {
                    Clicked?.Invoke(this); //We might want something else as a parameter, get back to it later.
                }
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
