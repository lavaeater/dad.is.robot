using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace robot.dad.game.Sprites
{
    public static class SpritePipe
    {
        public static Image Player => new Image("Sprites\\ship.png");
        public static Image Enemy => new Image("Sprites\\ship.png");
        public static Image ThrustOne => new Image("Sprites\\thrust1.png");
        public static Image ThrustTwo => new Image("Sprites\\thrust2.png");
        public static Image ThrustThree => new Image("Sprites\\thrust3.png");
        public static Image Ruin => new Image("Sprites\\ruinsCorner.png");
        public static Image UnknownTile => new Image("Sprites\\dome.png");

        public static Image Frame => new Image("Sprites\\combat\\frame.png");
        public static Image FrameSelectable => new Image("Sprites\\combat\\frameselected.png");
        public static Image FrameHover => new Image("Sprites\\combat\\frameselector.png");
        public static Image TorsoAndHead => new Image("Sprites\\combat\\torsoandhead.png");
    }
}
