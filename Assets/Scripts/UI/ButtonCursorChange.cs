using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Az eg�rkurzor v�ltoztat�s�t kezel� oszt�ly gombok f�l� h�z�skor.
/// </summary>
public class ButtonCursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Az alap�rtelmezett kurzor text�r�ja.
    /// </summary>
    [SerializeField] private Texture2D defCursor;

    /// <summary>
    /// A k�z kurzor text�r�ja.
    /// </summary>
    [SerializeField] private Texture2D handCursor;

    /// <summary>
    /// A k�z kurzor eltol�sa.
    /// </summary>
    private readonly Vector2 handCursorOffset = new Vector2(3, 5);

    /// <summary>
    /// Az alap�rtelmezett kurzor eltol�sa.
    /// </summary>
    private readonly Vector2 defCursorOffset = new Vector2(7, 3);

    /// <summary>
    /// Az eg�rkurzor v�ltoztat�sa, amikor az eg�r a gomb f�l� ker�l.
    /// </summary>
    /// <param name="eventData">Az esem�ny adatai.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(handCursor, handCursorOffset, CursorMode.Auto);
    }

    /// <summary>
    /// Az eg�rkurzor vissza�ll�t�sa, amikor az eg�r elhagyja a gombot.
    /// </summary>
    /// <param name="eventData">Az esem�ny adatai.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defCursor, defCursorOffset, CursorMode.Auto);
    }
}
