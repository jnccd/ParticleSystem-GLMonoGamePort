using System;

namespace ParticleSystem
{
    public static class GameStorage
    {
        public static Game1 game = new Game1();
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            GameStorage.game.Run();
        }
    }
}

