using Otter;
using robot.dad.game.Components;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class Player : Entity
    {
        private readonly float _scale;
        public readonly Session Session;
     
        public Player(float scale, float x, float y, Session session)
        {
            //_yOrigin = 200f;//173f;
            //_xOrigin = 50f;
            Init();
            //hmmm
            var axis = Axis.CreateArrowKeys();

            var movement = new ThrusterMovement(3, 5, axis, 90f, 200f, 500f, true);
            AddComponents(axis, movement);

            _scale = scale;
            Session = session;
            X = x;
            Y = y;
        }

        private void Init()
        {
            AddGraphics(SpritePipe.Player);
            Graphic.CenterOrigin();
            Graphic.Scale = _scale;
        }

        public override void Update()
        {
            base.Update();
            Scene.BringToFront(this);
        }
    }
}