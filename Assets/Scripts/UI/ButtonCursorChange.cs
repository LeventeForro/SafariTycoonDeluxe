using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Az egérkurzor változtatását kezelõ osztály gombok fölé húzáskor.
/// </summary>
public class ButtonCursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Az alapértelmezett kurzor textúrája.
    /// </summary>
    [SerializeField] private Texture2D defCursor;

    /// <summary>
    /// A kéz kurzor textúrája.
    /// </summary>
    [SerializeField] private Texture2D handCursor;

    /// <summary>
    /// A kéz kurzor eltolása.
    /// </summary>
    private readonly Vector2 handCursorOffset = new Vector2(3, 5);

    /// <summary>
    /// Az alapértelmezett kurzor eltolása.
    /// </summary>
    private readonly Vector2 defCursorOffset = new Vector2(7, 3);

    /// <summary>
    /// Az egérkurzor változtatása, amikor az egér a gomb fölé kerül.
    /// </summary>
    /// <param name="eventData">Az esemény adatai.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(handCursor, handCursorOffset, CursorMode.Auto);
    }

    /// <summary>
    /// Az egérkurzor visszaállítása, amikor az egér elhagyja a gombot.
    /// </summary>
    /// <param name="eventData">Az esemény adatai.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defCursor, defCursorOffset, CursorMode.Auto);
    }
}
