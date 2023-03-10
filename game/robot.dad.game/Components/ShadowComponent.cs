using Otter;
using robot.dad.game.Entities;
using robot.dad.game.Sprites;

namespace robot.dad.game.Components
{
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
            PlayerShadowSprite.X = _player.PlayerSprite.X + 50;
            PlayerShadowSprite.Y = _player.PlayerSprite.Y + 50;
        }
    }
}