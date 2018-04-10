
using RedBlueGame.Interfaces;

namespace RedBlueGame
{
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
}