using System;
using Otter;
using robot.dad.combat;

namespace robot.dad.game.Scenes
{
    public class CombatScene : Scene
    {
        private readonly Action _returnAction;
        private long _tick = 0;
        private CombatEngine _combatEngine;

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
             _combatEngine = new CombatEngine();
            _combatEngine.StartCombat();

        }

        public override void Update()
        {
            _tick++;
            if (_tick > 100)
                _returnAction();
        }
    }
}