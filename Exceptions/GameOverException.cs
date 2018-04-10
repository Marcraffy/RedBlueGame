using System;

namespace RedBlueGame.Exceptions 
{
    class GameOverException : Exception
    {
        public GameOverException() : base("Turns Excede Max Turns") {}
    }
}