using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A növények típusait reprezentáló felsorolás.
/// </summary>
public enum PlantTypes
{
    /// <summary>Majomkenyérfa típusa.</summary>
    MAJOMKENYERFA,
    /// <summary>Akáciafa típusa.</summary>
    AKACIAFA,
    /// <summary>Bokor típusa.</summary>
    BOKOR,
    /// <summary>Fû típusa.</summary>
    FU
};

/// <summary>
/// Egy növényt reprezentáló osztály.
/// </summary>
public class Plant : MonoBehaviour
{
    /// <summary>
    /// A növény típusa.
    /// </summary>
    public PlantTypes PlantType;

    /// <summary>
    /// A növény darabszáma.
    /// </summary>
    public int count;

    /// <summary>
    /// Beállítja a növény darabszámát.
    /// </summary>
    /// <param name="c">Az új darabszám.</param>
    public void SetCount(int c)
    {
        this.count = c;
    }

    /// <summary>
    /// Visszaadja a növény adatait egy <see cref="PlantData"/> objektumként.
    /// </summary>
    /// <returns>
    /// A növény adatait tartalmazó <see cref="PlantData"/> objektum.
    /// </returns>
    public PlantData GetPlantData() // Módosítás - Persistence
    {
        return new PlantData
        {
            type = this.PlantType.ToString(),
            position = new Vector3Data(transform.position)
        };
    }
}
