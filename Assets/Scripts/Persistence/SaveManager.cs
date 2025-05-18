using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// A játék mentéséért és betöltéséért felelõs osztály.
/// </summary>
public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// A mentési fájl elérési útja.
    /// </summary>
    private string savePath => Path.Combine(Application.persistentDataPath, "save.json");

    /// <summary>
    /// Elmenti a játék állapotát a megadott adatokkal.
    /// </summary>
    /// <param name="data">A mentendõ játékadatok.</param>
    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Játék mentve: " + savePath);
    }

    /// <summary>
    /// Betölti a játék állapotát a mentési fájlból.
    /// </summary>
    /// <returns>A betöltött játékadatok.</returns>
    public SaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Mentés nem található!");
            return null;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("A játék betöltve");
        return data;
    }


}
