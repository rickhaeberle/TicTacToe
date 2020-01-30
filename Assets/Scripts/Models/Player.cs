public abstract class Player {

    protected BoardController _boardController;

    public char Marker { get; private set; }

    public void Register(BoardController boardController, char marker) {
        _boardController = boardController;
        Marker = marker;
    }

    public abstract void MakePlay(Board board);

    protected char GetOpponentMarker() {
        return Marker.Equals(Constants.Marker.X) ? Constants.Marker.O : Constants.Marker.X;
    }

}
