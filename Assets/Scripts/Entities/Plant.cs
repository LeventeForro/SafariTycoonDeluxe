using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A n�v�nyek t�pusait reprezent�l� felsorol�s.
/// </summary>
public enum PlantTypes
{
    /// <summary>Majomkeny�rfa t�pusa.</summary>
    MAJOMKENYERFA,
    /// <summary>Ak�ciafa t�pusa.</summary>
    AKACIAFA,
    /// <summary>Bokor t�pusa.</summary>
    BOKOR,
    /// <summary>F� t�pusa.</summary>
    FU
};

/// <summary>
/// Egy n�v�nyt reprezent�l� oszt�ly.
/// </summary>
public class Plant : MonoBehaviour
{
    /// <summary>
    /// A n�v�ny t�pusa.
    /// </summary>
    public PlantTypes PlantType;

    /// <summary>
    /// A n�v�ny darabsz�ma.
    /// </summary>
    public int count;

    /// <summary>
    /// Be�ll�tja a n�v�ny darabsz�m�t.
    /// </summary>
    /// <param name="c">Az �j darabsz�m.</param>
    public void SetCount(int c)
    {
        this.count = c;
    }

    /// <summary>
    /// Visszaadja a n�v�ny adatait egy <see cref="PlantData"/> objektumk�nt.
    /// </summary>
    /// <returns>
    /// A n�v�ny adatait tartalmaz� <see cref="PlantData"/> objektum.
    /// </returns>
    public PlantData GetPlantData() // M�dos�t�s - Persistence
    {
        return new PlantData
        {
            type = this.PlantType.ToString(),
            position = new Vector3Data(transform.position)
        };
    }
}
