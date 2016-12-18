using System;
using Otter;

namespace robot.dad.game.Scenes
{
    public class BlankScene : Scene
    {
        public Action SomeAction { get; set; }

        public BlankScene(Action someAction)
        {
            SomeAction = someAction;
        }

        public override void Begin()
        {
            SomeAction?.Invoke();
        }
    }
}
