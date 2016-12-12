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

            //Integrate with the combat system.
            /*
             * Just do it. No biggie. Change combat system to do one players moves at a time, which will be 
             * more action packed.
             * 
             * BUUUT start with drawing cards with all players. See your notebook.
             */

        }

        public override void Update()
        {
            _tick++;
            if (_tick > 100)
                _returnAction();
        }
    }
}