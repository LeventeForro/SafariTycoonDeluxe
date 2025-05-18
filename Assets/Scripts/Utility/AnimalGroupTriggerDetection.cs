using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Az állatcsoportok trigger eseményeinek kezeléséért felelõs osztály.
/// </summary>
public class AnimalGroupTriggerDetection : MonoBehaviour
{
    /// <summary>
    /// Az állatcsoporthoz tartozó szülõ kezelõ.
    /// </summary>
    private PlantEaterAnimal parentHandler;

    /// <summary>
    /// Inicializálja a szülõ kezelõt az ébredéskor.
    /// </summary>
    private void Awake()
    {
        parentHandler = this.GetComponentInParent<PlantEaterAnimal>();
    }

    /// <summary>
    /// Kezeli a triggerben maradás eseményét.
    /// </summary>
    /// <param name="other">A triggerben lévõ másik objektum.</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlantEaterAnimal>() != null)
        {
            parentHandler?.OnGroupFindTriggerStay(other);
        }
    }


    /// <summary>
    /// Kezeli a triggerbõl való kilépés eseményét.
    /// </summary>
    /// <param name="other">A triggerbõl kilépõ másik objektum.</param>
    private void OnTriggerExit(Collider other)
    {
        parentHandler?.OnGroupFindTriggerExit(other);
    }
}
