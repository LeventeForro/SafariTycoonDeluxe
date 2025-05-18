using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy utat reprezentáló osztály.
/// </summary>
public class RoadTile : MonoBehaviour
{
    /// <summary>
    /// Az összes lerakott út listája.
    /// </summary>
    public static List<RoadTile> allRoadTiles = new List<RoadTile>();  // az összes lerakott út

    /// <summary>
    /// Inicializálja az út objektumot és hozzáadja a listához.
    /// </summary>
    public void Awake()
    {
        allRoadTiles.Add(this);
        gameObject.name = gameObject.name.Replace("(Clone)", "").Trim();
    }

    /// <summary>
    /// Visszaadja az út pozícióját és nevét egy <see cref="RoadData"/> objektumban.
    /// </summary>
    /// <returns>Az út adatait tartalmazó <see cref="RoadData"/> objektum.</returns>
    public RoadData GetRoadPos() // Módosítás - Persistence
    {
        return new RoadData
        {
            name = gameObject.name,
            position = new Vector3Data(this.transform.position)
        };

    }
}
