public class MoveCommand : Command
{
    Board board;
    public Board.MoveData moveData;

    public MoveCommand(Board b, Board.MoveData m)
    {
        board = b;
        moveData = m;
    }

    public override void Execute()
    {
        board.GetHolderWithIndex(moveData.Old).ChangeState(PieceHolder.State.Empty);
        board.GetHolderWithIndex(moveData.Deleted).ChangeState(PieceHolder.State.Empty);
        board.GetHolderWithIndex(moveData.New).ChangeState(PieceHolder.State.Filled);
    }

    public override void Undo()
    {
        board.GetHolderWithIndex(moveData.Old).ChangeState(PieceHolder.State.Filled);
        board.GetHolderWithIndex(moveData.Deleted).ChangeState(PieceHolder.State.Filled);
        board.GetHolderWithIndex(moveData.New).ChangeState(PieceHolder.State.Empty);
    }
}
