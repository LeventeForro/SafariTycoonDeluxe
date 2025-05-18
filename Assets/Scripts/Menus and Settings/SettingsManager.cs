using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A be�ll�t�sok kezel�s��rt felel�s oszt�ly.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    /// <summary>
    /// Az aktu�lis felbont�s indexe.
    /// </summary>
    public int resolutionIndex;

    /// <summary>
    /// Teljes k�perny�s m�d �llapota.
    /// </summary>
    public bool isFullscreen;

    /// <summary>
    /// VSync �llapota.
    /// </summary>
    public bool vSync;

    /// <summary>
    /// A f� hanger� szintje.
    /// </summary>
    public float masterVolume;

    /// <summary>
    /// A hangeffektek hanger� szintje.
    /// </summary>
    public float effectsVolume;

    /// <summary>
    /// A zene hanger� szintje.
    /// </summary>
    public float musicVolume;

    /// <summary>
    /// A felbont�sokat tartalmaz� leg�rd�l� men�.
    /// </summary>
    public TMP_Dropdown resolutionDrowpdown;

    /// <summary>
    /// A teljes k�perny�s m�d kapcsol�ja.
    /// </summary>
    public Toggle fullscreenToggle;

    /// <summary>
    /// A VSync kapcsol�ja.
    /// </summary>
    public Toggle vSyncToggle;
    
    /// <summary>
    /// A f� hanger�t szab�lyoz� cs�szka.
    /// </summary>
    public Slider masterAudioSlider;

    /// <summary>
    /// A zene hanger�t szab�lyoz� cs�szka.
    /// </summary>
    public Slider musicSlider;

    /// <summary>
    /// A hangeffektek hanger�t szab�lyoz� cs�szka.
    /// </summary>
    public Slider soundEffectsSlider;

    /// <summary>
    /// A teljes k�perny�s m�d kapcsol�j�nak kezel�je.
    /// </summary>
    public ShwitchHandler fullscreenToggleSwitch;

    /// <summary>
    /// A VSync kapcsol�j�nak kezel�je.
    /// </summary>
    public ShwitchHandler vSyncToggleSwitch;

    /// <summary>
    /// A felbont�sok sz�veges list�ja.
    /// </summary>
    List<string> resTexts = new List<string>();

    /// <summary>
    /// Az el�rhet� felbont�sok list�ja.
    /// </summary>
    public Resolution[] resolutions;

    /// <summary>
    /// A be�ll�t�sok men� panelje.
    /// </summary>
    public GameObject settingsMenuPanel;

    /// <summary>
    /// Az audio kever�.
    /// </summary>
    public AudioMixer audioMixer;


    /// <summary>
    /// Inicializ�lja a be�ll�t�sokat az alkalmaz�s ind�t�sakor.
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
    /// Inicializ�lja a be�ll�t�sokat a j�t�k ind�t�sakor.
    /// </summary>
    void Start()
    {
        // Ez sajnos musz�j ide, mert k�l�nben nem t�lti be, mivel az Awake el�bb fut le, mint hogy hozz�t�rs�tan� a "beh�zott" dolgokat
        this.SetMasterVolume(this.masterVolume);
        this.SetEffectsVolume(this.effectsVolume);
        this.SetMusicVolume(this.musicVolume);
        this.resolutionDrowpdown.value = this.resolutionIndex;
        Debug.Log(resolutionIndex);
    }

    /// <summary>
    /// Friss�ti a be�ll�t�sok men� �llapot�t minden k�pkock�ban.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) settingsMenuPanel.SetActive(!settingsMenuPanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Elrejti a be�ll�t�sok panelt.
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
    /// Bet�lti a j�t�k jelenet be�ll�t�sait.
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
    /// Bet�lti a be�ll�t�sokat a PlayerPrefs-b�l.
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

        Debug.Log("A be�ll�t�sok bet�lt�dtek �s alkalmazva lettek");
    }

    /// <summary>
    /// Elmenti a be�ll�t�sokat a PlayerPrefs-be.
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

        Debug.Log("A be�ll�t�sok el lettek mentve");
    }

    /// <summary>
    /// Be�ll�tja a felbont�st.
    /// </summary>
    /// <param name="resIndex">A felbont�s indexe.</param>
    public void SetResolution(int resIndex)
    {
        Debug.Log("A felbont�s �t lett �ll�tva: " + resolutionDrowpdown.options[resIndex].text.ToString());
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, isFullscreen);
        this.resolutionIndex = resIndex;
    }

    /// <summary>
    /// Be�ll�tja a teljes k�perny�s m�dot.
    /// </summary>
    /// <param name="fullscreen">Teljes k�perny�s m�d �llapota.</param>
    public void SetFullscreenMode(bool fullscreen) 
    {
        if (fullscreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen; // Mivel ez m�g DirectX 11-es app, ez�rt itt m�g van abszol�t fullscreen
        else Screen.fullScreenMode = FullScreenMode.Windowed;

        Debug.Log("Teljes k�perny�: " + fullscreen.ToString());
        this.isFullscreen = fullscreen;
    }

    /// <summary>
    /// Be�ll�tja a VSync �llapot�t.
    /// </summary>
    /// <param name="vSync">A VSync �llapota.</param>
    public void SetVSync(bool vSync)
    {
       // csak ki �s bekapcsolja, semmi extra
        if (vSync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;

        this.vSync = vSync;
        Debug.Log("VSync be�ll�tva: " + vSync.ToString());
    }

    /// <summary>
    /// Be�ll�tja a f� hanger�t.
    /// </summary>
    /// <param name="volume">A hanger� szintje.</param>
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
        this.masterVolume = volume;
    }

    /// <summary>
    /// Be�ll�tja a hangeffektek hanger�t.
    /// </summary>
    /// <param name="volume">A hanger� szintje.</param>
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", volume);
        this.effectsVolume = volume;
    }

    /// <summary>
    /// Be�ll�tja a zene hanger�t.
    /// </summary>
    /// <param name="volume">A hanger� szintje.</param>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        this.musicVolume = volume;
    }
}
