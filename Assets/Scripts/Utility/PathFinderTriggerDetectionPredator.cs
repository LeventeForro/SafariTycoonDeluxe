using UnityEngine;

/// <summary>
/// Az útkeresés trigger eseményeinek kezeléséért felelõs osztály ragadozók számára.
/// </summary>
public class PathFinderTriggerDetectionPredator : MonoBehaviour
{
    /// <summary>
    /// Az útkereséshez tartozó szülõ kezelõ.
    /// </summary>
    private Predator parentHandler;

    /// <summary>
    /// Inicializálja a szülõ kezelõt az ébredéskor.
    /// </summary>
    private void Awake()
    {
        parentHandler = this.GetComponentInParent<Predator>();
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
