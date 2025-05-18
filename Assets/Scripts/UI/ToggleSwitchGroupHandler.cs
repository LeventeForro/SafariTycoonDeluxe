using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy kapcsol�csoport kezel�s��rt felel�s oszt�ly.
/// </summary>
public class ToggleSwitchGroupHandler : MonoBehaviour
{
    /// <summary>
    /// Az alap�rtelmezett kapcsol�.
    /// </summary>
    [Header("Start Value")]
    [SerializeField] private ShwitchHandler initialToggleSwitch;

    /// <summary>
    /// Meghat�rozza, hogy az �sszes kapcsol� kikapcsolhat�-e.
    /// </summary>
    [Header("Toggle Options")]
    [SerializeField] private bool allCanBeToggledOff;

    /// <summary>
    /// A kapcsol�k list�ja.
    /// </summary>
    private List<ShwitchHandler> _toggleSwitches = new List<ShwitchHandler>();

    /// <summary>
    /// Inicializ�lja a kapcsol�csoportot az �bred�skor.
    /// </summary>
    private void Awake()
    {
        ShwitchHandler[] toggleSwitches = GetComponentsInChildren<ShwitchHandler>();
        foreach (var toggleSwitch in toggleSwitches)
        {
            RegisterToggleButtonToGroup(toggleSwitch);
        }
    }

    /// <summary>
    /// Regisztr�l egy kapcsol�t a csoporthoz.
    /// </summary>
    /// <param name="toggleSwitch">A regisztr�land� kapcsol�.</param>
    private void RegisterToggleButtonToGroup(ShwitchHandler toggleSwitch)
    {
        if (_toggleSwitches.Contains(toggleSwitch))
            return;

        _toggleSwitches.Add(toggleSwitch);

        toggleSwitch.SetupForManager(this);
    }

    /// <summary>
    /// Inicializ�lja a kapcsol�csoportot az ind�t�skor.
    /// </summary>
    private void Start()
    {
        bool areAllToggledOff = true;
        foreach (var button in _toggleSwitches)
        {
            if (!button.CurrentValue)
                continue;

            areAllToggledOff = false;
            break;
        }

        if (!areAllToggledOff || allCanBeToggledOff)
            return;

        if (initialToggleSwitch != null)
            initialToggleSwitch.ToggleByGroupManager(true);
        else
            _toggleSwitches[0].ToggleByGroupManager(true);
    }

    /// <summary>
    /// Kezeli a kapcsol�csoport �llapot�nak v�lt�s�t.
    /// </summary>
    /// <param name="toggleSwitch">A kapcsol�, amely v�lt�st kezdem�nyezett.</param>
    public void ToggleGroup(ShwitchHandler toggleSwitch)
    {
        if (_toggleSwitches.Count <= 1)
            return;

        if (allCanBeToggledOff && toggleSwitch.CurrentValue)
        {
            foreach (var button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                button.ToggleByGroupManager(false);
            }
        }
        else
        {
            foreach (var button in _toggleSwitches)
            {
                if (button == null)
                    continue;

                if (button == toggleSwitch)
                    button.ToggleByGroupManager(true);
                else
                    button.ToggleByGroupManager(false);
            }
        }
    }
}
