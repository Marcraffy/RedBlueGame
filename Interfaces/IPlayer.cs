namespace RedBlueGame.Interfaces
{   
    public interface IPlayer 
    {
        bool PlayFrom(bool[,] state);

        string OwnerName();
    }
}