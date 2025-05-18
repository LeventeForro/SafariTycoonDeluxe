using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A vízhullám effektet kezelõ osztály.
/// </summary>
public class WaterWaveEffect : MonoBehaviour
{
    /// <summary>
    /// A víz MeshRenderer komponense.
    /// </summary>
    private MeshRenderer water;
    /// <summary>
    /// A vízhullám intenzitása.
    /// </summary>
    public float waterWave = 0f;
    /// <summary>
    /// Meghatározza, hogy a vízhullám növekszik vagy csökken.
    /// </summary>
    public bool fill = false;
    /// <summary>
    /// Egy segédváltozó a vízhullám mozgásához.
    /// </summary>
    public float goodValue = 0;

    /// <summary>
    /// Inicializálja a vízhullám effektet.
    /// </summary>
    void Start()
    {
        water = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Frissíti a vízhullám effektet minden képkockában.
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
    /// Mozgatja a víz textúráját az idõ függvényében.
    /// </summary>
    void MoveWater()
    {
        float offset = Time.time * 0.02f;
        water.material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }
}
