using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    WorldGrid grid;

    [SerializeField] PieceHolder holderPrefab;
    List<PieceHolder> holders = new List<PieceHolder>(); 

    public Action OnGameStarted;
    public Action<int> OnGameEnded;
    public Action<Command> OnUserCommand;

    Transform graphicsTransform;
    MoveData currentMove = new MoveData();
    public int moveCount = 0;
    bool MoveStarted;

    public void Initialize()
    {
        grid = GetComponent<WorldGrid>();
        graphicsTransform = transform.GetChild(0);
        CreateBoard();
    }
    private void CreateBoard()
    {
        Vector2Int pos = Vector2Int.zero;
        holders.Clear();

        int i = 0;
        foreach (var position in grid)
        {
            var coords = grid.GetCoords(position);
            if (coords.x < 2 && coords.y < 2) continue;
            if (coords.x < 2 && coords.y > 4) continue;
            if (coords.x > 4 && coords.y < 2) continue;
            if (coords.x > 4 && coords.y > 4) continue;

            var holder = Instantiate(holderPrefab, position, Quaternion.identity);
            holder.transform.SetParent(graphicsTransform);
            holder.Initialize(this, i++, coords, PieceHolder.State.Filled);

            holders.Add(holder);
        }
    }

    public PieceHolder GetHolderWithIndex(int index)
    {
        Debug.Assert(index >= 0 || index < holders.Count, $"No hoplder with index {index}");

        return holders[index];
    }

    public void OnHolderClicked(int pieceID)
    {
        if (moveCount == 0)
        {
            OnUserCommand?.Invoke(new StartCommand(this, pieceID));
            moveCount++;
            OnGameStarted?.Invoke();
            return;
        }

        if (holders[pieceID].state == PieceHolder.State.Filled)
        {
            holders[currentMove.Old].DeSelect();
            currentMove.Old = pieceID;
            holders[currentMove.Old].Select();
            MoveStarted = true;
        }
        else
        {
            if (!MoveStarted)
                return;

            currentMove.New = pieceID;

            if (!IsLegalMove())
                return;

            MoveStarted = false;

            ResolveMove();
        }
    }

    bool IsLegalMove()
    {
        var New = holders[currentMove.New];
        var Old = holders[currentMove.Old];
        int XDiff = New.GetXPosition() - Old.GetXPosition(); 
        int YDiff = New.GetYPosition() - Old.GetYPosition();

        if (XDiff != 0 && YDiff != 0) return false;
        if (Mathf.Abs(XDiff) != 2 && Mathf.Abs(YDiff) != 2) return false;

        if (Mathf.Abs(XDiff) == 2)
        {
            PieceHolder middlePiece = holders.Where(x => x.GetXPosition() == Old.GetXPosition() + XDiff * 0.5f && x.GetYPosition() == Old.GetYPosition()).FirstOrDefault();
            if (middlePiece.state == PieceHolder.State.Filled)
            {
                currentMove.Deleted = middlePiece.index;
                return true;
            }

            return false;
        }
        else if (Mathf.Abs(YDiff) == 2)
        {
            PieceHolder middlePiece = holders.Where(x => x.GetYPosition() == Old.GetYPosition() + YDiff * 0.5f && x.GetXPosition() == Old.GetXPosition()).FirstOrDefault();
            if (middlePiece.state == PieceHolder.State.Filled)
            {
                currentMove.Deleted = middlePiece.index;
                return true;
            }

            return false;
        }

        return false;
    }

    void ResolveMove()
    {
        OnUserCommand?.Invoke(new MoveCommand(this, currentMove));
        moveCount++;

        OnHolderClicked(holders[currentMove.New].index);

        if (!AvaibleMoves())
        {
            OnGameEnded?.Invoke(moveCount);
        }
    }

    bool AvaibleMoves()
    {
        var filledHolders = holders.Where(x => x.state == PieceHolder.State.Filled);
        var emptyHolders = holders.Where(x => x.state == PieceHolder.State.Empty);
        var Holders = (filledHolders.Count() <= emptyHolders.Count()) ? filledHolders : emptyHolders;

        foreach (var holder in Holders)
        {
            PieceHolder neighbour1 = null;
            PieceHolder neighbour2 = null;
            
            if (holder.state == PieceHolder.State.Filled)
            {
                neighbour1 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() + 1 && x.GetYPosition() == holder.GetYPosition());
                neighbour2 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() + 2 && x.GetYPosition() == holder.GetYPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                    neighbour2 != null && neighbour2.state == PieceHolder.State.Empty)
                {
                    return true;
                }

                neighbour1 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() - 1 && x.GetYPosition() == holder.GetYPosition());
                neighbour2 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() - 2 && x.GetYPosition() == holder.GetYPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                   neighbour2 != null && neighbour2.state == PieceHolder.State.Empty)
                {
                    return true;
                }

                neighbour1 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() + 1 && x.GetXPosition() == holder.GetXPosition());
                neighbour2 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() + 2 && x.GetXPosition() == holder.GetXPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                    neighbour2 != null && neighbour2.state == PieceHolder.State.Empty)
                {
                    return true;
                }

                neighbour1 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() - 1 && x.GetXPosition() == holder.GetXPosition());
                neighbour2 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() - 2 && x.GetXPosition() == holder.GetXPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                    neighbour2 != null && neighbour2.state == PieceHolder.State.Empty)
                {
                    return true;
                }
            }
            else
            {
                neighbour1 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() + 1 && x.GetYPosition() == holder.GetYPosition());
                neighbour2 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() + 2 && x.GetYPosition() == holder.GetYPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                    neighbour2 != null && neighbour2.state == PieceHolder.State.Filled)
                    return true;

                neighbour1 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() - 1 && x.GetYPosition() == holder.GetYPosition());
                neighbour2 = holders.Find(x => x.GetXPosition() == holder.GetXPosition() - 2 && x.GetYPosition() == holder.GetYPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                   neighbour2 != null && neighbour2.state == PieceHolder.State.Filled)
                    return true;

                neighbour1 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() + 1 && x.GetXPosition() == holder.GetXPosition());
                neighbour2 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() + 2 && x.GetXPosition() == holder.GetXPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                    neighbour2 != null && neighbour2.state == PieceHolder.State.Filled)
                    return true;

                neighbour1 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() - 1 && x.GetXPosition() == holder.GetXPosition());
                neighbour2 = holders.Find(x => x.GetYPosition() == holder.GetYPosition() - 2 && x.GetXPosition() == holder.GetXPosition());
                if (neighbour1 != null && neighbour1.state == PieceHolder.State.Filled &&
                    neighbour2 != null && neighbour2.state == PieceHolder.State.Filled)
                    return true;
            }
        }

        return false;
    }

    public struct MoveData
    {
        public int New;
        public int Old;
        public int Deleted;
    }
}


