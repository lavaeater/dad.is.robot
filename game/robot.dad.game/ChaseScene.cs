using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game
{
    public class ChaseScene : Scene
    {
        private Player _player;
        private Enemy _enemy;

        public ChaseScene()
        {
            _player = new Player(Center.X, Center.Y, Global.PlayerOne);
            Add(_player);
            _enemy = new Enemy(0, 0, _player);
            _enemy.AddComponent(new Alarm(EnemyGivesUp, 1000));
            Add(_enemy);
            CameraFocus = _player;
            BackGroundColor = new Color(0.85f, 0.85f);
        }

        public void EnemyGivesUp()
        {
            string yadad = "ahashd";
        }
    }

    public class Enemy : Entity
    {
        public Enemy(float x, float y, Entity chasedEntity)
        {
            X = x;
            Y = y;
            this.Graphic = SpritePipe.Ship;
            this.Graphic.Scale = 0.5f;
        }
    }
}