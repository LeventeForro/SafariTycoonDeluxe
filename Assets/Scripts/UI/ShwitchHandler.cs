using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Egy kapcsol� kezel�s��rt felel�s oszt�ly, amely cs�szk�t haszn�l a vizu�lis megjelen�t�shez.
/// </summary>
public class ShwitchHandler : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// A cs�szka aktu�lis �rt�ke.
    /// </summary>
    [SerializeField, Range(0, 1f)]
    protected float sliderValue;

    /// <summary>
    /// A kapcsol� aktu�lis �llapota.
    /// </summary>
    public bool CurrentValue { get; set; }

    /// <summary>
    /// Az anim�ci� id�tartama.
    /// </summary>
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;

    /// <summary>
    /// Az el�z� �rt�k.
    /// </summary>
    private bool _previousValue;

    /// <summary>
    /// Az anim�ci� g�rb�je.
    /// </summary>
    [SerializeField]
    private AnimationCurve slideEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    /// <summary>
    /// Az esem�ny, amely akkor h�v�dik meg, amikor a kapcsol� bekapcsol.
    /// </summary>
    [Header("Events")]
    [SerializeField] private UnityEvent onToggleOn;

    /// <summary>
    /// Az esem�ny, amely akkor h�v�dik meg, amikor a kapcsol� kikapcsol.
    /// </summary>
    [SerializeField] private UnityEvent onToggleOff;

    /// <summary>
    /// A kapcsol� csoportkezel�je.
    /// </summary>
    private ToggleSwitchGroupHandler _toggleSwitchGroupManager;

    /// <summary>
    /// Az anim�ci�s �tmenet hat�s�nak kezel�je.
    /// </summary>
    protected Action transitionEffect;

    /// <summary>
    /// A kapcsol� inicializ�l�sa az �rv�nyes�t�s sor�n.
    /// </summary>
    protected virtual void OnValidate()
    {
        SetupToggleComponents();
        _slider.value = sliderValue;
    }

    /// <summary>
    /// A kapcsol� inicializ�l�sa az �bred�skor.
    /// </summary>
    protected virtual void Awake()
    {
        SetupSliderComponent();
    }

    /// <summary>
    /// A kapcsol� �llapot�nak v�lt�sa kattint�skor.
    /// </summary>
    /// <param name="eventData">Az esem�ny adatai.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    /// <summary>
    /// A kapcsol� �llapot�nak v�lt�sa a csoportkezel� �ltal.
    /// </summary>
    /// <param name="valueToSetTo">Az �j �llapot.</param>
    public void ToggleByGroupManager(bool valueToSetTo)
    {
        SetStateAndStartAnimation(valueToSetTo);
    }

    /// <summary>
    /// A kapcsol� anim�ci�j�nak kezel�se.
    /// </summary>
    /// <returns>Korutin az anim�ci� kezel�s�hez.</returns>
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
