using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Egy kapcsoló kezeléséért felelõs osztály, amely csúszkát használ a vizuális megjelenítéshez.
/// </summary>
public class ShwitchHandler : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// A csúszka aktuális értéke.
    /// </summary>
    [SerializeField, Range(0, 1f)]
    protected float sliderValue;

    /// <summary>
    /// A kapcsoló aktuális állapota.
    /// </summary>
    public bool CurrentValue { get; set; }

    /// <summary>
    /// Az animáció idõtartama.
    /// </summary>
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;

    /// <summary>
    /// Az elõzõ érték.
    /// </summary>
    private bool _previousValue;

    /// <summary>
    /// Az animáció görbéje.
    /// </summary>
    [SerializeField]
    private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    /// <summary>
    /// Az esemény, amely akkor hívódik meg, amikor a kapcsoló bekapcsol.
    /// </summary>
    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;

    /// <summary>
    /// Az esemény, amely akkor hívódik meg, amikor a kapcsoló kikapcsol.
    /// </summary>
    [SerializeField] private UnityEvent onToggleOff;

    /// <summary>
    /// A kapcsoló csoportkezelõje.
    /// </summary>
    private ToggleSwitchGroupHandler _toggleSwitchGroupManager;

    /// <summary>
    /// Az animációs átmenet hatásának kezelõje.
    /// </summary>
    protected Action transitionEffect;

    /// <summary>
    /// A kapcsoló inicializálása az érvényesítés során.
    /// </summary>
    protected virtual void OnValidate()
    {
        SetupToggleComponents();
        _slider.value = sliderValue;
    }

    /// <summary>
    /// A kapcsoló inicializálása az ébredéskor.
    /// </summary>
    protected virtual void Awake()
    {
        SetupSliderComponent();
    }

    /// <summary>
    /// A kapcsoló állapotának váltása kattintáskor.
    /// </summary>
    /// <param name="eventData">Az esemény adatai.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    /// <summary>
    /// A kapcsoló állapotának váltása a csoportkezelõ által.
    /// </summary>
    /// <param name="valueToSetTo">Az új állapot.</param>
    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    /// <summary>
    /// A kapcsoló animációjának kezelése.
    /// </summary>
    /// <returns>Korutin az animáció kezeléséhez.</returns>
    private IEnumerator AnimateSlider()
    {
        float startValue = _slider.value;
        float endValue = CurrentValue ? 1 : 0;

        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                _slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transitionEffect?.Invoke();

                yield return null;
            }
        }

        _slider.value = endValue;
    }

    private Slider _slider;

    private Coroutine _animateSliderCoroutine;

    private void SetupToggleComponents()
    {
        if (_slider != null)
            return;

        SetupSliderComponent();
    }

    private void SetupSliderComponent()
    {
        _slider = GetComponent<Slider>();

        if (_slider == null)
        {
            Debug.Log("No slider found!", this);
            return;
        }

        _slider.interactable = false;
        var sliderColors = _slider.colors;
        sliderColors.disabledColor = Color.white;
        _slider.colors = sliderColors;
        _slider.transition = Selectable.Transition.None;
    }

    public void SetupForManager(ToggleSwitchGroupHandler manager)
    {
        _toggleSwitchGroupManager = manager;
    }

    private void Toggle()
    {
        if (_toggleSwitchGroupManager != null)
            _toggleSwitchGroupManager.ToggleGroup(this);
        else
            SetStateAndStartAnimation(!CurrentValue);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        _previousValue = CurrentValue;
        CurrentValue = state;

        if (_previousValue != CurrentValue)
        {
            if (CurrentValue)
                onToggleOn?.Invoke();
            else
                onToggleOff?.Invoke();
        }

        if (_animateSliderCoroutine != null)
            StopCoroutine(_animateSliderCoroutine);

        _animateSliderCoroutine = StartCoroutine(AnimateSlider());
    }
}
