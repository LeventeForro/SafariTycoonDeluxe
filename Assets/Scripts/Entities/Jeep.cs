using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Jeep viselkedését vezérlő osztály, amely képes turistákat szállítani az úthálózaton belül.
/// </summary>
public class Jeep : MonoBehaviour
{
    /// <summary>
    /// A Jeep mozgási sebessége.
    /// </summary>
    public float speed = 20f;

    /// <summary>
    /// Az aktuális útelem, amelyen a Jeep éppen tartózkodik.
    /// </summary>
    public RoadTile currentRoad;

    /// <summary>
    /// Az előző útelem, amelyen a Jeep korábban haladt.
    /// </summary>
    public RoadTile previousRoad;

    /// <summary>
    /// Jelzi, hogy a Jeep mozgásban van-e.
    /// </summary>
    public bool isMoving = false;

    /// <summary>
    /// A Jeep maximális utaskapacitása.
    /// </summary>
    public int MaxSpace = 4;

    /// <summary>
    /// Az éppen a Jeep-ben utazó turisták száma.
    /// </summary>
    public int TouristInJeep = 0;

    /// <summary>
    /// Az út vége objektum referenciája.
    /// </summary>
    public GameObject End_;

    /// <summary>
    /// Az út kezdete objektum referenciája.
    /// </summary>
    public GameObject Start_; // az út prefab

    /// <summary>
    /// Az elégedett turisták összesített száma.
    /// </summary>
    public static int satisfiedTourist = 0;

    /// <summary>
    /// A Jeep előre néző sprite-ja.
    /// </summary>
    public Sprite jeepUp;

    /// <summary>
    /// A Jeep hátra néző sprite-ja.
    /// </summary>
    public Sprite jeepDown;

    /// <summary>
    /// A Jeep balra néző sprite-ja.
    /// </summary>
    public Sprite jeepLeft;

    /// <summary>
    /// A Jeep jobbra néző sprite-ja.
    /// </summary>
    public Sprite jeepRight;

    /// <summary>
    /// A sprite megjelenítéséért felelős komponens.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// Az állatok, amelyeket látta a Jeep.
    /// </summary>
    private HashSet<Collider> triggeredAnimals = new HashSet<Collider>();


    /// <summary>
    /// Inicializáláskor beállítja a SpriteRenderert és elindítja a késleltetett inicializálást.
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("Nincs SpriteRenderer a Jeep alatt!");
        }
        StartCoroutine(InitializeAfterDelay());
        End_ = GameObject.Find("End");
        Start_ = GameObject.Find("Start");
    }

    /// <summary>
    /// Frissítési ciklus, amely elindítja a mozgást, ha a Jeep tele van turistákkal és a kezdőpontnál áll.
    /// </summary>
    public void Update()
    {
        if (TouristInJeep >= MaxSpace && Start_.transform.position == currentRoad.transform.position)
        {
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(MoveAlongRoad());
            }
        }
    }

    /// <summary>
    /// Késleltetett inicializálás, amely beállítja a legközelebbi útelemet, és elindítja a mozgást.
    /// </summary>
    public IEnumerator InitializeAfterDelay()
    {
        currentRoad = FindClosestRoadTile();
        StartCoroutine(MoveAlongRoad());

        if (currentRoad != null)
        {
            transform.position = currentRoad.transform.position;
        }
        else
        {
            Debug.Log("Nincs a jeep alatt út!!!!");
        }
        yield return null;
    }

    /// <summary>
    /// Megkeresi a legközelebbi útelemet a Jeep jelenlegi pozíciójához képest.
    /// </summary>
    /// <returns>A legközelebbi RoadTile objektum.</returns>
    RoadTile FindClosestRoadTile()
    {
        RoadTile closest = null;
        float minDist = Mathf.Infinity;
        foreach (var road in RoadTile.allRoadTiles)
        {
            float dist = Vector3.Distance(transform.position, road.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = road;
            }
        }
        return closest;
    }

    /// <summary>
    /// A Jeep automatikus mozgását végzi az útelemek között, irányválasztással, sprite frissítéssel és utaslogikával.
    /// </summary>
    public IEnumerator MoveAlongRoad()
    {
        while (isMoving)
        {
            List<RoadTile> availableDirections = new List<RoadTile>();

            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

            foreach (Vector3 dir in directions)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position + dir, 1f);

                foreach (var hit in hits)
                {
                    RoadTile other = hit.GetComponent<RoadTile>();
                    if (other != null && other != currentRoad && !availableDirections.Contains(other))
                    {
                        availableDirections.Add(other);
                    }
                }
            }

            if (availableDirections.Count > 0)
            {
                if (availableDirections.Count == 1)
                {
                    if (previousRoad != null)
                    {
                        availableDirections.Clear();
                        availableDirections.Add(previousRoad);
                    }
                }

                if (previousRoad != null && !(availableDirections.Count == 1))
                {
                    availableDirections.Remove(previousRoad);
                }

                RoadTile nextRoad = availableDirections[Random.Range(0, availableDirections.Count)];

                Vector3 direction = (nextRoad.transform.position - currentRoad.transform.position).normalized;

                // Sprite csere irány alapján
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    if (direction.x > 0)
                    {
                        spriteRenderer.sprite = jeepRight;
                    }
                    else
                    {
                        spriteRenderer.sprite = jeepLeft;
                    }
                }
                else
                {
                    if (direction.z > 0)
                    {
                        spriteRenderer.sprite = jeepUp;
                    }
                    else
                    {
                        spriteRenderer.sprite = jeepDown;
                    }
                }

                previousRoad = currentRoad;
                currentRoad = nextRoad;

                while (Vector3.Distance(transform.position, currentRoad.transform.position) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentRoad.transform.position, speed * Time.deltaTime * PlayerController.timeSpeed);
                    yield return null;
                }
            }
            if (currentRoad.transform.position == End_.transform.position)
            {
                TouristInJeep = 0;
                yield return new WaitForSeconds(5f - PlayerController.timeSpeed);
                satisfiedTourist += 4;
            }
            else if (currentRoad.transform.position == Start_.transform.position)
            {
                isMoving = false;
                transform.position = Start_.transform.position;
                yield break;
            }

            else
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// Kezeli az állatokkal való interakciót mozgás közben; ha turista van a Jeep-ben, növeli a hírnevet.
    /// </summary>
    /// <param name="other">A másik objektum ütközéskor.</param>
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Tiere") && isMoving)
        {
            // megnézzük hogy szerepel-e már a listában
            if (!triggeredAnimals.Contains(other))
            {
                triggeredAnimals.Add(other); // hozzáadjuk a listához
                Debug.Log("Animal tag detected");

                if (TouristInJeep > 0)
                {
                    Debug.Log("TouristInJeep > 0");
                    Base.reputation += 0.1f;
                    Debug.Log($"Reputation increased: {Base.reputation}");
                }
            }
        }
    }

    /// <summary>
    /// Visszaadja a Jeep kezdőpozícióját Vector3Data formátumban (mentéshez).
    /// </summary>
    /// <returns>A Jeep pozíciója Vector3Data formátumban.</returns>
    public Vector3Data GetJeep() // Módosítás - Persistence
    {
        return new Vector3Data(Start_.transform.position);
    }
}
