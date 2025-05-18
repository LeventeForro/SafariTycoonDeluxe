using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy kapcsolócsoport kezeléséért felelõs osztály.
/// </summary>
public class ToggleSwitchGroupHandler : MonoBehaviour
{
    /// <summary>
    /// Az alapértelmezett kapcsoló.
    /// </summary>
    [Header("Start Value")]
    [SerializeField] private ShwitchHandler initialToggleSwitch;

    /// <summary>
    /// Meghatározza, hogy az összes kapcsoló kikapcsolható-e.
    /// </summary>
    [Header("Toggle Options")]
    [SerializeField] private bool allCanBeToggledOff;

    /// <summary>
    /// A kapcsolók listája.
    /// </summary>
    private List<ShwitchHandler> _toggleSwitches = new List<ShwitchHandler>();

    /// <summary>
    /// Inicializálja a kapcsolócsoportot az ébredéskor.
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
    /// Regisztrál egy kapcsolót a csoporthoz.
    /// </summary>
    /// <param name="toggleSwitch">A regisztrálandó kapcsoló.</param>
    private void RegisterToggleButtonToGroup(ShwitchHandler toggleSwitch)
    {
        if (_toggleSwitches.Contains(toggleSwitch))
            return;

        _toggleSwitches.Add(toggleSwitch);

        toggleSwitch.SetupForManager(this);
    }

    /// <summary>
    /// Inicializálja a kapcsolócsoportot az indításkor.
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
    /// Kezeli a kapcsolócsoport állapotának váltását.
    /// </summary>
    /// <param name="toggleSwitch">A kapcsoló, amely váltást kezdeményezett.</param>
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
