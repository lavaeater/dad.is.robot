using Otter;

namespace robot.dad.game.GameSession
{
    public class CustomSession : Session
    {
        public CustomSession(Game game, string name) : base(game, name)
        {
        }

        public static CustomSession AddSession(Game game, string name)
        {
            var s = new CustomSession(game, name);
            game.Sessions.Add(s);
            return s;
        }
    }
}
