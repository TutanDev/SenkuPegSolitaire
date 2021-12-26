using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHolder : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    GameObject piece;
    Color pieceColor;
    Board board;
    [SerializeField] Vector2Int position = Vector2Int.zero;

    public enum State { Empty, Filled}

    public State state = State.Empty;
    public int index;

    public void Initialize(Board board, int index, Vector2Int position, State state = State.Empty)
    {
        this.board = board;
        this.index = index;
        this.position = position;

        piece = Instantiate(piecePrefab, transform.position, Quaternion.identity);
        piece.transform.SetParent(transform);
        pieceColor = piecePrefab.GetComponent<Renderer>().sharedMaterial.color;

        ChangeState(state);
    }

    public int GetXPosition()
    {
        return position.x;
    }

    public int GetYPosition()
    {
        return position.y;
    }

    public void SetPiecePrefab(GameObject go)
    {
        piecePrefab = go;
    }

    public void ChangeState(State newState)
    {
        state = newState;
        OnStateChanged(newState);
    }

    private void OnStateChanged(State newState)
    {
        switch (newState)
        {
            case State.Empty:
                piece.GetComponent<Renderer>().enabled = false;
                break;
            case State.Filled:
                piece.GetComponent<Renderer>().enabled = true;
                break;
            default:
                break;
        }
    }

    public void Select()
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
    }
    public void DeSelect()
    {
        transform.GetChild(0).GetComponent<Renderer>().material.color = pieceColor;
    }


    private void OnMouseUpAsButton()
    {
        board.OnHolderClicked(index);
    }
}
