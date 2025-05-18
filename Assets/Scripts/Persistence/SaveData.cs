using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy 3D-s vektort reprezentáló osztály.
/// </summary>
[Serializable]
public class Vector3Data
{
    public float x, y, z;

    /// <summary>
    /// Inicializálja a vektort a megadott <see cref="Vector3"/> alapján.
    /// </summary>
    /// <param name="vector">A Unity <see cref="Vector3"/> objektuma.</param>
    public Vector3Data(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    /// <summary>
    /// Visszaadja a vektort Unity <see cref="Vector3"/> formátumban.
    /// </summary>
    /// <returns>A vektor <see cref="Vector3"/> formátumban.</returns>
    public Vector3 ToVector3() => new Vector3(x, y, z);
}

/// <summary>
/// Egy állat adatait tároló osztály.
/// </summary>
[Serializable]
public class AnimalData
{
    public string type;
    public Vector3Data position;
}

/// <summary>
/// Egy út adatait tároló osztály.
/// </summary>
[Serializable]
public class RoadData
{
    public string name;
    public Vector3Data position;
}

/// <summary>
/// Egy növény adatait tároló osztály.
/// </summary>
[Serializable]
public class PlantData
{
    public string type;
    public Vector3Data position;
}

/// <summary>
/// A játék mentési adatait tároló osztály.
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// Az állatok adatai.
    /// </summary>
    public List<AnimalData> animals = new List<AnimalData>();

    /// <summary>
    /// A növények adatai.
    /// </summary>
    public List<PlantData> plants = new List<PlantData>();

    /// <summary>
    /// Az utak adatai.
    /// </summary>
    public List<RoadData> roads = new List<RoadData>();

    /// <summary>
    /// A tavak pozíciói.
    /// </summary>
    public List<Vector3Data> lakes = new List<Vector3Data>();

    /// <summary>
    /// A Jeepek pozíciói.
    /// </summary>
    public List<Vector3Data> jeeps = new List<Vector3Data>();

    /// <summary>
    /// A kamera pozíciója.
    /// </summary>
    public Vector3Data cameraPosition;

    /// <summary>
    /// A játékos pénzmennyisége.
    /// </summary>
    public int money;

    /// <summary>
    /// Az eltelt órák száma.
    /// </summary>
    public int hours;

    /// <summary>
    /// Az eltelt napok száma.
    /// </summary>
    public int days;

    /// <summary>
    /// Az eltelt hetek száma.
    /// </summary>
    public int weeks;
}
