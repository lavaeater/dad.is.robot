using System;
using Otter;

namespace robot.dad.game.Scenes
{
    public class CombatScene : Scene
    {
        private readonly Action _returnAction;
        private long _tick = 0;

        public CombatScene(Action returnAction)
        {
            _returnAction = returnAction;
            BackGroundColor = Color.White;
        }

        public override void Update()
        {
            _tick++;
            if (_tick > 100)
                _returnAction();
        }
    }
}