using UnityEngine;

/// <summary>
/// Az �tkeres�s trigger esem�nyeinek kezel�s��rt felel�s oszt�ly.
/// </summary>
public class PathFinderTriggerDetection : MonoBehaviour
{
    /// <summary>
    /// Az �tkeres�shez tartoz� sz�l� kezel�.
    /// </summary>
    [SerializeField]private PlantEaterAnimal parentHandler;

    /// <summary>
    /// Inicializ�lja a sz�l� kezel�t az �bred�skor.
    /// </summary>
    private void Awake()
    {
        parentHandler = this.GetComponentInParent<PlantEaterAnimal>();
    }


    /// <summary>
    /// Kezeli a triggerbe val� bel�p�s esem�ny�t.
    /// </summary>
    /// <param name="other">A triggerbe bel�p� m�sik objektum.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(parentHandler.pathFindingTrigger.gameObject.name == this.gameObject.name) parentHandler?.OnPathFindTriggerEnter(other);
    }
}
