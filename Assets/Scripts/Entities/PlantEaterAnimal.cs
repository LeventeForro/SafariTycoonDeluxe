using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Az állat állapotait reprezentáló felsorolás.
/// </summary>
public enum AnimalStates
{
    /// <summary>Állás állapot.</summary>
    IDLE,
    /// <summary>Bolyongás állapot.</summary>
    WANDER,
    /// <summary>Ételhez való mozgás állapot.</summary>
    GO_TO_FOOD,
    /// <summary>Fogyasztás állapot.</summary>
    CONSUME,
    /// <summary>Halál állapot.</summary>
    DIE
};

/// <summary>
/// A növényevő állatok típusait reprezentáló felsorolás.
/// </summary>
public enum PlantEaterAnimalTypes
{
    /// <summary>Zebra típusa.</summary>
    ZEBRA,
    /// <summary>Strucc típusa.</summary>
    OSTRICH,
    /// <summary>Zsiráf típusa.</summary>
    GIRAFFE,
    /// <summary>Gazella típusa.</summary>
    GAZELLE
};

/// <summary>
/// Egy növényevő állatot reprezentáló osztály.
/// </summary>
public class PlantEaterAnimal : MonoBehaviour
{
    [Header("Alpavető adatok")]
    /// <summary>Az állat életkora.</summary>
    public int age;
    /// <summary>Az állat típusa.</summary>
    public PlantEaterAnimalTypes type;
    /// <summary>Az állat éhségszintje.</summary>
    public int hunger;
    /// <summary>Az állat szomjúságszintje.</summary>
    public int thirst;
    /// <summary>Az állat életben van-e.</summary>
    public bool isAlive;
    /// <summary>Az állat maximális éhségszintje.</summary>
    public int maxHunger;
    /// <summary>Az állat maximális szomjúságszintje.</summary>
    public int maxThirst;
    /// <summary>Az állat aktuális állapota.</summary>
    public AnimalStates state;
    /// <summary>Az állat ára.</summary>
    public int myPrice;

    [Header("Mozgás")]
    /// <summary>Az állat mozgási sebessége.</summary>
    public float moveSpeed;
    [SerializeField]PlayerController playerController;
    Rigidbody rb;
    Terrain terrain;
    Collider oceanCollider;
    List<GameObject> terrainColliders;
    private SpriteRenderer spriteRenderer;
    [SerializeField] bool isRiverPath;
    [SerializeField] bool isHillPath;

    [Header("Kaja és pia")]
    /// <summary>Az állat által látott ételek listája.</summary>
    public List<GameObject> seenFoods;
    /// <summary>Az állat által látott tavak listája.</summary>
    public List<GameObject> seenLakes;
    /// <summary>Az állat által felfedezett tavak listája.</summary>
    public List<GameObject> exploredLakes;
    /// <summary>Az állat által felfedezett ételek listája.</summary>
    public List<GameObject> exploredFoods;
    /// <summary>Az állat által választott étel helye.</summary>
    public GameObject chosedFoodPlace;
    /// <summary>Az állat által választott tó helye.</summary>
    public GameObject chosedLakePlace;
    public List<GameObject> pathFindTriggers = new List<GameObject>();
    public GameObject pathFindingTrigger;
    public bool isHungry;
    public bool isThirsty;
    public Vector3 consumePlace;

    //Szaporodás
    [Header("Szaporodás")]
    /// <summary>Az állat születési száma.</summary>
    public int birthCount;
    /// <summary>Az állat születési véletlenszáma.</summary>
    public int birthRandom;
    /// <summary>Az állat gyerekeinek objektuma.</summary>
    public GameObject myBaby;

    [Header("Állat UI")]
    /// <summary>Az állat statisztikáit megjelenítő vászon.</summary>
    public Canvas myStatCanvas;
    /// <summary>Az éhségszintet megjelenítő csúszka.</summary>
    public Slider hungerSlider;
    /// <summary>A szomjúságszintet megjelenítő csúszka.</summary>
    public Slider thirstSlider;
    /// <summary>Az életkort megjelenítő csúszka.</summary>
    public Slider ageSlider;

    /// <summary>Egy ideiglenes időzitő.</summary>
    int tempTimer = -1;
    /// <summary>Egy időzitő.</summary>
    float realTimer = 0;
    /// <summary>Eltelt másodpercek</summary>
    int elapsedSeconds = 0;
    /// <summary>Ideiglenes hét számláló</summary>
    int tempWeeks = 1;

    private float nextUpdateTime = 0f;

    [SerializeField] int waitToWanderFood;
    [SerializeField] int waitToWanderThirst;

    [SerializeField] List<GameObject> otherAnimalsOfMySpecies;

    //Csordaszellem változói
    Vector3 followedPos;
    float followedGoalTime;
    public Coroutine cor;

    public Vector3 globalFPos;
    public float globalGoalTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Az állat inicializálása.
    /// </summary>
    private void Start()
    {
        state = AnimalStates.IDLE;
        this.hunger = 100;
        this.thirst = 100;
        this.maxThirst = this.thirst;
        this.isAlive = true;
        this.moveSpeed = 10f;
        this.maxHunger = hunger + age;
        
        
        waitToWanderFood = Random.Range(maxHunger - 40, maxHunger - 20); // Először 60-tól  indul, mert ahogy öregszik, ez a szám változik és 80-ig megy.
        waitToWanderThirst = Random.Range(60, 80);
        if (waitToWanderThirst % 2 != 0) ++waitToWanderThirst;
        terrain = GameObject.FindAnyObjectByType<Terrain>();
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        GameObject[] tempC = GameObject.FindGameObjectsWithTag("TerrainCollider");
        birthCount = Random.Range(0, 5);
        birthRandom = Random.Range(80, 85);

        tempWeeks = playerController.GetComponent<PlayerController>().weeks;

        terrainColliders = new List<GameObject>();

        for(int i = 0; i < tempC.Length; ++i)
            terrainColliders.Add(tempC[i]);

        pathFindingTrigger = GetComponentInChildren<PathFinderTriggerDetection>().gameObject;
        switch(type)
        {
            case PlantEaterAnimalTypes.ZEBRA:
                myPrice = 500;
                myBaby = GameObject.Find("ZebraSuc");
                break;
            case PlantEaterAnimalTypes.OSTRICH:
                myPrice = 1450;
                myBaby = GameObject.Find("OstrichSuc");
                break;
            case PlantEaterAnimalTypes.GIRAFFE:
                myPrice = 750;
                myBaby = GameObject.Find("GiraffeSuc");
                break;
            case PlantEaterAnimalTypes.GAZELLE:
                myPrice = 1000;
                myBaby = GameObject.Find("GazelleSuc");
                break;
        }
        
    }

    /// <summary>
    /// Az állat frissítése minden képkockában.
    /// </summary>
    private void Update() // Renderhez kötött frissítés
    {
        if(!playerController.isPaused)
        {
            Debug.Log("Állat frissítése");
            realTimer += Time.deltaTime;
            elapsedSeconds = Mathf.FloorToInt(realTimer % 60);

            if (Time.time >= nextUpdateTime)// Másodpercenként egyszer fogja meghívni az itt lévő cuccokat <- ÚJ IDŐZÍTŐ
            {
                nextUpdateTime = Time.time + 1f;

                LifeFunctionUpdate();

                if ((hunger <= 20 || thirst <= 20) && state != AnimalStates.CONSUME && state != AnimalStates.WANDER)
                {
                    if (hunger <= 20 && !isThirsty) isHungry = true;
                    if (thirst <= 20) isThirsty = true;
                    FindPath();
                }
                if (state == AnimalStates.IDLE)
                {
                    if (hunger == waitToWanderFood || thirst == waitToWanderThirst)
                    {
                        state = AnimalStates.WANDER;
                        globalGoalTime = Random.Range(3, 5);
                        globalFPos = GenerateValidWanderPosition(transform.position, 15);
                        StartCoroutine(AnimalMoving(globalGoalTime, globalFPos));
                        MoveTheGroup();
                    }
                }

                tempTimer = elapsedSeconds;
            }
        }
    }

    /// <summary>
    /// Az állat eladása.
    /// </summary>
    public void SellMyself()
    {
        playerController.playerCash += myPrice;
        Destroy(gameObject);
    }

    /// <summary>
    /// Egy véletlenszerű és érvényes vándorlási pozíció generálása az állat számára.
    /// </summary>
    /// <param name="startPos">Az állat kiinduló pozíciója, amely körül a véletlenszerű pozíciót generáljuk.</param>
    /// <param name="diff">A kiinduló pozíciótól való maximális távolság, amelyen belül a véletlenszerű pozíciót választhatjuk.</param>
    /// <returns>A generált érvényes vándorlási pozíció (X, Y, Z koordinátákkal).</returns>
    private Vector3 GenerateValidWanderPosition(Vector3 startPos, int diff)
    {
        Vector3 randomPos = Vector3.zero;

        bool isGood = false;

        do
        {
            float x = Random.Range(startPos.x - diff, (startPos.x + diff));
            float y = 6f;
            float z = Random.Range(startPos.z - diff, (startPos.z + diff));

            for(int i = 0; i < terrainColliders.Count; ++i)
            {
                if(terrainColliders[i].GetComponent<Collider>().bounds.Contains(new Vector3(x, y, z)))
                {
                    isGood = true;
                    randomPos = new Vector3(x, y, z);
                    break;
                }
            }
        } while (!isGood);
        return randomPos;
    }

    /// <summary>
    /// Az állat mozgása egy adott cél felé.
    /// </summary>
    /// <param name="goalTime">A cél eléréséhez szükséges idő.</param>
    /// <param name="followPos">A cél pozíciója.</param>
    /// <returns>Korutin az állat mozgásához.</returns>
    public IEnumerator AnimalMoving(float goalTime, Vector3 followPos)
    {
        this.state = AnimalStates.WANDER;
        Vector3 startPos = transform.position;
        Vector3 endPos = followPos;

        if(endPos.x > transform.position.x && endPos.z < transform.position.z) spriteRenderer.flipX = true;
        else if(endPos.x < transform.position.x && endPos.z > transform.position.z) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = false;

            float elapsedTime = 0;
        while (elapsedTime < goalTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / goalTime));
            elapsedTime += Time.deltaTime;
            if (isRiverPath || isHillPath)
            {
                goalTime = goalTime * 2f;
            }
            yield return null;
        }
        this.state = AnimalStates.IDLE; // Ha még nem éhes vagy szomis
    }

    /// <summary>
    /// Az állatok mozgását irányítja a csoportos viselkedés alapján, figyelembe véve a különböző állapotokat.
    /// </summary>
    private void MoveTheGroup()
    {
        if (state == AnimalStates.GO_TO_FOOD)
        {
            for (int i = 0; i < otherAnimalsOfMySpecies.Count; ++i)
            {
                Vector3 randomConsumePlace = GenerateValidWanderPosition(consumePlace, 2);
                if (otherAnimalsOfMySpecies != null) otherAnimalsOfMySpecies[i].GetComponent<PlantEaterAnimal>().cor = StartCoroutine(otherAnimalsOfMySpecies[i].GetComponent<PlantEaterAnimal>().AnimalMoving(Random.Range(3, 5), randomConsumePlace));
            }
        }
        else if (state == AnimalStates.WANDER)
        {
            for (int i = 0; i < otherAnimalsOfMySpecies.Count; ++i)
            {
                Vector3 randomFPos = GenerateValidWanderPosition(globalFPos, 5);
                if(otherAnimalsOfMySpecies != null) otherAnimalsOfMySpecies[i].GetComponent<PlantEaterAnimal>().cor = StartCoroutine(otherAnimalsOfMySpecies[i].GetComponent<PlantEaterAnimal>().AnimalMoving(globalGoalTime, randomFPos));
            }
        }
    }

    /// <summary>
    /// Az állat életfunkcióinak frissítése, például éhség, szomjúság, öregedés és életerősség.
    /// </summary>
    private void LifeFunctionUpdate()
    {
        
        if(isAlive)
        {
            if (myStatCanvas.gameObject.activeSelf) UpdateStatUI();
            if (isRiverPath)
            {
                if (thirst + 4 > 100) thirst += 4;
                else thirst = 100;
            }
            // Adatok frissítése
            if (elapsedSeconds % 2 == 0 && state != AnimalStates.CONSUME)
            {
                --hunger;
                thirst -= 2;
                Breed();
            }
            //Öregedés
            if (playerController != null)
            {
                if (tempWeeks != playerController.weeks) // Elég csak ennyi, mert az állatok hetente öregednek
                {
                    ++age;
                    this.maxHunger = hunger + (age * 4);
                    hungerSlider.maxValue = this.maxHunger;
                    tempWeeks = playerController.weeks;
                }
            }

            //Életerősség ellenőrzése
            if (hunger <= 0 || thirst <= 0 || age >= 30) isAlive = false;

            if(state  == AnimalStates.CONSUME)
            {
                Consume();
            }
        }
        else
        {
            state = AnimalStates.DIE;
            Die();
        }
    }

    /// <summary>
    /// Frissíti az állat statisztikai adatainak megjelenítését a felhasználói felületen.
    /// </summary>
    void UpdateStatUI()
    {
        hungerSlider.value = hunger;
        thirstSlider.value = thirst;
        ageSlider.value = age;
    }

    /// <summary>
    /// Az állat útvonaltervezését végzi el, hogy elérje a szükséges ételt vagy vizet.
    /// </summary>
    private void FindPath()
    {
        if(isHungry && state != AnimalStates.GO_TO_FOOD)
        {
            pathFindingTrigger.SetActive(true);
            if (exploredFoods.Count > 0)
            {
                bool a = false;
                List<GameObject> tempFoods = new List<GameObject>();
                for(int i = 0; i < exploredFoods.Count; ++i)
                {
                    if(seenFoods.Contains(exploredFoods[i])) { a = true; tempFoods.Add(exploredFoods[i]); }
                }
                if(a)
                {
                    GameObject tempFoodPos = tempFoods[0];
                    for (int i = 0; i < tempFoods.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, tempFoodPos.transform.position) > Vector3.Distance(transform.position, tempFoods[i].transform.position))
                        {
                            tempFoodPos = tempFoods[i];
                        }
                    }

                    if (state != AnimalStates.GO_TO_FOOD)
                    {
                        cor = StartCoroutine(GoToFood(tempFoodPos));
                    }
                }
                else
                {
                    if(seenFoods.Count > 0)
                    {
                        GameObject tempFoodPos = seenFoods[0];
                        for (int i = 0; i < seenFoods.Count; i++)
                        {
                            if (Vector3.Distance(transform.position, tempFoodPos.transform.position) > Vector3.Distance(transform.position, seenFoods[i].transform.position))
                            {
                                tempFoodPos = seenFoods[i];
                            }
                        }

                        if (state != AnimalStates.GO_TO_FOOD)
                        {
                            cor =StartCoroutine(GoToFood(tempFoodPos));
                        }
                    }
                }
            }
            else // Még semmit sem fedezett fel, ezért megnézi, hogy mik vannak körülötte
            {
                if (seenFoods.Count > 0)
                {
                    GameObject tempFoodPos = seenFoods[0];
                    for (int i = 0; i < seenFoods.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, tempFoodPos.transform.position) > Vector3.Distance(transform.position, seenFoods[i].transform.position))
                        {
                            tempFoodPos = seenFoods[i];
                        }
                    }

                    if (state != AnimalStates.GO_TO_FOOD)
                    {
                        cor = StartCoroutine(GoToFood(tempFoodPos));
                    }
                }
            }
        }
        //IVÁS
        if (isThirsty && state != AnimalStates.GO_TO_FOOD)
        {
            pathFindingTrigger.SetActive(true);
            if (exploredLakes.Count > 0)
            {
                bool a = false;
                List<GameObject> tempLakes = new List<GameObject>();
                for (int i = 0; i < exploredLakes.Count; ++i)
                {
                    if (seenLakes.Contains(exploredLakes[i])) { a = true; tempLakes.Add(exploredLakes[i]); }
                }
                if (a)
                {
                    GameObject tempLakePos = tempLakes[0];
                    for (int i = 0; i < tempLakes.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, tempLakePos.transform.position) > Vector3.Distance(transform.position, tempLakes[i].transform.position))
                        {
                            tempLakePos = tempLakes[i];
                        }
                    }

                    if (state != AnimalStates.GO_TO_FOOD)
                    {
                        cor = StartCoroutine(GoToFood(tempLakePos));
                    }
                }
                else
                {
                    if (seenLakes.Count > 0)
                    {
                        GameObject tempLakePos = seenLakes[0];
                        for (int i = 0; i < seenLakes.Count; i++)
                        {
                            if (Vector3.Distance(transform.position, tempLakePos.transform.position) > Vector3.Distance(transform.position, seenLakes[i].transform.position))
                            {
                                tempLakePos = seenLakes[i];
                            }
                        }

                        if (state != AnimalStates.GO_TO_FOOD)
                        {
                            cor = StartCoroutine(GoToFood(tempLakePos));
                        }
                    }
                }
            }
            else // Még semmit sem fedezett fel, ezért megnézi, hogy mik vannak körülötte
            {
                if (seenLakes.Count > 0)
                {
                    GameObject tempLakePos = seenLakes[0];
                    for (int i = 0; i < seenLakes.Count; i++)
                    {
                        if (Vector3.Distance(transform.position, tempLakePos.transform.position) > Vector3.Distance(transform.position, seenLakes[i].transform.position))
                        {
                            tempLakePos = seenLakes[i];
                        }
                    }

                    if (state != AnimalStates.GO_TO_FOOD)
                    {
                        cor = StartCoroutine(GoToFood(tempLakePos));
                    }
                }
            }
        }
        seenFoods.Clear();
        seenLakes.Clear();
    }

    /// <summary>
    /// Az állat elindul a megadott étel vagy víz célpontjához, majd a célba érkezés után elindítja a fogyasztási állapotot.
    /// </summary>
    /// <param name="destination">A célpont (étel vagy víz) GameObject-je, ahová az állat el akar jutni.</param>
    private IEnumerator GoToFood(GameObject destination)
    {
        state = AnimalStates.GO_TO_FOOD;
        consumePlace = destination.transform.position;
        pathFindingTrigger.SetActive(false);
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(Random.Range((int)destination.transform.position.x - 2, (int)destination.transform.position.x + 2), transform.position.y, Random.Range((int)destination.transform.position.z - 2, (int)destination.transform.position.z + 2));

        float goalTime = Vector3.Distance(transform.position, endPos) / 5; // Fizika 5-ös

        float elapsedTime = 0;
        while (Vector3.Distance(transform.position, endPos) != 0)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / goalTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (isHungry)
        {
            if(!exploredFoods.Contains(destination)) exploredFoods.Add(destination);
            chosedFoodPlace = destination;
        }
        else if (isThirsty)
        {
            if(!exploredLakes.Contains(destination)) exploredLakes.Add(destination);
            chosedLakePlace = destination;
        }
        state = AnimalStates.CONSUME;
        waitToWanderFood = Random.Range(maxHunger - 40, maxHunger - 20);
        waitToWanderThirst = Random.Range(60, 80);
        if (waitToWanderThirst % 2 != 0) ++waitToWanderThirst;

    }

    /// <summary>
    /// Fogyasztás logikát valósít meg az állat számára. 
    /// Ha az állat éhes, eszik, ha szomjas, vizet iszik.
    /// Az éhség és a szomjúság csökkenése után az állat visszatér az inaktív állapotba, ha egyik sem áll fent.
    /// </summary>
    private void Consume()
    {
        if(isHungry)
        {
            if(hunger < maxHunger) // Ha majd meggondoljuk, akkor lehet majd ezzel is, de úgy bonyolultabb -> chosedFoodPlace.GetComponent<Plant>().count > 0
            {
                hunger += 10;
                if(hunger > maxHunger) hunger = maxHunger;
            }
            else
            {
                isHungry = false;
                if (!isThirsty) state = AnimalStates.IDLE;
            }
        }
        else if (isThirsty)
        {
            if (thirst < maxThirst) // Ha majd meggondoljuk, akkor lehet majd ezzel is, de úgy bonyolultabb -> chosedFoodPlace.GetComponent<Plant>().count > 0
            {
                thirst += 10;
                if (thirst > maxThirst) thirst = maxThirst;
            }
            else
            {
                isThirsty = false;
                if (!isHungry) state = AnimalStates.IDLE;
            }
        }
    }

    /// <summary>
    /// Trigger esemény, amely akkor történik, amikor az állat egy élelemhez közelít.
    /// Hozzáadja az élelmet a 'seenFoods' listához, ha még nem szerepel benne.
    /// </summary>
    /// <param name="collider">Az élelemhez tartozó collider objektum</param>
    public void OnPathFindTriggerEnter(Collider collider)
    {
        if(collider.tag == "Food" && !seenFoods.Contains(collider.gameObject)) seenFoods.Add(collider.gameObject);
        if (collider.tag == "Lake" && !seenLakes.Contains(collider.gameObject)) seenLakes.Add(collider.gameObject);
        if (collider.tag == "Hill")
        {
            pathFindingTrigger = pathFindTriggers[1];
            isHillPath = true;
        }
    }

    /// <summary>
    /// Trigger esemény, amely akkor történik, amikor az állat egy másik állat csoportjához tartozó taghoz közelít.
    /// Hozzáadja a másik állatot a saját csoportjához, ha ugyanahhoz a fajhoz tartozik.
    /// </summary>
    /// <param name="collider">A csoporthoz tartozó állat collider objektuma</param>
    public void OnGroupFindTriggerStay(Collider collider)
    {
        if (!otherAnimalsOfMySpecies.Contains(collider.gameObject) && collider.GetComponent<PlantEaterAnimal>().type == this.type) otherAnimalsOfMySpecies.Add(collider.gameObject);
        if (collider.tag == "River")
        {
            isRiverPath = true;
        }
    }

    /// <summary>
    /// Trigger esemény, amely akkor történik, amikor egy másik állat elhagyja a csoport közelét.
    /// Először ellenőrzi, hogy az állat a csoporthoz tartozik-e, és ha igen, eltávolítja.
    /// </summary>
    /// <param name="collider">A csoporttól távozó állat collider objektuma</param>
    public void OnGroupFindTriggerExit(Collider collider)
    {
        if(elapsedSeconds > 0) if (otherAnimalsOfMySpecies.Contains(collider.gameObject) && collider.GetComponent<PlantEaterAnimal>().type == this.type) otherAnimalsOfMySpecies.Remove(collider.gameObject);
        if (collider.tag == "Hill")
        {
            pathFindingTrigger = pathFindTriggers[0];
            isHillPath = false;
        }
        if (collider.tag == "River") isRiverPath = false;
    }

    /// <summary>
    /// Az állat halála után megsemmisíti az állat objektumot.
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Az állat szaporodása.
    /// </summary>
    public void Breed()
    {
        if(otherAnimalsOfMySpecies.Count != 0 && hunger == birthRandom && birthCount > 0 && state == AnimalStates.IDLE && age > 15)
        {
            GameObject chosedLove = otherAnimalsOfMySpecies[Random.Range(0, otherAnimalsOfMySpecies.Count - 1)];
            if(chosedLove.GetComponent<PlantEaterAnimal>().birthCount > 0 && chosedLove.GetComponent<PlantEaterAnimal>().age > 15)
            {
                GameObject placedBaby = Instantiate(myBaby, this.transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag(type.ToString()).transform);
                placedBaby.GetComponent<PlantEaterAnimal>().enabled = true;
                placedBaby.GetComponent<PlantEaterAnimal>().age = 0;
                placedBaby.name = type.ToString();

                Debug.Log("Megvolt a dolog és most lett egy új gyerek");
                --birthCount;
            }
        }
    }

    /// <summary>
    /// Az állat adatainak lekérése.
    /// </summary>
    /// <returns>Az állat adatait tartalmazó <see cref="AnimalData"/> objektum.</returns>
    public AnimalData GetAnimalData() // Módosítás - Persistence
    {
        return new AnimalData
        {
            type = this.type.ToString(),
            position = new Vector3Data(transform.position)
        };
    }
}
