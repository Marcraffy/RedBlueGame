using System;
using System.IO;
using System.Linq;
using System.Reflection;
using RedBlueGame.Enums;
using RedBlueGame.Exceptions;
using RedBlueGame.Interfaces;

namespace RedBlueGame
{
    namespace Enums
    {
        enum Game 
        {
            InProgress,
            Over
        }

        enum Player 
        {
            One,
            Two
        }
    }

    namespace Exceptions 
    {
        class GameOverException : Exception
        {
            public GameOverException() : base("Turns Excede Max Turns") {}
        }
    }

    namespace Interfaces
    {   
        public interface IPlayer 
        {
            bool PlayFrom(bool[,] state);

            string OwnerName();
        }
    }
    
    class Player
    {
        public IPlayer One { get; set; }
        public IPlayer Two { get; set; }

        public Player(IPlayer playerOne, IPlayer playerTwo)
        {
            One = playerOne;
            Two = playerTwo;
        }
    }

    static class Program
    {
        public static void Main()
        {
            var test = @"I:\Projects\RedBluePlayer\bin\Debug\netcoreapp2.0\RedBluePlayer.dll";
            var game = new RedBlueGame(getPlayers(test, test), 2);
            Play(game);
        }

        private static void Play(RedBlueGame game)
        {
            while (game.State != Game.Over)
            {
                game.Play();
            }
            var score = game.Score();
            Console.WriteLine($"{score[0]} {score[1]}");
        }

        private static Player getPlayers(string playerOneFilePath, string playerTwoFilePath) =>
            new Player( 
                Activator.CreateInstance(Assembly.LoadFile(playerOneFilePath).DefinedTypes.First()) as IPlayer,
                Activator.CreateInstance(Assembly.LoadFile(playerTwoFilePath).DefinedTypes.First()) as IPlayer
            );
    }

    class RedBlueGame
    {

        private bool[,] history { get; set; }

        private int turn { get; set; }

        private int maxTurns { get; set; }

        private Player player { get; set; }

        public RedBlueGame(Player player, uint turns) 
        {
            turn        = 0;
            maxTurns    = (int)turns;
            this.player = player;
            history     = new bool[maxTurns, 2];
        }

        public void Play()
        {
            muteConsole(() => {
                history[turn, 0] = player.One.PlayFrom(stateFor(Enums.Player.One));
                history[turn, 1] = player.Two.PlayFrom(stateFor(Enums.Player.Two));
            });

            if(turn < maxTurns)
            {
                turn++;
            }
            else    
            {
                throw new GameOverException();
            }
        }

        public Game State { get => turn >= maxTurns ? Game.Over : Game.InProgress; }

        public int[] Score()
        {
            return new int[2];
        }

        private bool[,] stateFor(Enums.Player player)
        {
            var playerState         = new bool[turn, 2];
            var playersPosition     = player == Enums.Player.One ? 0 : 1;
            var opponentsPosition   = player == Enums.Player.One ? 1 : 0;

            for (int index = 0; index < turn; index++)
            {
                playerState[index, 0] = history[index, playersPosition];
                playerState[index, 1] = history[index, opponentsPosition];
            }

            return playerState;
        }

        private void muteConsole(Action action)
        {
            var ConsoleOut = Console.Out;
            Console.SetOut(TextWriter.Null); 

            action();
            Console.SetOut(ConsoleOut); 
        }
    }  
}
