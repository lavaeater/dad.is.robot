using System;
using System.Collections.Generic;
using Otter;
using robot.dad.combat;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class CombattantCard : Entity
    {
        private Image Frame => SpritePipe.Frame;
        private Image FramePickable => SpritePipe.FrameSelectable;
        private Image FrameHover => SpritePipe.FrameHover;
        private Image HeadTorso => SpritePipe.TorsoAndHead;

        public CombattantCard(Combattant combattant, float x, float y, float width, float height)
        {
            Combattant = combattant;
            X = x;
            Y = y;
            Width = Frame.Width;
            Height = Frame.Height;

            EntityArea = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            Moves = new List<CombatMoveCard>();

            float moveX = X + Width;
            float moveY = Y;
            foreach (var combatMove in Combattant.CombatMoves)
            {
                Moves.Add(new CombatMoveCard(combatMove, moveX, moveY));
                moveY += 50f; //should be some variable, height or something
            }
            SetCardMode(CardMode.Neutral);
        }

        public Rectangle EntityArea { get; set; }

        public override void Added()
        {
            Scene.Add(Moves);
        }

        public List<CombatMoveCard> Moves { get; set; }

        public Combattant Combattant { get; set; }
        public float Height { get; set; }

        public float Width { get; set; }
        public CardMode Mode { get; set; }

        public override void Render()
        {
            if (Mode == CardMode.Pickable)
            {
                if (EntityArea.Contains(Input.MouseRawX, Input.MouseRawY))
                {
                    Graphics.RemoveIfContains(Frame);
                    Graphics.Add(FrameHover);
                }
                else
                {
                    Graphics.Add(Frame);
                    Graphics.RemoveIfContains(FrameHover);
                }
            }
            Draw.Text(Combattant.Name, 30, X + 5, Y + 5);
            Draw.Text($"{Combattant.CurrentHealth} / {Combattant.Health}", 30, X + 5, Y + 35);
            RenderEffects();
        }

        private void RenderEffects()
        {
            foreach (var effect in Combattant.CombatEffects)
            {
                Draw.Text($"{effect.EffectName}", 30, X + 5, Y + 70);
            }
        }

        public void SetInPickMoveMode(Action<CombatMove> picked)
        {
            SetCardMode(CardMode.PickingMove);;    
            foreach (var move in Moves)
            {
                move.Picked = picked;
            }
        }

        public void StopPicking()
        {
            SetCardMode(CardMode.Neutral);
            foreach (var moveEntity in Moves)
            {
                moveEntity.Picked = null;
            }
        }

        public void MakePickable(Action<Combattant> picked)
        {
            SetCardMode(CardMode.Pickable);
            Picked = picked;
        }

        public override void Update()
        {
            if (Input.MouseButtonReleased(MouseButton.Left))
            {
                if (EntityArea.Contains((int)Input.MouseRawX, (int)Input.MouseRawY))
                {
                    Picked?.Invoke(Combattant);
//                    SetCardMode(CardMode.Neutral);
                }
            }
        }

        private void SetCardMode(CardMode mode)
        {
            Mode = mode;
            ClearGraphics();
            AddFaceGraphics();
            if (Mode == CardMode.Neutral || Mode == CardMode.PickingMove)
            {
                Graphics.Add(Frame);
            }
            if (Mode == CardMode.Hover)
            {
                Graphics.Add(FrameHover);
            }
        }

        private void AddFaceGraphics()
        {
            Graphics.Add(HeadTorso);
        }

        public Action<Combattant> Picked { get; set; }

        public void StopBeingPickable()
        {
            Picked = null;
            SetCardMode(CardMode.Neutral);
        }
    }
}