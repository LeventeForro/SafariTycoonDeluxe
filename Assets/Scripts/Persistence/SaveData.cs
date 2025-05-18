using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy 3D-s vektort reprezent�l� oszt�ly.
/// </summary>
[Serializable]
public class Vector3Data
{
    public float x, y, z;

    /// <summary>
    /// Inicializ�lja a vektort a megadott <see cref="Vector3"/> alapj�n.
    /// </summary>
    /// <param name="vector">A Unity <see cref="Vector3"/> objektuma.</param>
    public Vector3Data(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    /// <summary>
    /// Visszaadja a vektort Unity <see cref="Vector3"/> form�tumban.
    /// </summary>
    /// <returns>A vektor <see cref="Vector3"/> form�tumban.</returns>
    public Vector3 ToVector3() => new Vector3(x, y, z);
}

/// <summary>
/// Egy �llat adatait t�rol� oszt�ly.
/// </summary>
[Serializable]
public class AnimalData
{
    public string type;
    public Vector3Data position;
}

/// <summary>
/// Egy �t adatait t�rol� oszt�ly.
/// </summary>
[Serializable]
public class RoadData
{
    public string name;
    public Vector3Data position;
}

/// <summary>
/// Egy n�v�ny adatait t�rol� oszt�ly.
/// </summary>
[Serializable]
public class PlantData
{
    public string type;
    public Vector3Data position;
}

/// <summary>
/// A j�t�k ment�si adatait t�rol� oszt�ly.
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// Az �llatok adatai.
    /// </summary>
    public List<AnimalData> animals = new List<AnimalData>();

    /// <summary>
    /// A n�v�nyek adatai.
    /// </summary>
    public List<PlantData> plants = new List<PlantData>();

    /// <summary>
    /// Az utak adatai.
    /// </summary>
    public List<RoadData> roads = new List<RoadData>();

    /// <summary>
    /// A tavak poz�ci�i.
    /// </summary>
    public List<Vector3Data> lakes = new List<Vector3Data>();

    /// <summary>
    /// A Jeepek poz�ci�i.
    /// </summary>
    public List<Vector3Data> jeeps = new List<Vector3Data>();

    /// <summary>
    /// A kamera poz�ci�ja.
    /// </summary>
    public Vector3Data cameraPosition;

    /// <summary>
    /// A j�t�kos p�nzmennyis�ge.
    /// </summary>
    public int money;

    /// <summary>
    /// Az eltelt �r�k sz�ma.
    /// </summary>
    public int hours;

    /// <summary>
    /// Az eltelt napok sz�ma.
    /// </summary>
    public int days;

    /// <summary>
    /// Az eltelt hetek sz�ma.
    /// </summary>
    public int weeks;
}
