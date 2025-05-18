using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy tavat reprezentáló object az alkalmazásban.
/// </summary>
public class Lake : MonoBehaviour
{
    /// <summary>
    /// Visszaadja a tó pozícióját egy <see cref="Vector3Data"/> objektumként.
    /// </summary>
    /// <returns>
    /// A tó pozícióját tartalmazó <see cref="Vector3Data"/> objektum.
    /// </returns>
    public Vector3Data GetLakes()
    {
        return new Vector3Data(this.transform.position);
    }
}
