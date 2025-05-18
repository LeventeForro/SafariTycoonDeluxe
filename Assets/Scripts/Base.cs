using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// A játék bázisának működését kezelő osztály.
/// </summary>
public class Base : MonoBehaviour
{
    /// <summary>
    /// Az eltelt időzítő.
    /// </summary>
    private float Timer = 0f;


    /// <summary>
    /// Az eltelt másodpercek száma.
    /// </summary>
    private int elapseSeconds = 0;
    
    /// <summary>
    /// A bázison lévő turisták száma.
    /// </summary>
    public static int TouristInBase = 0;

    /// <summary>
    /// Meghatározza, hogy elérhető-e Jeep.
    /// </summary>
    public static bool isJeepAvailable = false;

    /// <summary>
    /// A bázis hírneve.
    /// </summary>
    public static float reputation = 1;

    /// <summary>
    /// A jegyár.
    /// </summary>
    private int ticketPrice;

    /// <summary>
    /// A jegyárat beállító szövegmező.
    /// </summary>
    public TMP_InputField ticketPriceInputField;
    /// <summary>
    /// A játékos vezérlője.
    /// </summary>
    public PlayerController playerController;


    /// <summary>
    /// Frissíti a bázis működését minden képkockában.
    /// </summary>
    public void Update()
    {
        if (ticketPriceInputField != null && int.TryParse(ticketPriceInputField.text, out int price))
        {
            ticketPrice = price;
        }
        Timer += Time.deltaTime;
        elapseSeconds = Mathf.FloorToInt(Timer % 60);
        if (elapseSeconds == (5 - PlayerController.timeSpeed))
        {
            Timer = 0f;
            elapseSeconds = 0;
            GameObject prefab = Resources.Load<GameObject>("Turista");
            float cash = reputation * UnityEngine.Random.Range(100, 801);
            cash = Mathf.Floor(cash);

            if (prefab != null && TouristInBase < 10 && ticketPrice <= cash)
            {
                float spawnRadius = 10f; // a base kürüli távolság ahova spawnolnak a turistak
                Vector3 randomOffset = new Vector3(
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius),
                    6.73f,
                    UnityEngine.Random.Range(-spawnRadius, spawnRadius)
                );
                Vector3 spawnPosition = transform.position + randomOffset;

                Instantiate(prefab, spawnPosition, Quaternion.identity);
                TouristInBase++;
                GameObject.FindAnyObjectByType<PlayerController>().playerCash += ticketPrice;
            }
            else
            {
                Debug.Log("Nincs elég hely a turisták számára vagy nincs elég pénze vagy a prefab nem található.");
            }
        }
    }

    /// <summary>
    /// Beállítja a jegyárat a megadott értékre.
    /// </summary>
    /// <param name="value">A jegyár értéke.</param>
    public void GetValue(string value)
    {
        ticketPrice = int.Parse(value);
    }
}
