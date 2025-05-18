using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A men�kezel�s�rt felel�s oszt�ly.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public AudioSource auSource;
    public List<AudioClip> soundEffects;

    /// <summary>
    /// Az �j j�t�k panel.
    /// </summary>
    public GameObject newGamePanel;

    /// <summary>
    /// A bet�lt�si k�perny�.
    /// </summary>
    public GameObject loadingScreen;

    /// <summary>
    /// A j�t�kseg�ts�g panel.
    /// </summary>
    public GameObject gameHelpPanel;

    public GameObject helpPanel1;
    public GameObject helpPanel2;
    public GameObject helpPanel3;

    /// <summary>
    /// Inicializ�lja a men�kezel�t.
    /// </summary>
    void Start()
    {
        loadingScreen.SetActive(false);
    }


    /// <summary>
    /// Bet�lti a j�t�kot.
    /// </summary>
    public void LoadGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        LoadGameFlag.ShouldLoadGame = true;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsyncGame());
        Debug.Log("J�t�k bet�lt�se");
    }

    /// <summary>
    /// Megjelen�ti vagy elrejti a j�t�kseg�ts�g panelt.
    /// </summary>
    public void ShowGameHelp()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        gameHelpPanel.SetActive(!gameHelpPanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Aktiv�lja a megadott seg�ts�g panelt.
    /// </summary>
    /// /// <param name="value"> seg�ts�g panel sz�ma </param>
    public void ChangeHelpPanel(int value)
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        switch (value)
        {
            case 0:
                helpPanel1.SetActive(true);
                helpPanel2.SetActive(false);
                helpPanel3.SetActive(false);
                break;
            case 1:
                helpPanel1.SetActive(false);
                helpPanel2.SetActive(true);
                helpPanel3.SetActive(false);
                break;
            case 2:
                helpPanel1.SetActive(false);
                helpPanel2.SetActive(false);
                helpPanel3.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Kil�p a j�t�kb�l.
    /// </summary>
    public void ExitGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        Debug.Log("Kil�p�s");
        Application.Quit();
    }

    /// <summary>
    /// �j j�t�kot ind�t.
    /// </summary>
    public void StartNewGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Megjelen�ti vagy elrejti az �j j�t�k panelt.
    /// </summary>
    public void ShowNewGamePanel()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        newGamePanel.SetActive(!newGamePanel.activeSelf);
    }


    /// <summary>
    /// �j j�t�kot ind�t a megadott neh�zs�gi szinttel.
    /// </summary>
    /// <param name="diff">A neh�zs�gi szint.</param>
    public void StartGameWithDifficulty(int diff)
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        PlayerPrefs.SetInt("gameDifficulty", diff);
        StartNewGame();
    }

    /// <summary>
    /// Aszinkron m�don bet�lti a j�t�kot.
    /// </summary>
    /// <returns>Korutin a bet�lt�si folyamat kezel�s�hez.</returns>
    IEnumerator LoadAsyncGame()
    {
        LoadGameFlag.ShouldLoadGame = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);


        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }

    }
}
