using robot.dad.game.SceneManager;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            StartManager();
        }

        private static void StartManager()
        {
            var manager = new Manager();
            manager.StartGame();
        }
    }
}
