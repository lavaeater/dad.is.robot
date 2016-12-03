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
        public static Image Ruin => new Image("Sprites\\ruinsCorner.png");
        public static Image UnknownTile => new Image("Sprites\\grass_01.png");
    }
}
