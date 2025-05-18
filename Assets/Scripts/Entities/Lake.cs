using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy tavat reprezent�l� object az alkalmaz�sban.
/// </summary>
public class Lake : MonoBehaviour
{
    /// <summary>
    /// Visszaadja a t� poz�ci�j�t egy <see cref="Vector3Data"/> objektumk�nt.
    /// </summary>
    /// <returns>
    /// A t� poz�ci�j�t tartalmaz� <see cref="Vector3Data"/> objektum.
    /// </returns>
    public Vector3Data GetLakes()
    {
        return new Vector3Data(this.transform.position);
    }
}
