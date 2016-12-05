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
        public static Image Ship => new Image("Sprites\\spaceShips_003.png");
        public static Image Ship0 => new Image("Sprites\\ship0.png");
        public static Image Ship1 => new Image("Sprites\\ship1.png");
        public static Image Ship2 => new Image("Sprites\\ship2.png");
        public static Image Ship3 => new Image("Sprites\\ship3.png");
        public static Image ThrustOne => new Image("Sprites\\thrust1.png");
        public static Image ThrustTwo => new Image("Sprites\\thrust2.png");
        public static Image ThrustThree => new Image("Sprites\\thrust3.png");
        public static Image Ruin => new Image("Sprites\\ruinsCorner.png");
        public static Image UnknownTile => new Image("Sprites\\dome.png");
    }
}
