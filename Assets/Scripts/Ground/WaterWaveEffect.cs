using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A v�zhull�m effektet kezel� oszt�ly.
/// </summary>
public class WaterWaveEffect : MonoBehaviour
{
    /// <summary>
    /// A v�z MeshRenderer komponense.
    /// </summary>
    private MeshRenderer water;
    /// <summary>
    /// A v�zhull�m intenzit�sa.
    /// </summary>
    public float waterWave = 0f;
    /// <summary>
    /// Meghat�rozza, hogy a v�zhull�m n�vekszik vagy cs�kken.
    /// </summary>
    public bool fill = false;
    /// <summary>
    /// Egy seg�dv�ltoz� a v�zhull�m mozg�s�hoz.
    /// </summary>
    public float goodValue = 0;

    /// <summary>
    /// Inicializ�lja a v�zhull�m effektet.
    /// </summary>
    void Start()
    {
        water = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Friss�ti a v�zhull�m effektet minden k�pkock�ban.
    /// </summary>
    void Update()
    {
        
        
        water.material.SetFloat("_Glossiness", waterWave);

        float deltaWave = 0.5f * Time.deltaTime;
        if (waterWave < 1 && !fill)
        {
            waterWave += deltaWave;
            if (waterWave > 0.9f) fill = true;
        }
        else if (fill)
        {
            waterWave -= deltaWave;
            if (waterWave < 0.2f) fill = false;
        }
        if (goodValue >= 0.005f)
        {

            goodValue = 0;
        }
        else goodValue += 3 * Time.deltaTime;
        MoveWater();
    }

    /// <summary>
    /// Mozgatja a v�z text�r�j�t az id� f�ggv�ny�ben.
    /// </summary>
    void MoveWater()
    {
        float offset = Time.time * 0.02f;
        water.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }
}
