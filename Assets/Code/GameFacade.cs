using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    [SerializeField] Board boardPrefab;
    Board board;

    [SerializeField] UIController UIPrefab;
    UIController UI;

    private void OnEnable()
    {
        board = Instantiate(boardPrefab, transform);
        board.Initialize();
        board.OnGameStarted += OnGameStarted;
        board.OnGameEnded += OnGameEnded;

        UI = Instantiate(UIPrefab);
        UI.ShowHelp();
    }

    void OnGameStarted()
    {
        StartCoroutine(HelpBox());
    }

    void OnGameEnded(int moveCount)
    {
        UI.ShowGameOver(33 - moveCount);
    }
    IEnumerator HelpBox()
    {
        UI.ShowHelp("Puedes saltar una ficha horizontalmente o verticalmente");
        yield return new WaitForSeconds(5);
        UI.HideHelp();
    }

    private void OnDisable()
    {
        board.OnGameStarted -= OnGameStarted;
        board.OnGameEnded -= OnGameEnded;
    }
}
