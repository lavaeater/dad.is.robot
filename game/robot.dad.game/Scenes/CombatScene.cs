using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            Protagonists = CombatDemo.Protagonists;
            Antagonists = CombatDemo.Antagonists;
             _combatEngine = new CombatEngine(Protagonists, Antagonists);

            //_combatEngine.StartCombat();

            float startX = 50;
            float startY = 50;
            float width = 150;
            float height = width*1.25f;
            foreach (var antagonist in Antagonists)
            {
                Add(new CombattantCard(antagonist, startX, startY, width, height));
                startY += height + 30;
            }

            startY = 50;
            startX = 300;
            foreach (var antagonist in Antagonists)
            {
                Add(new CombattantCard(antagonist, startX, startY, width, height));
                startY += height + 30;
            }
        }

        public List<Combattant> Antagonists { get; set; }

        public List<Combattant> Protagonists { get; set; }

        public override void Update()
        {
            _tick++;
            if (_tick > 100)
                _returnAction();
        }
    }

    public class CombattantCard:Entity
    {
        public CombattantCard(Combattant combattant, float x, float y, float width, float height)
        {
            Combattant = combattant;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Combattant Combattant { get; set; }
        public float Height { get; set; }

        public float Width { get; set; }

        public override void Render()
        {
            Draw.Rectangle(X, Y, Width, Height, Color.Blue, Color.Red, 0.2f);
        }
    }
}