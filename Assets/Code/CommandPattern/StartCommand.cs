public class StartCommand : Command
{
    Board board;
    public int pieceIndex;

    public StartCommand(Board b, int index)
    {
        board = b;
        pieceIndex = index;
    }
    public override void Execute()
    {
        board.GetHolderWithIndex(pieceIndex).ChangeState(PieceHolder.State.Empty);
    }

    public override void Undo()
    {
        board.GetHolderWithIndex(pieceIndex).ChangeState(PieceHolder.State.Filled);
    }
}
