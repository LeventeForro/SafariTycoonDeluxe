using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// A j�t�k ment�s��rt �s bet�lt�s��rt felel�s oszt�ly.
/// </summary>
public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// A ment�si f�jl el�r�si �tja.
    /// </summary>
    private string savePath => Path.Combine(Application.persistentDataPath, "save.json");

    /// <summary>
    /// Elmenti a j�t�k �llapot�t a megadott adatokkal.
    /// </summary>
    /// <param name="data">A mentend� j�t�kadatok.</param>
    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("J�t�k mentve: " + savePath);
    }

    /// <summary>
    /// Bet�lti a j�t�k �llapot�t a ment�si f�jlb�l.
    /// </summary>
    /// <returns>A bet�lt�tt j�t�kadatok.</returns>
    public SaveData LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Ment�s nem tal�lhat�!");
            return null;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("A j�t�k bet�ltve");
        return data;
    }


}
