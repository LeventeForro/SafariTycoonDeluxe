using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A beállítások kezeléséért felelõs osztály.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Az aktuális felbontás indexe.
    /// </summary>
    public int resolutionIndex;

    /// <summary>
    /// Teljes képernyõs mód állapota.
    /// </summary>
    public bool isFullscreen;

    /// <summary>
    /// VSync állapota.
    /// </summary>
    public bool vSync;

    /// <summary>
    /// A fõ hangerõ szintje.
    /// </summary>
    public float masterVolume;

    /// <summary>
    /// A hangeffektek hangerõ szintje.
    /// </summary>
    public float effectsVolume;

    /// <summary>
    /// A zene hangerõ szintje.
    /// </summary>
    public float musicVolume;

    /// <summary>
    /// A felbontásokat tartalmazó legördülõ menü.
    /// </summary>
    public TMP_Dropdown resolutionDrowpdown;

    /// <summary>
    /// A teljes képernyõs mód kapcsolója.
    /// </summary>
    public Toggle fullscreenToggle;

    /// <summary>
    /// A VSync kapcsolója.
    /// </summary>
    public Toggle vSyncToggle;
    
    /// <summary>
    /// A fõ hangerõt szabályozó csúszka.
    /// </summary>
    public Slider masterAudioSlider;

    /// <summary>
    /// A zene hangerõt szabályozó csúszka.
    /// </summary>
    public Slider musicSlider;

    /// <summary>
    /// A hangeffektek hangerõt szabályozó csúszka.
    /// </summary>
    public Slider soundEffectsSlider;

    /// <summary>
    /// A teljes képernyõs mód kapcsolójának kezelõje.
    /// </summary>
    public ShwitchHandler fullscreenToggleSwitch;

    /// <summary>
    /// A VSync kapcsolójának kezelõje.
    /// </summary>
    public ShwitchHandler vSyncToggleSwitch;

    /// <summary>
    /// A felbontások szöveges listája.
    /// </summary>
    List<string> resTexts = new List<string>();

    /// <summary>
    /// Az elérhetõ felbontások listája.
    /// </summary>
    public Resolution[] resolutions;

    /// <summary>
    /// A beállítások menü panelje.
    /// </summary>
    public GameObject settingsMenuPanel;

    /// <summary>
    /// Az audio keverõ.
    /// </summary>
    public AudioMixer audioMixer;


    /// <summary>
    /// Inicializálja a beállításokat az alkalmazás indításakor.
    /// </summary>
    private void Awake()
    {
        foreach (Resolution r in Screen.resolutions) resTexts.Add(r.width.ToString() + "x" + r.height.ToString());
        resolutions = Screen.resolutions;

        resolutionDrowpdown.AddOptions(resTexts);

        if(SceneManager.GetActiveScene().buildIndex == 0) LoadSettings();
        LoadGameSceneSettings();

    }

    /// <summary>
    /// Inicializálja a beállításokat a játék indításakor.
    /// </summary>
    void Start()
    {
        // Ez sajnos muszáj ide, mert különben nem tölti be, mivel az Awake elõbb fut le, mint hogy hozzátársítaná a "behúzott" dolgokat
        this.SetMasterVolume(this.masterVolume);
        this.SetEffectsVolume(this.effectsVolume);
        this.SetMusicVolume(this.musicVolume);
        this.resolutionDrowpdown.value = this.resolutionIndex;
        Debug.Log(resolutionIndex);
    }

    /// <summary>
    /// Frissíti a beállítások menü állapotát minden képkockában.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) settingsMenuPanel.SetActive(!settingsMenuPanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Elrejti a beállítások panelt.
    /// </summary>
    public void HidePanel()
    {

        settingsMenuPanel.SetActive(!settingsMenuPanel.gameObject.activeSelf);

        if (settingsMenuPanel.activeSelf) 
        {
            this.fullscreenToggleSwitch.ToggleByGroupManager(this.isFullscreen);
            this.vSyncToggleSwitch.ToggleByGroupManager(this.vSync);
        }
    }

    /// <summary>
    /// Betölti a játék jelenet beállításait.
    /// </summary>
    void LoadGameSceneSettings()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {

            this.vSync = PlayerPrefs.GetInt("VSync", 1) == 1;
            this.isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            this.resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
            this.masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            this.effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
            this.musicVolume = PlayerPrefs.GetFloat("MusicVolume");

            this.vSyncToggle.isOn = this.vSync;
            this.masterAudioSlider.value = this.masterVolume;
            this.soundEffectsSlider.value = this.effectsVolume;
            this.musicSlider.value = this.musicVolume;
        }
    }

    /// <summary>
    /// Betölti a beállításokat a PlayerPrefs-bõl.
    /// </summary>
    public void LoadSettings()
    {
        Debug.Log("Vsync: " + PlayerPrefs.GetInt("VSync").ToString());
        Debug.Log("Fullscreen: " + PlayerPrefs.GetInt("Fullscreen").ToString());
        Debug.Log("Resolution: " + PlayerPrefs.GetInt("ResolutionIndex").ToString());

        this.vSync = PlayerPrefs.GetInt("VSync", 1) == 1;
        this.isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        this.resolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
        this.masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        this.effectsVolume = PlayerPrefs.GetFloat("EffectsVolume");
        this.musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        this.vSyncToggle.isOn = this.vSync;
        this.masterAudioSlider.value = this.masterVolume;
        this.soundEffectsSlider.value = this.effectsVolume;
        this.musicSlider.value = this.musicVolume;
        

        this.SetResolution(this.resolutionIndex);
        this.SetFullscreenMode(this.isFullscreen);
        this.SetVSync(this.vSync);
        this.SetMasterVolume(this.masterVolume);
        this.SetEffectsVolume(this.effectsVolume);
        this.SetMusicVolume(this.musicVolume);

        Debug.Log("A beállítások betöltõdtek és alkalmazva lettek");
    }

    /// <summary>
    /// Elmenti a beállításokat a PlayerPrefs-be.
    /// </summary>
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("VSync", vSync ? 1 : 0);
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();

        Debug.Log("A beállítások el lettek mentve");
    }

    /// <summary>
    /// Beállítja a felbontást.
    /// </summary>
    /// <param name="resIndex">A felbontás indexe.</param>
    public void SetResolution(int resIndex)
    {
        Debug.Log("A felbontás át lett állítva: " + resolutionDrowpdown.options[resIndex].text.ToString());
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, isFullscreen);
        this.resolutionIndex = resIndex;
    }

    /// <summary>
    /// Beállítja a teljes képernyõs módot.
    /// </summary>
    /// <param name="fullscreen">Teljes képernyõs mód állapota.</param>
    public void SetFullscreenMode(bool fullscreen) 
    {
        if (fullscreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; // Mivel ez még DirectX 11-es app, ezért itt még van abszolút fullscreen
        else Screen.fullScreenMode = FullScreenMode.Windowed;

        Debug.Log("Teljes képernyõ: " + fullscreen.ToString());
        this.isFullscreen = fullscreen;
    }

    /// <summary>
    /// Beállítja a VSync állapotát.
    /// </summary>
    /// <param name="vSync">A VSync állapota.</param>
    public void SetVSync(bool vSync)
    {
       // csak ki és bekapcsolja, semmi extra
        if (vSync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;

        this.vSync = vSync;
        Debug.Log("VSync beállítva: " + vSync.ToString());
    }

    /// <summary>
    /// Beállítja a fõ hangerõt.
    /// </summary>
    /// <param name="volume">A hangerõ szintje.</param>
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
        this.masterVolume = volume;
    }

    /// <summary>
    /// Beállítja a hangeffektek hangerõt.
    /// </summary>
    /// <param name="volume">A hangerõ szintje.</param>
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", volume);
        this.effectsVolume = volume;
    }

    /// <summary>
    /// Beállítja a zene hangerõt.
    /// </summary>
    /// <param name="volume">A hangerõ szintje.</param>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        this.musicVolume = volume;
    }
}
