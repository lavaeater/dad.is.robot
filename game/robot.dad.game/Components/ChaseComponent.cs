using System;
using Otter;
using robot.dad.game.Scenes;

namespace robot.dad.game.Components
{
    public class ChaseComponent : Component
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
}