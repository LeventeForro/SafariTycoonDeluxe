using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A játék felhasználói felületének kezeléséért felelõs osztály.
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// Interakciók hangjához szükséges változók
    /// </summary>
    public AudioSource auSource;
    public List<AudioClip> soundEffects;

    /// <summary>
    /// A játékos pénzét megjelenítõ szöveg.
    /// </summary>
    public TMP_Text cashText;

    /// <summary>
    /// A növényevõk számát megjelenítõ szöveg.
    /// </summary>
    public TMP_Text plantEaterCounterText;

    /// <summary>
    /// A ragadozók számát megjelenítõ szöveg.
    /// </summary>
    public TMP_Text predatorCounterText;

    /// <summary>
    /// Az eltelt idõt megjelenítõ szöveg.
    /// </summary>
    public TMP_Text ellapsedTimeText;

    /// <summary>
    /// A hírnevet megjelenítõ szöveg.
    /// </summary>
    public TMP_Text reputationText;

    /// <summary>
    /// A pénz progress barja.
    /// </summary>
    public Slider cashProgressBar;

    /// <summary>
    /// A növényevõk számának progress barja.
    /// </summary>
    public Slider plantEaterCountProgressBar;

    /// <summary>
    /// A ragadozók számának progress barja.
    /// </summary>
    public Slider predatorCountProgressBar;

    /// <summary>
    /// A turisták számának progress barja.
    /// </summary>
    public Slider touristCountProgressBar;

    /// <summary>
    /// A játékos vezérlõje.
    /// </summary>
    public PlayerController playerController;

    /// <summary>
    /// A rács vezérlõje.
    /// </summary>
    public GridController gridController;

    /// <summary>
    /// Az állatokhoz tartozó gombok.
    /// </summary>
    public GameObject animalsButtons;

    /// <summary>
    /// A nevezetességekhez tartozó gombok.
    /// </summary>
    public GameObject landmarksButtons;

    /// <summary>
    /// Az idõ sebessége.
    /// </summary>
    public int timeSpeed;

    /// <summary>
    /// A kamera objektum (teszteléshez).
    /// </summary>
    public GameObject cam1;

    /// <summary>
    /// A bolt felülete.
    /// </summary>
    public GameObject shopUI;

    /// <summary>
    /// A rács elhelyezési gombjai.
    /// </summary>
    public GameObject gridPlaceButtons;

    /// <summary>
    /// Az alapértelmezett menü gombjai.
    /// </summary>
    public GameObject defaultMenuButtons;

    /// <summary>
    /// A szünet menü felülete.
    /// </summary>
    public GameObject pauseMenuUI;

    /// <summary>
    /// A statisztikák felülete.
    /// </summary>
    public GameObject statisticsUI;

    /// <summary>
    /// Az alapvetõ statisztikák felülete.
    /// </summary>
    public GameObject basicStatsUI;

    /// <summary>
    /// Az alapértelmezett idõ gombja.
    /// </summary>
    public Button defTimeButton;

    /// <summary>
    /// A gyorsabb idõ gombja.
    /// </summary>
    public Button fasterTimeButton;

    /// <summary>
    /// A leggyorsabb idõ gombja.
    /// </summary>
    public Button fastestTimeButton;

    /// <summary>
    /// A kiválasztott elem képe.
    /// </summary>
    public GameObject ChosedItemImage;

    /// <summary>
    /// A kiválasztott elemek képeinek listája.
    /// </summary>
    public List<Sprite> chosedItemsImages;

    /// <summary>
    /// A kiválasztott elem képének renderelõje.
    /// </summary>
    public Image chosedItemImageRender;

    /// <summary>
    /// Inicializálja a felhasználói felületet a játék indításakor.
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
    /// Frissíti a felhasználói felületet minden képkockában.
    /// </summary>
    void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// Frissíti a felhasználói felület elemeit.
    /// </summary>
    public void UpdateUI()
    {
        cashText.text = playerController.playerCash + " Ft";
        plantEaterCounterText.text = "Növényevõk: " + GridController.plantEaterCounter.ToString();
        predatorCounterText.text = "Ragadozók: " + GridController.predatorCounter.ToString();
        reputationText.text = "Hírnév: " + Base.reputation.ToString();
        plantEaterCountProgressBar.value = GridController.plantEaterCounter;
        predatorCountProgressBar.value = GridController.predatorCounter;
        touristCountProgressBar.value = Jeep.satisfiedTourist;
        cashProgressBar.value = playerController.playerCash;
        ellapsedTimeText.text = string.Format("{0}. hét {1}. nap {2} óra", playerController.weeks, playerController.days, playerController.hours);
    }

    /// <summary>
    /// Megjeleníti vagy elrejti a bolt felületét.
    /// </summary>
    public void SetShopUI()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        playerController.camZoomStateText.text = "Szünet";
        playerController.isPaused = true;
        shopUI.SetActive(!shopUI.gameObject.activeSelf);
        gridController.inShop = shopUI.gameObject.activeSelf;
        ChosedItemImage.SetActive(!shopUI.activeSelf);
    }

    /// <summary>
    /// Elhelyez egy elemet a rácson.
    /// </summary>
    /// <param name="itemID">Az elhelyezendõ elem azonosítója.</param>
    public void PlaceOnTheGrid(int itemID)
    {
        chosedItemImageRender.sprite = chosedItemsImages[itemID];
        gridController.WorkingOnTheGridWithShop(itemID);
        SetShopUI();
        defaultMenuButtons.SetActive(false);
        gridPlaceButtons.SetActive(true);
    }

    /// <summary>
    /// Beállítja a kiválasztott elemeket.
    /// </summary>
    /// <param name="id">Az elem azonosítója.</param>
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
    /// Megjeleníti vagy elrejti a szünet menüt.
    /// </summary>
    public void ShowPauseMenu()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        playerController.isPaused = pauseMenuUI.gameObject.activeSelf;
    }

    /// <summary>
    /// Megjeleníti vagy elrejti a statisztikák felületét.
    /// </summary>
    public void ShowStatistics()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        statisticsUI.SetActive(!statisticsUI.activeSelf);
        basicStatsUI.SetActive(!basicStatsUI.activeSelf);
    }

    /// <summary>
    /// Visszatér a fõmenübe.
    /// </summary>
    public void BackToMenu()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Módosítja az idõ sebességét.
    /// </summary>
    /// <param name="speed">Az új idõsebesség.</param>
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
    /// Visszatér a játékhoz.
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
    /// Kilép a játékból.
    /// </summary>
    public void ExitGame()
    {
        auSource.clip = soundEffects[0];
        auSource.Play();
        Application.Quit();
    }

}
