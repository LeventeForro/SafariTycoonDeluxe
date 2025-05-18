using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A j�t�k felhaszn�l�i fel�let�nek kezel�s��rt felel�s oszt�ly.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// Interakci�k hangj�hoz sz�ks�ges v�ltoz�k
    /// </summary>
    public AudioSource auSource;
    public List<AudioClip> soundEffects;

    /// <summary>
    /// A j�t�kos p�nz�t megjelen�t� sz�veg.
    /// </summary>
    public TMP_Text cashText;

    /// <summary>
    /// A n�v�nyev�k sz�m�t megjelen�t� sz�veg.
    /// </summary>
    public TMP_Text plantEaterCounterText;

    /// <summary>
    /// A ragadoz�k sz�m�t megjelen�t� sz�veg.
    /// </summary>
    public TMP_Text predatorCounterText;

    /// <summary>
    /// Az eltelt id�t megjelen�t� sz�veg.
    /// </summary>
    public TMP_Text ellapsedTimeText;

    /// <summary>
    /// A h�rnevet megjelen�t� sz�veg.
    /// </summary>
    public TMP_Text reputationText;

    /// <summary>
    /// A p�nz progress barja.
    /// </summary>
    public Slider cashProgressBar;

    /// <summary>
    /// A n�v�nyev�k sz�m�nak progress barja.
    /// </summary>
    public Slider plantEaterCountProgressBar;

    /// <summary>
    /// A ragadoz�k sz�m�nak progress barja.
    /// </summary>
    public Slider predatorCountProgressBar;

    /// <summary>
    /// A turist�k sz�m�nak progress barja.
    /// </summary>
    public Slider touristCountProgressBar;

    /// <summary>
    /// A j�t�kos vez�rl�je.
    /// </summary>
    public PlayerController playerController;

    /// <summary>
    /// A r�cs vez�rl�je.
    /// </summary>
    public GridController gridController;

    /// <summary>
    /// Az �llatokhoz tartoz� gombok.
    /// </summary>
    public GameObject animalsButtons;

    /// <summary>
    /// A nevezetess�gekhez tartoz� gombok.
    /// </summary>
    public GameObject landmarksButtons;

    /// <summary>
    /// Az id� sebess�ge.
    /// </summary>
    public int timeSpeed;

    /// <summary>
    /// A kamera objektum (tesztel�shez).
    /// </summary>
    public GameObject cam1;

    /// <summary>
    /// A bolt fel�lete.
    /// </summary>
    public GameObject shopUI;

    /// <summary>
    /// A r�cs elhelyez�si gombjai.
    /// </summary>
    public GameObject gridPlaceButtons;

    /// <summary>
    /// Az alap�rtelmezett men� gombjai.
    /// </summary>
    public GameObject defaultMenuButtons;

    /// <summary>
    /// A sz�net men� fel�lete.
    /// </summary>
    public GameObject pauseMenuUI;

    /// <summary>
    /// A statisztik�k fel�lete.
    /// </summary>
    public GameObject statisticsUI;

    /// <summary>
    /// Az alapvet� statisztik�k fel�lete.
    /// </summary>
    public GameObject basicStatsUI;

    /// <summary>
    /// Az alap�rtelmezett id� gombja.
    /// </summary>
    public Button defTimeButton;

    /// <summary>
    /// A gyorsabb id� gombja.
    /// </summary>
    public Button fasterTimeButton;

    /// <summary>
    /// A leggyorsabb id� gombja.
    /// </summary>
    public Button fastestTimeButton;

    /// <summary>
    /// A kiv�lasztott elem k�pe.
    /// </summary>
    public GameObject ChosedItemImage;

    /// <summary>
    /// A kiv�lasztott elemek k�peinek list�ja.
    /// </summary>
    public List<Sprite> chosedItemsImages;

    /// <summary>
    /// A kiv�lasztott elem k�p�nek renderel�je.
    /// </summary>
    public Image chosedItemImageRender;

    /// <summary>
    /// Inicializ�lja a felhaszn�l�i fel�letet a j�t�k ind�t�sakor.
    /// </summary>
    void Start()
    {
        ChosedItemImage.gameObject.SetActive(false);
        cashProgressBar.maxValue = playerController.goodCashes[playerController.gameDifficulty];
        plantEaterCountProgressBar.maxValue = playerController.goodPlantEaters[playerController.gameDifficulty];
        predatorCountProgressBar.maxValue = playerController.goodPredators[playerController.gameDifficulty];
        touristCountProgressBar.maxValue = playerController.goodVisitors[playerController.gameDifficulty];
    }


    /// <summary>
    /// Friss�ti a felhaszn�l�i fel�letet minden k�pkock�ban.
    /// </summary>
    void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// Friss�ti a felhaszn�l�i fel�let elemeit.
    /// </summary>
    public void UpdateUI()
    {
        cashText.text = playerController.playerCash + " Ft";
        plantEaterCounterText.text = "N�v�nyev�k: " + GridController.plantEaterCounter.ToString();
        predatorCounterText.text = "Ragadoz�k: " + GridController.predatorCounter.ToString();
        reputationText.text = "H�rn�v: " + Base.reputation.ToString();
        plantEaterCountProgressBar.value = GridController.plantEaterCounter;
        predatorCountProgressBar.value = GridController.predatorCounter;
        touristCountProgressBar.value = Jeep.satisfiedTourist;
        cashProgressBar.value = playerController.playerCash;
        ellapsedTimeText.text = string.Format("{0}. h�t {1}. nap {2} �ra", playerController.weeks, playerController.days, playerController.hours);
    }

    /// <summary>
    /// Megjelen�ti vagy elrejti a bolt fel�let�t.
    /// </summary>
    public void SetShopUI()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        playerController.camZoomStateText.text = "Sz�net";
        playerController.isPaused = true;
        shopUI.SetActive(!shopUI.gameObject.activeSelf);
        gridController.inShop = shopUI.gameObject.activeSelf;
        ChosedItemImage.SetActive(!shopUI.activeSelf);
    }

    /// <summary>
    /// Elhelyez egy elemet a r�cson.
    /// </summary>
    /// <param name="itemID">Az elhelyezend� elem azonos�t�ja.</param>
    public void PlaceOnTheGrid(int itemID)
    {
        chosedItemImageRender.sprite = chosedItemsImages[itemID];
        gridController.WorkingOnTheGridWithShop(itemID);
        SetShopUI();
        defaultMenuButtons.SetActive(false);
        gridPlaceButtons.SetActive(true);
    }

    /// <summary>
    /// Be�ll�tja a kiv�lasztott elemeket.
    /// </summary>
    /// <param name="id">Az elem azonos�t�ja.</param>
    public void SetSelectedThings(int id)
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        if (id == 0)
        {
            landmarksButtons.SetActive(true);
            animalsButtons.SetActive(false);
        }
        else
        {
            landmarksButtons.SetActive(false);
            animalsButtons.SetActive(true);
        }
    }

    /// <summary>
    /// Megjelen�ti vagy elrejti a sz�net men�t.
    /// </summary>
    public void ShowPauseMenu()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        playerController.isPaused = pauseMenuUI.gameObject.activeSelf;
    }

    /// <summary>
    /// Megjelen�ti vagy elrejti a statisztik�k fel�let�t.
    /// </summary>
    public void ShowStatistics()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        statisticsUI.SetActive(!statisticsUI.activeSelf);
        basicStatsUI.SetActive(!basicStatsUI.activeSelf);
    }

    /// <summary>
    /// Visszat�r a f�men�be.
    /// </summary>
    public void BackToMenu()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// M�dos�tja az id� sebess�g�t.
    /// </summary>
    /// <param name="speed">Az �j id�sebess�g.</param>
    public void ChangeTimeSpeed(int speed)
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        playerController.ChangeTimeSpeed(speed);
        switch (speed)
        {
            case 1:
                this.defTimeButton.GetComponent<Button>().interactable = false;
                this.fasterTimeButton.GetComponent<Button>().interactable = true;
                this.fastestTimeButton.GetComponent<Button>().interactable = true;
                timeSpeed = 1;
                break;
            case 2:
                this.defTimeButton.GetComponent<Button>().interactable = true;
                this.fasterTimeButton.GetComponent<Button>().interactable = false;
                this.fastestTimeButton.GetComponent<Button>().interactable = true;
                timeSpeed = 2;
                break;
            case 3:
                this.defTimeButton.GetComponent<Button>().interactable = true;
                this.fasterTimeButton.GetComponent<Button>().interactable = true;
                this.fastestTimeButton.GetComponent<Button>().interactable = false;
                timeSpeed = 3;
                break;
        }
    }

    /// <summary>
    /// Visszat�r a j�t�khoz.
    /// </summary>
    public void BackToGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        gridController.isGridEnabled = false;
        gridController.OrganizePlacedItems();
        defaultMenuButtons.SetActive(true);
        gridPlaceButtons.SetActive(false);
        ChosedItemImage.SetActive(false);
        playerController.isPaused = false;
        playerController.camZoomStateText.text = playerController.cameraZoomState.ToString();
    }

    /// <summary>
    /// Kil�p a j�t�kb�l.
    /// </summary>
    public void ExitGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        Application.Quit();
    }

}
