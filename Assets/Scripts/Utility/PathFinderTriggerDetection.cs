using UnityEngine;

/// <summary>
/// Az útkeresés trigger eseményeinek kezeléséért felelõs osztály.
/// </summary>
public class PathFinderTriggerDetection : MonoBehaviour
{
    /// <summary>
    /// Az útkereséshez tartozó szülõ kezelõ.
    /// </summary>
    [SerializeField]private PlantEaterAnimal parentHandler;

    /// <summary>
    /// Inicializálja a szülõ kezelõt az ébredéskor.
    /// </summary>
    private void Awake()
    {
        parentHandler = this.GetComponentInParent<PlantEaterAnimal>();
    }


    /// <summary>
    /// Kezeli a triggerbe való belépés eseményét.
    /// </summary>
    /// <param name="other">A triggerbe belépõ másik objektum.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(parentHandler.pathFindingTrigger.gameObject.name == this.gameObject.name) parentHandler?.OnPathFindTriggerEnter(other);
    }
}
