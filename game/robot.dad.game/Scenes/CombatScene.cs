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
        private readonly Action _winAction;
        private long _tick = 0;
        private CombatEngine _combatEngine;
        public List<CombattantCard> CombattantCards { get; set; } = new List<CombattantCard>();

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

            //_tick++;
            //if (_tick > 100)
            //    _winAction();
        }

        public override void Render()
        {
            base.Render();
            Draw.Text(
                $"{Input.MouseScreenX}:{Input.MouseScreenY}|{Input.GameMouseX}:{Input.GameMouseY}|{Input.MouseRawX}:{Input.MouseRawY}", 30, 10,10);
        }
    }

    public class GraphicalPicker : MovePickerBase
    {
        public CombatScene Scene { get; set; }

        public GraphicalPicker(Action donePicking, CombatScene scene) : base(donePicking)
        {
            Scene = scene;
        }

        public CombattantCard CurrentCard { get; set; }

        public override void PickMove(Combattant attacker, IEnumerable<Combattant> possibleTargets)
        {
            //1. Find card
            CurrentCard = Scene.GetEntities<CombattantCard>().Single(cc => cc.Combattant == attacker);

            //2. Put it in "picking mode"
            CurrentCard.SetInPickMoveMode(AMoveWasPicked);

            //3. Wait for input... like a click on something? Read on ze internet

        }

        public void AMoveWasPicked(CombatMove pickedMove)
        {
            CurrentCard.Combattant.CurrentMove = pickedMove;
            CurrentCard.StopPicking();
            foreach (var card in Scene.CombattantCards.Except(new []{ CurrentCard}))
            {
                if(card.Combattant.Status == CombatStatus.Active)
                    card.MakePickable(ATargetWasPicked);
            }
        }

        public void ATargetWasPicked(Combattant target)
        {
            foreach (var card in Scene.CombattantCards.Except(new[] { CurrentCard }))
            {
                card.StopBeingPickable();
            }
            CurrentCard.Combattant.CurrentTarget = target;
            DonePicking();
        }
    }


    public class MoveEntity : Entity
    {
        public float Width;
        public float Height;
        public CombatMove Move { get; set; }
        public Action<CombatMove> Picked { get; set; }
        public Rectangle EntityArea;

        public MoveEntity(CombatMove move, float x, float y) : base(x, y)
        {
            Move = move;
            Width = 200;
            Height = 50;
            EntityArea = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        public override void Render()
        {
            var foreColor = Picked != null ? EntityArea.Contains((int)Input.MouseRawX, (int)Input.MouseRawY) ? Color.Red : Color.Green : Color.Gray;
            Draw.Rectangle(X, Y, Width, Height, foreColor, Color.Green, 0.5f);
            Draw.Text(Move.Name, 15, X + 5, Y + 5);
            Draw.Text($"Maxskada: {Move.MaxDamage}", 15, X + 5, Y + 35);
        }

        public override void Update()
        {
            if (Input.MouseButtonReleased(MouseButton.Left))
            {
                if (EntityArea.Contains((int)Input.MouseRawX, (int)Input.MouseRawY))
                {
                    Picked?.Invoke(Move);
                }
            }
        }
    }
}