using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A menükezelésért felelõs osztály.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public AudioSource auSource;
    public List<AudioClip> soundEffects;

    /// <summary>
    /// Az új játék panel.
    /// </summary>
    public GameObject newGamePanel;

    /// <summary>
    /// A betöltési képernyõ.
    /// </summary>
    public GameObject loadingScreen;

    /// <summary>
    /// A játéksegítség panel.
    /// </summary>
    public GameObject gameHelpPanel;

    public GameObject helpPanel1;
    public GameObject helpPanel2;
    public GameObject helpPanel3;

    /// <summary>
    /// Inicializálja a menükezelõt.
    /// </summary>
    void Start()
    {
        loadingScreen.SetActive(false);
    }


    /// <summary>
    /// Betölti a játékot.
    /// </summary>
    public void LoadGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        LoadGameFlag.ShouldLoadGame = true;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsyncGame());
        Debug.Log("Játék betöltése");
    }

    /// <summary>
    /// Megjeleníti vagy elrejti a játéksegítség panelt.
    /// </summary>
    public void ShowGameHelp()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        gameHelpPanel.SetActive(!gameHelpPanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Aktiválja a megadott segítség panelt.
    /// </summary>
    /// /// <param name="value"> segítség panel száma </param>
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
    /// Kilép a játékból.
    /// </summary>
    public void ExitGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        Debug.Log("Kilépés");
        Application.Quit();
    }

    /// <summary>
    /// Új játékot indít.
    /// </summary>
    public void StartNewGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Megjeleníti vagy elrejti az új játék panelt.
    /// </summary>
    public void ShowNewGamePanel()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        newGamePanel.SetActive(!newGamePanel.activeSelf);
    }


    /// <summary>
    /// Új játékot indít a megadott nehézségi szinttel.
    /// </summary>
    /// <param name="diff">A nehézségi szint.</param>
    public void StartGameWithDifficulty(int diff)
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        PlayerPrefs.SetInt("gameDifficulty", diff);
        StartNewGame();
    }

    /// <summary>
    /// Aszinkron módon betölti a játékot.
    /// </summary>
    /// <returns>Korutin a betöltési folyamat kezeléséhez.</returns>
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
