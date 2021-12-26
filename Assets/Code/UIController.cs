using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    Transform helpPanel;
    Transform gameOverPanel;
    Transform buttons;

    private void Awake()
    {
        helpPanel = transform.GetChild(0);
        HideHelp();

        gameOverPanel = transform.GetChild(1);
        gameOverPanel.gameObject.SetActive(false);

        buttons = transform.GetChild(2);
        buttons.gameObject.SetActive(false);
    }

    public void ShowHelp()
    {
        helpPanel.gameObject.SetActive(true);
    }

    public void ShowHelp(string txt)
    {
        helpPanel.gameObject.SetActive(true);
        helpPanel.GetChild(0).GetComponent<TMP_Text>().text = txt;
    }

    public void HideHelp()
    {
        helpPanel.gameObject.SetActive(false);
    }

    public void ShowGameOver(int pieces)
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.GetChild(0).GetComponent<TMP_Text>().text = string.Format(gameOverPanel.GetChild(0).GetComponent<TMP_Text>().text, pieces);

        buttons.gameObject.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Reiniciar()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
