using System;
using Otter;

namespace robot.dad.game.Scenes
{
    public class Message : Entity
    {
        private readonly Action<Message> _removeAction;

        public Message(string message, Action<Message> removeAction, float x = 0, float y = 0) : base(x, y)
        {
            _removeAction = removeAction;
            var textGraphic = new Text(message, 30);
            AddGraphic(textGraphic);
        }

        public override void Update()
        {
            if (Y < 0)
            {
                _removeAction(this);
            }
            Y--;
        }
    }
}