using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Az �llatcsoportok trigger esem�nyeinek kezel�s��rt felel�s oszt�ly.
/// </summary>
public class AnimalGroupTriggerDetection : MonoBehaviour
{
    /// <summary>
    /// Az �llatcsoporthoz tartoz� sz�l� kezel�.
    /// </summary>
    private PlantEaterAnimal parentHandler;

    /// <summary>
    /// Inicializ�lja a sz�l� kezel�t az �bred�skor.
    /// </summary>
    private void Awake()
    {
        parentHandler = this.GetComponentInParent<PlantEaterAnimal>();
    }

    /// <summary>
    /// Kezeli a triggerben marad�s esem�ny�t.
    /// </summary>
    /// <param name="other">A triggerben l�v� m�sik objektum.</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlantEaterAnimal>() != null)
        {
            parentHandler?.OnGroupFindTriggerStay(other);
        }
    }


    /// <summary>
    /// Kezeli a triggerb�l val� kil�p�s esem�ny�t.
    /// </summary>
    /// <param name="other">A triggerb�l kil�p� m�sik objektum.</param>
    private void OnTriggerExit(Collider other)
    {
        parentHandler?.OnGroupFindTriggerExit(other);
    }
}
