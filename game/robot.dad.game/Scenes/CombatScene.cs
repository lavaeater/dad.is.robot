using System;
using System.Collections.Generic;
using Otter;
using robot.dad.combat;

namespace robot.dad.game.Scenes
{
    public class CombatScene : Scene
    {
        private readonly Action _winAction;
        private long _previousTick = DateTime.Now.Ticks;
        private CombatEngine _combatEngine;
        private float _timeActive;
        public List<CombattantCard> CombattantCards { get; set; } = new List<CombattantCard>();

        public Queue<string> MessageQueue { get; set; } = new Queue<string>();

        public CombatScene(Action winAction)
        {
            Game.Instance.MouseVisible = true;
             
            _winAction = winAction;
            BackGroundColor = Color.Grey;

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
                protagonist.MovePicker = new GraphicalPicker(CombatEngine.Picked, this);
            }
            Antagonists = CombatDemo.Antagonists;
            _combatEngine = new CombatEngine(Protagonists, Antagonists, winAction, LoseAction);

            _combatEngine.MoveFailed = MoveFailed; 
            _combatEngine.MoveSucceeded = MoveSucceeded;
            _combatEngine.SomeoneIsDoingSomething = SomeoneIsDoingSomething;

            //_combatEngine.StartCombat();

            float startX = 50;
            float startY = 50;
            float width = 250;
            float height = width * 1.25f;
            foreach (var protagonist in Protagonists)
            {
                Add(AddCombattantCard(protagonist, startX, startY, width,height));
                startY += height + 30;
            }

            startY = 50;
            startX = 800;
            foreach (var antagonist in Antagonists)
            {
                Add(AddCombattantCard(antagonist, startX, startY, width, height));
                startY += height + 30;
            }

            Add(new MessageQueueDisplayer());

        }

        private void SomeoneIsDoingSomething(Combattant attacker, CombatMove combatMove)
        {
            MessageQueue.Enqueue($"{attacker.Name} {combatMove.Verbified} {attacker.CurrentTarget?.Name}");
        }

        private void MoveSucceeded(Combattant attacker, Combattant target)
        {
            MessageQueue.Enqueue($"och {attacker.Name} lyckas!");
        }

        private void MoveFailed(Combattant attacker)
        {
            MessageQueue.Enqueue($"och {attacker.Name} misslyckas!");
        }

        public void LoseAction()
        {
            string leif = "Leif";
        }

        public CombattantCard AddCombattantCard(Combattant combattant, float x, float y, float width, float height)
        {
            var card = new CombattantCard(combattant, x, y, width, height);
            CombattantCards.Add(card);
            return card;
        }

        public override void Begin()
        {
            _combatEngine.StartCombat();
        }

        public List<Combattant> Antagonists { get; set; }

        public List<Combattant> Protagonists { get; set; }

        public override void Update()
        {
            _timeActive += Game.RealDeltaTime;

        }

        public override void Render()
        {
            base.Render();
            Draw.Text(
                $"{Input.MouseScreenX}:{Input.MouseScreenY}|{Input.GameMouseX}:{Input.GameMouseY}|{Input.MouseRawX}:{Input.MouseRawY}", 30, 10,10);
        }
    }

    public class MessageQueueDisplayer : Entity
    {
        public readonly Queue<string> Queue;
        public Queue<Text> MessageTextQueue;

        public MessageQueueDisplayer(Queue<string> queue)
        {
            Queue = queue;
            MessageTextQueue = new Queue<Text>( new []{new Text("Message for you, sir!", 16)});
            AddGraphic(MessageText, 300, 50);
            Timmer = new AutoTimer(0, 0, 500, 1); 
            Timmer.MaxReached = TimerReached;           
            AddComponent(Timmer);
        }

        private void TimerReached()
        {
            MessageText.Text
        }

        public AutoTimer Timmer { get; set; }
    }
}