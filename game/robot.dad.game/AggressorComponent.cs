using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using Otter;

namespace robot.dad.game
{
    public class AggressorComponent : Component
    {
        private readonly TileEvent _tileEvent;
        private readonly int _sightRadius;
        private CubicHexCoord[] _areaAround;

        public AggressorComponent(TileEvent tileEvent, int sightRadius)
        {
            _tileEvent = tileEvent;
            _sightRadius = sightRadius;
            _areaAround = _tileEvent.Hex.AreaAround(_sightRadius);
        }

        public override void Update()
        {
            base.Update();
            if (!_tileEvent.Visible) return;

            //We're visible, time to find the player! How?
            //Through the scene!
            //var mainScene = (_tileEvent.Scene as MainScene);
            //var curPos = mainScene.BackGround.CurrentPosition;
            //if (_areaAround.Contains(curPos))
            //{
            //    mainScene.StartChase();
            //}
        }
    }
}