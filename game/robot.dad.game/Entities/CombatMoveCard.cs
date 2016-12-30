using System;
using Otter;
using robot.dad.common;

namespace robot.dad.game.Entities
{
    public class CombatMoveCard : Entity
    {
        public float Width;
        public float Height;
        public ICombatMove Move { get; set; }
        public Action<ICombatMove> Picked { get; set; }
        public Rectangle EntityArea;

        public CombatMoveCard(ICombatMove move, float x, float y) : base(x, y)
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