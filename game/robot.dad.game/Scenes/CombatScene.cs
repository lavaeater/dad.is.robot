using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach (var protagonist in Protagonists)
            {
                protagonist.MovePicker = GraphicalPicker;
            }
            Antagonists = CombatDemo.Antagonists;
            _combatEngine = new CombatEngine(Protagonists, Antagonists);

            //_combatEngine.StartCombat();

            float startX = 50;
            float startY = 50;
            float width = 250;
            float height = width * 1.25f;
            foreach (var protagonist in Protagonists)
            {
                Add(new CombattantCard(protagonist, startX, startY, width, height));
                startY += height + 30;
            }

            startY = 50;
            startX = 800;
            foreach (var antagonist in Antagonists)
            {
                Add(new CombattantCard(antagonist, startX, startY, width, height));
                startY += height + 30;
            }
            _combatEngine.StartCombat();
        }

        public List<Combattant> Antagonists { get; set; }

        public List<Combattant> Protagonists { get; set; }

        public override void Update()
        {
            //_tick++;
            //if (_tick > 100)
            //    _returnAction();
        }

        public void GraphicalPicker(Combattant picker, IEnumerable<Combattant> possibleTargets, List<CombatMove> possibleMoves, Action picked)
        {
            //1. Find card
            CombattantCard card = GetEntities<CombattantCard>().Single(cc => cc.Combattant == picker);

            //2. Put it in "picking mode"
            card.Mode = CardMode.Picking;

            //3. Wait for input... like a click on something? Read on ze internet
            picked();
        }


    }

    public class GraphicalPicker : MovePickerBase
    {
        public CombatScene Scene { get; set; }

        public GraphicalPicker(Action donePicking, CombatScene scene) : base(donePicking)
        {
            Scene = scene;
        }

        public override void PickMove(Combattant attacker, IEnumerable<Combattant> possibleTargets)
        {
            //1. Find card
            CombattantCard card = Scene.GetEntities<CombattantCard>().Single(cc => cc.Combattant == attacker);
            
            //2. Put it in "picking mode"
            card.Mode = CardMode.Picking;

            //3. Wait for input... like a click on something? Read on ze internet
            
            base.PickMove(attacker, possibleTargets);
        }
    }

    public enum CardMode
    {
        Picking
    }

    public class CombattantCard : Entity
    {
        public CombattantCard(Combattant combattant, float x, float y, float width, float height)
        {
            Combattant = combattant;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Moves = new List<Entity>();

            float moveX = X + Width;
            float moveY = Y;
            foreach (var combatMove in Combattant.CombatMoves)
            {
                Moves.Add(new MoveEntity(combatMove, moveX, moveY));
                moveY += 50f; //should be some variable, height or something
            }
        }

        public override void Added()
        {
            //Add entities for all the attacks to the scene!
            Scene.Add(Moves);
        }

        public List<Entity> Moves { get; set; }

        public Combattant Combattant { get; set; }
        public float Height { get; set; }

        public float Width { get; set; }
        public CardMode Mode { get; set; }

        public override void Render()
        {
            Draw.Rectangle(X, Y, Width, Height, Color.Blue, Color.Red, 0.2f);
            Draw.Text(Combattant.Name, 30, X + 5, Y + 5);
        }
    }

    public class MoveEntity : Entity
    {
        public float Width;
        public float Height;
        public CombatMove Move { get; set; }

        public MoveEntity(CombatMove move, float x, float y) : base(x, y)
        {
            Move = move;
            Width = 200;
            Height = 50;
        }

        public override void Render()
        {
            Draw.Rectangle(X, Y, Width, Height, Color.Gray, Color.Green, 0.5f);
            Draw.Text(Move.Name, 30, X + 5, Y + 5);
        }
    }
}