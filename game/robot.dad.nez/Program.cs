using System;

namespace robot.dad.nez
{
    internal static class Program
    {
        private static Game game;

        internal static void RunGame()
        {
            game = new Game();
            game.Run();
            game.Dispose();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            RunGame();
        }
    }
}

