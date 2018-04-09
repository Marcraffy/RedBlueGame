using System;
using System.Linq;
using System.Reflection;

namespace RedBlueGame
{
    class Program
    {
        public static void Main()
        {
            var player1Assembly = Assembly.LoadFrom("RedBluePlayer.dll");
            //var player2Assembly = Assembly.LoadFrom("RedBluePlayer2.dll");

            var player1 = Activator.CreateInstance(player1Assembly.DefinedTypes.First()) as IPlayer;
            //var player2 = Activator.CreateInstance(player2Assembly.DefinedTypes.First()) as IPlayer;

            // run the application
            player1.Step(new bool[0, 0]);
        }
    }

    public interface IPlayer {
        void Step(bool[,] state);

        string GetOwnerName();
    }
}
