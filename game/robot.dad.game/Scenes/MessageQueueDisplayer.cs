using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Otter;

namespace robot.dad.game.Scenes
{
    public class MessageQueueDisplayer
    {
        public readonly Queue<string> Queue;
        private readonly int _offset;
        private readonly float _speed;
        private List<Message> _messages;

        public MessageQueueDisplayer(Queue<string> queue, Scene scene, int offset, float speed)
        {
            Queue = queue;
            _offset = offset;
            _speed = speed;
            ContainingScene = scene;
            _messages = new List<Message>();
        }

        public Scene ContainingScene { get; set; }

        public void Update()
        {
            if (Queue.IsNotEmpty())
            {
                string message = Queue.Dequeue();
                var messageEntity = new Message(message, RemoveAction, _speed, ContainingScene.Width + _offset, ContainingScene.Height);
                if (_messages.Any(m => m.Y > ContainingScene.Height - 30))
                {
                    var biggestY = _messages.Max(m => m.Y);
                    messageEntity.Y = biggestY + 30;
                }
                _messages.Add(messageEntity);
                ContainingScene.Add(messageEntity);
            }
        }

        private void RemoveAction(Message message)
        {
            _messages.Remove(message);
            ContainingScene.Remove(message);
            if (_messages.IsEmpty())
            {
                MessagesListIsEmpty = true;
            }
        }

        public bool MessagesListIsEmpty { get; set; }
    }
}