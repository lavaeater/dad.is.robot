using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using robot.dad.game.GameSession;

namespace robot.dad.game
{
    class Global
    {
        public static CustomSession
            PlayerOne;
    }

    public enum Controls
    {
        Up,
        Down,
        Left,
        Right
    }
}
