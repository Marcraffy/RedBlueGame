using System;
using System.IO;
using RedBlueGame.Enums;
using RedBlueGame.Exceptions;

namespace RedBlueGame 
{
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
                history[turn, 0] = player.One.PlayFrom(stateFor(Enums.Player.One), stateFor(Enums.Player.Two));
                history[turn, 1] = player.Two.PlayFrom(stateFor(Enums.Player.Two), stateFor(Enums.Player.One));
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
            var score = new int[2];
            for (int turn = 0; turn < this.turn; turn++)
            {
                var scoreThisTurn = this.score(history[turn, 0], history[turn, 1]);
                score[0] += scoreThisTurn[0];
                score[1] += scoreThisTurn[1];
            }
            return score;
        }

        public bool[] GetLastAction() =>
            new bool[2] { history[turn - 1, 0], history[turn - 1, 1] };

        private int[] score(bool playerOneChoice, bool playerTwoChoice) 
        {
            if (playerOneChoice && playerTwoChoice)
            {
                return new int[] { 1, 1 };
            }
            if (playerOneChoice && !playerTwoChoice)
            {
                return new int[] { 3, 0 };
            }
            if (!playerOneChoice && playerTwoChoice)
            {
                return new int[] { 0, 3 };
            }
            else
            {
                return new int[] { 2, 2 };
            }
        }

        private bool[] stateFor(Enums.Player player)
        {
            var playerState         = new bool[turn];
            var playersPosition     = player == Enums.Player.One ? 0 : 1;

            for (int index = 0; index < turn; index++)
            {
                playerState[index] = history[index, playersPosition];
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