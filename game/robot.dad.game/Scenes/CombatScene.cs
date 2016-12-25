using System;
using System.Collections.Generic;
using System.Linq;
using Otter;
using robot.dad.combat;
using robot.dad.game.Entities;

namespace robot.dad.game.Scenes
{
    public class CombatScene : Scene
    {
        private readonly Action _winAction;
        private CombatEngine _combatEngine;
        private MessageQueueDisplayer _messageQueue;
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
            Protagonists = CombatDemo.GetProtagonists();

            foreach (var protagonist in Protagonists)
            {
                protagonist.MovePicker = new GraphicalPicker(CombatEngine.Picked, this);
            }

            Antagonists = CombatDemo.GetAntagonists(2).ToList();
            _combatEngine = new CombatEngine(Protagonists, Antagonists, winAction, LoseAction);

            _combatEngine.MoveFailed = MoveFailed;
            _combatEngine.MoveSucceeded = MoveSucceeded;
            _combatEngine.SomeoneIsDoingSomething = SomeoneIsDoingSomething;
            _combatEngine.SomeoneDied = SomeoneDied;
            _combatEngine.SomeoneTookDamage = SomeoneTookDamage;

            //_combatEngine.StartCombat();

            float startX = 50;
            float startY = 50;
            float width = 250;
            float height = width * 1.25f;
            foreach (var protagonist in Protagonists)
            {
                Add(AddCombattantCard(protagonist, startX, startY, width, height));
                startY += height + 30;
            }

            startY = 50;
            startX = 600;
            foreach (var antagonist in Antagonists)
            {
                Add(AddCombattantCard(antagonist, startX, startY, width, height));
                startY += height + 30;
            }

            _messageQueue = new MessageQueueDisplayer(MessageQueue, this, - 600, 1);
        }

        private void SomeoneTookDamage(ICombattant combattant, int damage)
        {
            MessageQueue.Enqueue($"{combattant.Name} fick {damage} i skada.");
        }

        private void SomeoneDied(ICombattant combattant)
        {
            MessageQueue.Enqueue($"{combattant.Name} dog!");
        }

        private void SomeoneIsDoingSomething(ICombattant attacker, ICombatMove combatMove)
        {
            MessageQueue.Enqueue($"{attacker.Name} {combatMove.Verbified} {attacker.CurrentTarget?.Name}");
        }

        private void MoveSucceeded(ICombattant attacker, ICombattant target)
        {
            MessageQueue.Enqueue($"och {attacker.Name} lyckas!");
        }

        private void MoveFailed(ICombattant attacker)
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
            _messageQueue.Update();
        }

        public override void Render()
        {
            base.Render();
            Draw.Text(
                $"{Input.MouseScreenX}:{Input.MouseScreenY}|{Input.GameMouseX}:{Input.GameMouseY}|{Input.MouseRawX}:{Input.MouseRawY}", 30, 10, 10);
        }
    }
}