using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    const int dimensions = 7;
    const int offset = -3;

    [SerializeField] PieceHolder holderPrefab;
    List<PieceHolder> holders = new List<PieceHolder>(); 

    public Action OnGameStarted;
    public Action<int> OnGameEnded;

    Transform graphicsTransform;
    MoveData moveData = new MoveData();
    int moveCount = 0;
    bool MoveStarted;
    bool firstClick;

    public void Initialize()
    {
        graphicsTransform = transform.GetChild(0);
        CreateBoard();
        firstClick = true;
    }


    private void CreateBoard()
    {
        Vector3 position = Vector3.zero;
        position.y = 0.1f;
        Vector2Int pos = Vector2Int.zero;

        holders.Clear();
        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                if (i < 2 && j < 2) continue;
                if (i < 2 && j > 4) continue;
                if (i > 4 && j < 2) continue;
                if (i > 4 && j > 4) continue;

                position.x = j + offset;
                position.z = i + offset;
                pos.x = j;
                pos.y = i;

                var holder = Instantiate(holderPrefab, position, Quaternion.identity);
                holder.transform.SetParent(graphicsTransform);
                holder.Initialize(this, holders.Count, pos, PieceHolder.State.Filled);

                holders.Add(holder);
            }
        }
    }

    public void OnHolderClicked(int pieceID)
    {
        if (firstClick)
        {
            holders[pieceID].ChangeState(PieceHolder.State.Empty);
            firstClick = false;
            moveCount++;
            OnGameStarted?.Invoke();
            return;
        }

        if (holders[pieceID].state == PieceHolder.State.Filled)
        {
            holders[moveData.Old].DeSelect();
            moveData.Old = pieceID;
            holders[moveData.Old].Select();
            MoveStarted = true;
        }
        else
        {
            if (!MoveStarted)
                return;

            moveData.New = pieceID;

            if (!IsLegalMove())
                return;

            MoveStarted = false;

            ResolveMove();
        }
    }

    bool IsLegalMove()
    {
        var New = holders[moveData.New];
        var Old = holders[moveData.Old];
        int XDiff = New.GetXPosition() - Old.GetXPosition(); 
        int YDiff = New.GetYPosition() - Old.GetYPosition();

        if (XDiff != 0 && YDiff != 0) return false;
        if (Mathf.Abs(XDiff) != 2 && Mathf.Abs(YDiff) != 2) return false;

        if (Mathf.Abs(XDiff) == 2)
        {
            PieceHolder middlePiece = holders.Where(x => x.GetXPosition() == Old.GetXPosition() + XDiff * 0.5f && x.GetYPosition() == Old.GetYPosition()).FirstOrDefault();
            if (middlePiece.state == PieceHolder.State.Filled)
            {
                moveData.Deleted = middlePiece.index;
                return true;
            }

            return false;
        }
        else if (Mathf.Abs(YDiff) == 2)
        {
            PieceHolder middlePiece = holders.Where(x => x.GetYPosition() == Old.GetYPosition() + YDiff * 0.5f && x.GetXPosition() == Old.GetXPosition()).FirstOrDefault();
            if (middlePiece.state == PieceHolder.State.Filled)
            {
                moveData.Deleted = middlePiece.index;
                return true;
            }

            return false;
        }

        return false;
    }

    void ResolveMove()
    {
        holders[moveData.Old].ChangeState(PieceHolder.State.Empty);
        holders[moveData.Deleted].ChangeState(PieceHolder.State.Empty);
        holders[moveData.New].ChangeState(PieceHolder.State.Filled);
        moveCount++;

        OnHolderClicked(holders[moveData.New].index);

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
