using System;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using Otter;
using robot.dad.game.Event;
using robot.dad.game.SceneManager;

namespace robot.dad.game.Components
{
    public class AggressorComponent : Component
    {
        public IEvent TileEvent { get; set; }
        public Action DoneAction { get; set; }
        public CubicHexCoord[] AreaAround { get; }

        public AggressorComponent(IEvent tileEvent, int sightRadius, Action doneAction = null)
        {
            TileEvent = tileEvent;
            DoneAction = doneAction;
            AreaAround = tileEvent.Hex.AreaAround(sightRadius);
        }

        public override void Update()
        {
            if (!TileEvent.ShouldUpdate || TileEvent.EventDone) return;

            //We're visible, time to find the player! How?
            //Through the scene!
            if (!Done)
            {
                var curPos = Manager.Instance.MainScene.BackGround.CurrentPosition;
                if (AreaAround.Contains(curPos))
                {

                    DoneAction?.Invoke();
                    Done = true;
                    Manager.Instance.StartChaseScene();
                }
            }
        }

        public bool Done;
    }
}