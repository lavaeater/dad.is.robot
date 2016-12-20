using Otter;
using robot.dad.game.Components;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class PlayerEntity : Entity
    {
        public readonly float Scale;
        public Graphic PlayerSprite = SpritePipe.PlayerSprite;

        public PlayerEntity(float scale, float x, float y, bool showShadow)
        {
            Scale = scale;
            Init();
            var axis = Axis.CreateArrowKeys();

            var movement = new ThrusterMovement(3, 5, axis, 90f, 100f, 250f, true);
            AddComponents(axis, movement);
            if (showShadow)
            {
                var shadowComponent = new ShadowComponent(this);
                AddComponent(shadowComponent);
            }
            X = x;
            Y = y;
        }

        private void Init()
        {
            AddGraphics(PlayerSprite);
            PlayerSprite.CenterOrigin();
            PlayerSprite.Scale = Scale;
        }

        public override void Update()
        {
            base.Update();
            Scene.BringToFront(this);
        }
    }
}