using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Egy turistát reprezentáló osztály.
/// </summary>
public class Tourist : MonoBehaviour
{
    /// <summary>
    /// A turista frissítése minden képkockában.
    /// Ellenőrzi, hogy van-e szabad hely egy Jeepben, és ha igen, a turista beszáll.
    /// </summary>
    void Update()
    {
        GameObject Start_ = GameObject.Find("Start");
        Jeep[] jeeps = FindObjectsOfType<Jeep>(); // Az �sszes jeep p�ld�ny lek�r�se

        foreach (Jeep jeep in jeeps)
        {
            if (jeep.TouristInJeep < jeep.MaxSpace && jeep.transform.position == Start_.transform.position)
            {
                Vector3 targetPosition = Start_.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5f * PlayerController.timeSpeed);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    jeep.TouristInJeep++;
                    Base.TouristInBase--;
                    GameObject.FindAnyObjectByType<PlayerController>().playerCash += 100; // P�lda p�nz hozz�ad�sa
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
