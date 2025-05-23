using UnityEngine;

/// <summary>
/// Az �tkeres�s trigger esem�nyeinek kezel�s��rt felel�s oszt�ly ragadoz�k sz�m�ra.
/// </summary>
public class PathFinderTriggerDetectionPredator : MonoBehaviour
{
    /// <summary>
    /// Az �tkeres�shez tartoz� sz�l� kezel�.
    /// </summary>
    private Predator parentHandler;

    /// <summary>
    /// Inicializ�lja a sz�l� kezel�t az �bred�skor.
    /// </summary>
    private void Awake()
    {
        parentHandler = this.GetComponentInParent<Predator>();
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
