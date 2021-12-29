using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameUIController : MonoBehaviour
{
    Transform helpPanel;
    Transform buttons;
    Transform gameOverPanel;
    Transform gomeOverButtons;

    private void Awake()
    {
        helpPanel = transform.GetChild(0);
        HideHelp();

        buttons = transform.GetChild(1);
        buttons.gameObject.SetActive(false);

        gomeOverButtons = transform.GetChild(3);
        gameOverPanel = transform.GetChild(2);
        gameOverPanel.gameObject.SetActive(false);

        gomeOverButtons = transform.GetChild(3);
        gomeOverButtons.gameObject.SetActive(false);
    }

    public void ShowHelp(string txt)
    {
        helpPanel.gameObject.SetActive(true);
        helpPanel.GetChild(0).GetComponent<TMP_Text>().text = txt;
        buttons.gameObject.SetActive(true);
    }

    public void HideHelp()
    {
        helpPanel.gameObject.SetActive(false);
    }

    public void ShowGameOver(int pieces)
    {
        buttons.gameObject.SetActive(false);

        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.GetChild(0).GetComponent<TMP_Text>().text = string.Format(gameOverPanel.GetChild(0).GetComponent<TMP_Text>().text, pieces);

        gomeOverButtons.gameObject.SetActive(true);
    }



    #region Button Actions
    public Action OnSaveClicked;
    public void SaveClick()
    {
        OnSaveClicked?.Invoke();
    }

    public Action OnLoadClicked;
    public void LoadClick()
    {
        OnLoadClicked?.Invoke();
    }

    public Action OnUndoClicked;
    public void UndoClick()
    {
        OnUndoClicked?.Invoke();
    }

    public Action OnRedoClicked;
    public void RedoClick()
    {
        OnRedoClicked?.Invoke();
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Reiniciar()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
