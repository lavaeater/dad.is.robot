using System;
using Otter;
using robot.dad.game.Components;
using robot.dad.game.Entities;

namespace robot.dad.game.Scenes
{
    public class ChaseScene : Scene
    {
        public ChaseScene(Action returnAction)
        {
            var player = new Player(0.5f, Center.X, Center.Y, Global.PlayerOne);
            Add(player);
            var enemy = new Enemy(Center.X - 500, Center.Y - 500);
            var axis = new Axis();
            var chase = new ChaseComponent(axis, player, 100, returnAction);
            var thruster = new ThrusterMovement(2, 5, axis, 90, 200, 700, true);
            enemy.AddComponents(axis, thruster, chase, new Alarm(returnAction, 1000));
            Add(enemy);
            CameraFocus = player;
            BackGroundColor = new Color(0.85f, 0.85f);
        }
    }
}