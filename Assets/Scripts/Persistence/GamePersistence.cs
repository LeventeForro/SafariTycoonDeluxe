using TMPro;
using UnityEngine;

/// <summary>
/// A j�t�k �llapot�nak ment�s��rt �s bet�lt�s��rt felel�s oszt�ly.
/// </summary>
public class GamePersistence : MonoBehaviour
{
    /// <summary>
    /// A zebra prefab.
    /// </summary>
    public GameObject zebraPrefab;

    /// <summary>
    /// A zsir�f prefab.
    /// </summary>
    public GameObject giraffePrefab;

    /// <summary>
    /// A strucc prefab.
    /// </summary>
    public GameObject ostrichPrefab;

    /// <summary>
    /// A gazella prefab.
    /// </summary>
    public GameObject gazellePrefab;

    /// <summary>
    /// A krokodil prefab.
    /// </summary>
    public GameObject crocoPrefab;

    /// <summary>
    /// Az oroszl�n prefab.
    /// </summary>
    public GameObject lionPrefab;

    /// <summary>
    /// A leop�rd prefab.
    /// </summary>
    public GameObject leopardPrefab;

    /// <summary>
    /// A bokor prefab.
    /// </summary>
    public GameObject bushPrefab;

    /// <summary>
    /// A f� prefab.
    /// </summary>
    public GameObject grassPrefab;

    /// <summary>
    /// A majomkeny�rfa prefab.
    /// </summary>
    public GameObject majomkenyerfaPrefab;

    /// <summary>
    /// Az ak�ciafa prefab.
    /// </summary>
    public GameObject akaciafaPrefab;

    /// <summary>
    /// Az �t prefab.
    /// </summary>
    public GameObject roadPrefab;

    /// <summary>
    /// A t� prefab.
    /// </summary>
    public GameObject lakePrefab;

    /// <summary>
    /// A Jeep prefab.
    /// </summary>
    public GameObject jeepPrefab;

    /// <summary>
    /// A ment�skezel�.
    /// </summary>
    public SaveManager saveManager;

    /// <summary>
    /// Inicializ�lja a j�t�k ment�s�t �s bet�lt�s�t.
    /// </summary>
    private void Start()
    {
        if (LoadGameFlag.ShouldLoadGame)
        {
            LoadGameFlag.ShouldLoadGame = false;
            Load();
        }
    }

    /// <summary>
    /// Elmenti a j�t�k �llapot�t.
    /// </summary>
    public void Save()
    {
        GameObject.FindAnyObjectByType<UIController>().auSource.clip = GameObject.FindAnyObjectByType<UIController>().soundEffects[0];
        GameObject.FindAnyObjectByType<UIController>().auSource.Play();

        GameObject.FindAnyObjectByType<GridController>().buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
        GameObject.FindAnyObjectByType<GridController>().buyAnimatedText.GetComponent<TMP_Text>().text = "J�t�k elmentve.";
        SaveData data = new SaveData();

        foreach (var animal in FindObjectsOfType<PlantEaterAnimal>())
        {
            if (animal.enabled)
                data.animals.Add(animal.GetAnimalData());
        }
        foreach (var animal in FindObjectsOfType<Predator>())
        {
            if (animal.enabled)
                data.animals.Add(animal.GetAnimalData());
        }

        foreach (var road in FindObjectsOfType<RoadTile>())
        {
            data.roads.Add(road.GetRoadPos());
        }

        foreach (var lake in FindObjectsOfType<Lake>())
        {
            data.lakes.Add(lake.GetLakes());
        }

        foreach (var jeep in FindObjectsOfType<Jeep>())
        {
            data.jeeps.Add(jeep.GetJeep());
        }

        foreach (var plant in FindObjectsOfType<Plant>())
        {
            data.plants.Add(plant.GetPlantData());
        }

        data.cameraPosition = new Vector3Data(Camera.main.transform.position);
        data.money = GameObject.FindAnyObjectByType<PlayerController>().playerCash;
        data.hours = GameObject.FindAnyObjectByType<PlayerController>().hours;
        data.days = GameObject.FindAnyObjectByType<PlayerController>().days;
        data.weeks = GameObject.FindAnyObjectByType<PlayerController>().weeks;
        saveManager.SaveGame(data);
    }

    /// <summary>
    /// Bet�lti a j�t�k �llapot�t.
    /// </summary>
    public void Load()
    {
        SaveData data = saveManager.LoadGame();
        if (data == null) return;

        ClearAllObjects();

        foreach (var a in data.animals)
        {
            SpawnAnimal(a.type, a.position.ToVector3());
        }

        foreach (var p in data.plants)
        {
            SpawnPlant(p.type, p.position.ToVector3());
        }

        foreach (var r in data.roads)
        {
            SpawnRoad(r.name, r.position.ToVector3());
        }

        foreach (var l in data.lakes)
        {
            SpawnLake(l.ToVector3());
        }

        foreach (var j in data.jeeps)
        {
            SpawnJeep(j.ToVector3());
        }

        Camera.main.transform.position = data.cameraPosition.ToVector3();
        GameObject.FindAnyObjectByType<PlayerController>().playerCash = data.money;
        GameObject.FindAnyObjectByType<PlayerController>().hours = data.hours;
        GameObject.FindAnyObjectByType<PlayerController>().days = data.days;
        GameObject.FindAnyObjectByType<PlayerController>().weeks = data.weeks;
    }

    /// <summary>
    /// T�rli az �sszes objektumot az �jrat�lt�shez.
    /// </summary>
    private void ClearAllObjects()
    {
        foreach (var animal in FindObjectsOfType<PlantEaterAnimal>())
        {
            Destroy(animal.gameObject);
        }

        foreach (var animal in FindObjectsOfType<Predator>())
        {
            Destroy(animal.gameObject);
        }

        foreach (var road in FindObjectsOfType<RoadTile>())
        {
            Destroy(road.gameObject);
        }

        foreach (var lake in FindObjectsOfType<Lake>())
        {
            Destroy(lake.gameObject);
        }

        foreach (var jeep in FindObjectsOfType<Jeep>())
        {
            Destroy(jeep.gameObject);
        }

        foreach (var plant in FindObjectsOfType<Plant>())
        {
            Destroy(plant.gameObject);
        }
    }

    /// <summary>
    /// L�trehoz egy �llatot a megadott t�pus �s poz�ci� alapj�n.
    /// </summary>
    /// <param name="type">Az �llat t�pusa.</param>
    /// <param name="position">Az �llat poz�ci�ja.</param>
    private void SpawnAnimal(string type, Vector3 position)
    {
        GameObject prefabToSpawn = null;

        switch (type.ToUpper())
        {
            case "ZEBRA":
                prefabToSpawn = zebraPrefab;
                break;
            case "GIRAFFE":
                prefabToSpawn = giraffePrefab;
                break;
            case "OSTRICH":
                prefabToSpawn = ostrichPrefab;
                break;
            case "GAZELLE":
                prefabToSpawn = gazellePrefab;
                break;
            case "CROCO":
                prefabToSpawn = crocoPrefab;
                break;
            case "LION":
                prefabToSpawn = lionPrefab;
                break;
            case "LEOPARD":
                prefabToSpawn = leopardPrefab;
                break;
            default:
                Debug.LogWarning("Ismeretlen �llat t�pusa: " + type);
                return;
        }

        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }

    /// <summary>
    /// L�trehoz egy n�v�nyt a megadott t�pus �s poz�ci� alapj�n.
    /// </summary>
    /// <param name="type">A n�v�ny t�pusa.</param>
    /// <param name="position">A n�v�ny poz�ci�ja.</param>
    private void SpawnPlant(string type, Vector3 position)
    {
        GameObject prefabToSpawn = null;

        switch (type.ToUpper())
        {
            case "BOKOR":
                prefabToSpawn = bushPrefab;
                break;
            case "FU":
                prefabToSpawn = grassPrefab;
                break;
            case "MAJOMKENYERFA":
                prefabToSpawn = majomkenyerfaPrefab;
                break;
            case "AKACIAFA":
                prefabToSpawn = akaciafaPrefab;
                break;
            default:
                Debug.LogWarning("Ismeretlen n�v�ny t�pusa: " + type);
                return;
        }

        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }

    /// <summary>
    /// L�trehoz egy utat a megadott n�v �s poz�ci� alapj�n.
    /// </summary>
    /// <param name="name">Az �t neve.</param>
    /// <param name="position">Az �t poz�ci�ja.</param>
    private void SpawnRoad(string name, Vector3 position)
    {
        GameObject prefabToSpawn = roadPrefab;
        prefabToSpawn.name = name;
        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }

    /// <summary>
    /// L�trehoz egy tavat a megadott poz�ci� alapj�n.
    /// </summary>
    /// <param name="position">A t� poz�ci�ja.</param>
    private void SpawnLake(Vector3 position)
    {
        GameObject prefabToSpawn = lakePrefab;
        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }

    /// <summary>
    /// L�trehoz egy Jeepet a megadott poz�ci� alapj�n.
    /// </summary>
    /// <param name="position">A Jeep poz�ci�ja.</param>
    private void SpawnJeep(Vector3 position)
    {
        GameObject prefabToSpawn = jeepPrefab;
        Instantiate(prefabToSpawn, position, Quaternion.identity);
    }
}