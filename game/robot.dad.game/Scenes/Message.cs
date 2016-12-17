using System;
using Otter;

namespace robot.dad.game.Scenes
{
    public class Message : Entity
    {
        private readonly Action<Message> _removeAction;
        private readonly float _speed;

        public Message(string message, Action<Message> removeAction, float speed, float x = 0, float y = 0) : base(x, y)
        {
            _removeAction = removeAction;
            _speed = speed;
            var textGraphic = new Text(message, 30);
            AddGraphic(textGraphic);
        }

        public override void Update()
        {
            if (Y < 0)
            {
                _removeAction(this);
            }
            Y -= _speed;
        }
    }
}