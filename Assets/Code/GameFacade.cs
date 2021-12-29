using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    [SerializeField] Board boardPrefab;
    Board board;

    [SerializeField] GameUIController gameUIPrefab;
    GameUIController gameUI;

    CommandInvoker commandInvoker;
    SaveSystem gameData;

    private void OnEnable()
    {
        board = Instantiate(boardPrefab, transform);
        board.Initialize();
        board.OnGameStarted += OnGameStarted;
        board.OnGameEnded += OnGameEnded;
        board.OnUserCommand += DoUserCommand;

        gameUI = Instantiate(gameUIPrefab);
        gameUI.OnUndoClicked += DoUndo;
        gameUI.OnRedoClicked += DoRedo;
        gameUI.OnSaveClicked += DoSave;
        gameUI.OnLoadClicked += DoLoad;
        gameUI.ShowHelp("Toca la primera pieza para empezar");

        commandInvoker = new CommandInvoker();
        gameData = new SaveSystem();
    }

    void OnGameStarted()
    {
        StartCoroutine(HelpBox());
    }

    void OnGameEnded(int moveCount)
    {
        gameUI.ShowGameOver(33 - moveCount);
    }


    void DoSave()
    {
        gameData.SaveGame("\\partida1", commandInvoker.GetHistoryData());
    }

    void DoLoad()
    {
        GameData data = gameData.LoadGame("\\partida1");
        commandInvoker.LoadListFromData(data, board);
    }

    void DoUserCommand(Command command)
    {
        commandInvoker.ExecuteCommand(command);
    }

    void DoUndo()
    {
        commandInvoker.UndoCommand();
        board.moveCount--;
        if(board.moveCount == 0)
        {
            StopCoroutine("HelpBox");
            gameUI.ShowHelp("Toca la primera pieza para empezar");
        }
    }

    void DoRedo()
    {
        commandInvoker.ExecuteCommand();
    }

    IEnumerator HelpBox()
    {
        gameUI.ShowHelp("Puedes saltar una ficha horizontalmente o verticalmente");
        yield return new WaitForSeconds(5);
        gameUI.HideHelp();
    }

    private void OnDisable()
    {
        board.OnGameStarted -= OnGameStarted;
        board.OnGameEnded -= OnGameEnded;
        board.OnUserCommand -= DoUserCommand;

        gameUI.OnUndoClicked -= DoUndo;
        gameUI.OnRedoClicked -= DoRedo;
        gameUI.OnSaveClicked -= DoSave;
        gameUI.OnLoadClicked -= DoLoad;
    }
}
