using System;
using System.Threading;
using Otter;
using robot.dad.game.Sprites;

namespace robot.dad.game
{
    public class ChaseScene : Scene
    {
        private Player _player;
        private Enemy _enemy;

        public ChaseScene(Action returnAction)
        {
            _player = new Player(Center.X, Center.Y, Global.PlayerOne);
            Add(_player);
            _enemy = new Enemy(Center.X - 500, Center.Y - 500);
            var axis = new Axis();
            var chase = new ChaseComponent(axis, _player, 100, returnAction);
            var thruster = new ThrusterMovement(2, 5, axis, 90, 200, 700, true);
            _enemy.AddComponents(axis, thruster, chase, new Alarm(returnAction, 1000));
            Add(_enemy);
            CameraFocus = _player;
            BackGroundColor = new Color(0.85f, 0.85f);
        }
    }

    public class Enemy : Entity
    {
        public Enemy(float x, float y)
        {
            X = x;
            Y = y;
            this.Graphic = SpritePipe.Ship;
            this.Graphic.Scale = 0.5f;
        }
    }

    class ChaseComponent : Component
    {
        private readonly Axis _axis;
        private readonly Entity _chasee;
        private readonly float _catchDistance;
        private readonly Action _returnAction;

        public ChaseComponent(Axis axis, Entity chasee, float catchDistance, Action returnAction)
        {
            _axis = axis;
            _chasee = chasee;
            _catchDistance = catchDistance;
            _returnAction = returnAction;
        }

        public override void Update()
        {
            //1. Check position of chasee
            var something = Util.Angle(_chasee, Entity);

            //2. Figure out if we should turn left or right!
            var leftOrRight = Util.AngleDifferenceSign(Entity.Graphic.Angle - 90f, something);

            //3. Turn towards correct angle
            _axis.ForceState(leftOrRight, 1);

            if (Util.Distance(Entity, _chasee) < _catchDistance)
            {
                Game.Instance.SwitchScene(new CombatScene(_returnAction));
            }

            //4. Pedal to the metal!
        }
    }

    public class CombatScene : Scene
    {
        private readonly Action _returnAction;
        private long _tick = 0;

        public CombatScene(Action returnAction)
        {
            _returnAction = returnAction;
            BackGroundColor = Color.White;
        }

        public override void Update()
        {
            _tick++;
            if (_tick > 100)
                _returnAction();
        }
    }
}