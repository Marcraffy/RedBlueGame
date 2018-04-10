namespace RedBlueGame.Interfaces
{   
    public interface IPlayer 
    {
        bool PlayFrom(bool[] playerState, bool[] opponentState);

        string OwnerName();
    }
}