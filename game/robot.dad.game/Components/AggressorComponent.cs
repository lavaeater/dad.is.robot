using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using Otter;
using robot.dad.game.Entities;
using robot.dad.game.SceneManager;
using robot.dad.game.Scenes;

namespace robot.dad.game.Components
{
    public class AggressorComponent : Component
    {
        private readonly TileEvent _tileEvent;
        private readonly CubicHexCoord[] _areaAround;

        public AggressorComponent(TileEvent tileEvent, int sightRadius)
        {
            _tileEvent = tileEvent;
            _areaAround = _tileEvent.Hex.AreaAround(sightRadius);
        }

        public override void Update()
        {
            if (!_tileEvent.ShouldUpdate) return;

            //We're visible, time to find the player! How?
            //Through the scene!
            if (!Done)
            {
                var curPos = Manager.Instance.MainScene.BackGround.CurrentPosition;
                if (_areaAround.Contains(curPos))
                {
                    Done = true;
                    Manager.Instance.StartChaseScene(_tileEvent);
                }
            }
        }

        public bool Done;
    }
}