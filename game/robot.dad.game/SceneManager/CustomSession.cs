using Otter;
using robot.dad.common;

namespace robot.dad.game.SceneManager
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

        public void AddCharacter(Character playerCharacter)
        {
            PlayerCharacter = playerCharacter;
        }

        public Character PlayerCharacter { get; set; }
    }
}
