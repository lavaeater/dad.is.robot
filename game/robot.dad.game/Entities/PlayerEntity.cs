using Otter;
using robot.dad.game.Components;
using robot.dad.game.Sprites;

namespace robot.dad.game.Entities
{
    public class PlayerEntity : Entity
    {
        public readonly float Scale;
        public readonly Session Session;
        public Graphic PlayerSprite = SpritePipe.PlayerSprite;
        
        public PlayerEntity(float scale, float x, float y, Session session)
        {
            Scale = scale;
            Init();
            var axis = Axis.CreateArrowKeys();

            var movement = new ThrusterMovement(3, 5, axis, 90f, 100f, 250f, true);
            var shadowComponent = new ShadowComponent(this);
            AddComponents(axis, movement, shadowComponent);

            Session = session;
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

    public class ShadowComponent : Component
    {
        private readonly PlayerEntity _player;
        public Graphic PlayerShadowSprite = SpritePipe.PlayerShadowSprite;

        public ShadowComponent(PlayerEntity player)
        {
            _player = player;
        }

        public override void Added()
        {
            PlayerShadowSprite.CenterOrigin();
            PlayerShadowSprite.Scale = _player.Scale / 2;
            Graphics.Add(PlayerShadowSprite);
        }

        public override void Update()
        {
            PlayerShadowSprite.X = _player.PlayerSprite.X + 100;
            PlayerShadowSprite.Y = _player.PlayerSprite.Y + 100;
        }
    }
}