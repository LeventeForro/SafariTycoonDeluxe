using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A ragadozó állat típusát jelöli (oroszlán, leopárd, krokodil).
/// </summary>
public enum PredatorAnimalTypes
{
    LION,
    LEOPARD,
    CROCO
}


/// <summary>
/// Predátor állat osztály, amely kezeli a mozgást, éhséget, szomjúságot, szaporodást és interakciókat.
/// </summary>
public class Predator : MonoBehaviour
{

    [Header("Alapvetõ adatok")]
    /// <summary> Az állat életkora. </summary>
    public int age;
    /// <summary> A ragadozó típusa. </summary>
    public PredatorAnimalTypes type;
    /// <summary> Éhség szint. </summary>
    public int hunger;
    /// <summary> Szomjúság szint. </summary>
    public int thirst;
    /// <summary> Az állat életben van-e. </summary>
    public bool isAlive;
    /// <summary> Maximális éhség. </summary>
    public int maxHunger;
    /// <summary> Maximális szomjúság. </summary>
    public int maxThirst;
    /// <summary> Az állat aktuális állapota. </summary>
    public AnimalStates state;
    /// <summary> Az állat eladási ára. </summary>
    public int myPrice;

    [Header("Mozgás")]
    /// <summary> Mozgási sebesség. </summary>
    public float moveSpeed;
    /// <summary> Játékos vezérlõ referencia. </summary>
    [SerializeField]PlayerController playerController;
    Rigidbody rb;
    Terrain terrain;
    Collider oceanCollider;
    List<GameObject> terrainColliders;
    private SpriteRenderer spriteRenderer;
    /// <summary> Folyó útvonalon mozog-e. </summary>
    [SerializeField] bool isRiverPath;
    /// <summary> Hegyes útvonalon mozog-e. </summary>
    [SerializeField] bool isHillPath;

    [Header("Kaja és pia")]
    /// <summary> Látott ételek listája. </summary>
    public List<GameObject> seenFoods;
    /// <summary> Látott tavak listája. </summary>
    public List<GameObject> seenLakes;
    /// <summary> Felfedezett tavak listája. </summary>
    public List<GameObject> exploredLakes;
    /// <summary> Felfedezett ételek listája. </summary>
    public List<GameObject> exploredFoods;
    /// <summary> Kiválasztott étel helyszíne. </summary>
    public GameObject chosedFoodPlace;
    /// <summary> Kiválasztott tó helyszíne. </summary>
    public GameObject chosedLakePlace;
    public List<GameObject> pathFindTriggers = new List<GameObject>();
    public GameObject pathFindingTrigger;
    /// <summary> Éhes-e az állat. </summary>
    public bool isHungry;
    /// <summary> Szomjas-e az állat. </summary>
    public bool isThirsty;
    /// <summary> A hely, ahol az állat éppen fogyaszt. </summary>
    public Vector3 consumePlace;

    [Header("Szaporodás")]
    /// <summary> Megszületett utódok száma. </summary>
    public int birthCount;
    /// <summary> Véletlenszerû szaporodási érték. </summary>
    public int birthRandom;
    /// <summary> Az állat utóda (prefab). </summary>
    public GameObject myBaby;

    [Header("Állat UI")]
    /// <summary> A statisztikai canvas UI. </summary>
    public Canvas myStatCanvas;
    /// <summary> Az éhséget mutató csúszka. </summary>
    public Slider hungerSlider;
    /// <summary> A szomjúságot mutató csúszka. </summary>
    public Slider thirstSlider;
    /// <summary> Az életkort mutató csúszka. </summary>
    public Slider ageSlider;

    int tempTimer = -1;
    float realTimer = 0;
    int elapsedSeconds = 0;
    int tempWeeks = 1;

    private float nextUpdateTime = 0f;

    [SerializeField] int waitToWanderFood;
    [SerializeField] int waitToWanderThirst;

    [SerializeField] List<GameObject> otherAnimalsOfMySpecies;

    Vector3 followedPos;
    float followedGoalTime;
    public Coroutine cor;

    public Vector3 globalFPos;
    public float globalGoalTime;

    private GameObject victimPlantEater;

    //Csak a krokodil változói
    [SerializeField]private GameObject myRiver;
    [SerializeField]private BoxCollider[] myRiverColliders;
    [SerializeField] private GameObject[] mainRivers;


    /// <summary>
    /// Inicializálás, komponensek és változók beállítása.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Kezdeti értékek beállítása és referencia keresések.
    /// </summary>
    private void Start()
    {
        state = AnimalStates.IDLE;
        this.hunger = 30;
        this.thirst = 100;
        this.maxThirst = this.thirst;
        this.isAlive = true;
        this.moveSpeed = 10f;
        this.maxHunger = hunger + age;


        waitToWanderFood = Random.Range(maxHunger - 40, maxHunger - 20); // Elõször 60-tól  indul, mert ahogy öregszik, ez a szám változik és 80-ig megy.
        waitToWanderThirst = Random.Range(60, 80);
        if (waitToWanderThirst % 2 != 0) ++waitToWanderThirst;
        terrain = GameObject.FindAnyObjectByType<Terrain>();
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        GameObject[] tempC = GameObject.FindGameObjectsWithTag("TerrainCollider");
        birthCount = Random.Range(0, 5);
        birthRandom = Random.Range(80, 85);

        tempWeeks = playerController.GetComponent<PlayerController>().weeks;

        terrainColliders = new List<GameObject>();

        for (int i = 0; i < tempC.Length; ++i)
            terrainColliders.Add(tempC[i]);

        pathFindingTrigger = GetComponentInChildren<PathFinderTriggerDetectionPredator>().gameObject;
        switch (type)
        {
            case PredatorAnimalTypes.LION:
                myPrice = 1650;
                myBaby = GameObject.Find("LionSuc");
                break;
            case PredatorAnimalTypes.LEOPARD:
                myPrice = 2050;
                myBaby = GameObject.Find("LeopardSuc");
                break;
            case PredatorAnimalTypes.CROCO:
                myPrice = 1750;
                myBaby = GameObject.Find("CrocoSuc");
                break;
        }

        mainRivers = GameObject.FindGameObjectsWithTag("River");
        if (type == PredatorAnimalTypes.CROCO)
        {
            for(int i = 0; i < mainRivers.Length; ++i)
            {
                int childCount = mainRivers[i].transform.childCount;
                for(int j = 0; j < childCount; ++j)
                {
                    if(RiverColliderCheck(mainRivers[i].transform.GetChild(j).gameObject.GetComponent<BoxCollider>()))
                    {
                        myRiver = mainRivers[i];
                        goto SetRiverCollider;
                    }
                }
            }
        SetRiverCollider:
            Debug.Log("Folyó Collider-ek beállítása");
            myRiverColliders = myRiver.GetComponentsInChildren<BoxCollider>();
        }
    }



    /// <summary>
    /// Állapotfrissítés frame-enként, másodperc alapú logikával.
    /// </summary>
    private void Update() // Renderhez kötött frissítés
    {
        if (!playerController.isPaused)
        {
            realTimer += Time.deltaTime;
            elapsedSeconds = Mathf.FloorToInt(realTimer % 60);

            if (Time.time >= nextUpdateTime)// Másodpercenként egyszer fogja meghívni az itt lévõ cuccokat <- ÚJ IDÕZÍTÕ
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
                        if (this.type == PredatorAnimalTypes.CROCO) globalFPos = GenerateRiverPosition();
                        else globalFPos = GenerateValidWanderPosition(transform.position, 15);
                            StartCoroutine(AnimalMoving(globalGoalTime, globalFPos));
                    }
                }
                tempTimer = elapsedSeconds;
            }
        }
    }

    /// <summary>
    /// Az állat eladása, pénz jóváírása a játékosnak.
    /// </summary>
    public void SellMyself()
    {
        playerController.playerCash += myPrice;
        Destroy(gameObject);
    }

    #region CSAK CROCO

    /// <summary>
    /// Krokodilhoz tartozó: folyó collider ellenõrzés.
    /// </summary>
    private bool RiverColliderCheck(Collider col)
    {
        if (col.bounds.Contains(this.transform.position)) return true;
        else return false;
    }

    /// <summary>
    /// Krokodilhoz tartozó: random pozíció generálása folyó mentén.
    /// </summary>
    private Vector3 GenerateRiverPosition()
    {
        BoxCollider chosedRiverPos = myRiverColliders[Random.Range(0, myRiverColliders.Length)];
        Vector3 chosedRiverLocalPos = new Vector3(Random.Range(-chosedRiverPos.size.x / 2f, chosedRiverPos.size.x / 2f), Random.Range(-chosedRiverPos.size.y / 2f, chosedRiverPos.size.y / 2f), Random.Range(-chosedRiverPos.size.z / 2f, chosedRiverPos.size.z / 2f));
        Vector3 realChosedPos = chosedRiverPos.transform.TransformPoint(chosedRiverPos.center + chosedRiverLocalPos);
        return new Vector3(realChosedPos.x, transform.position.y, realChosedPos.z);
    }

    /// <summary>
    /// Krokodilhoz tartozó: véletlenszerû evés.
    /// </summary>    private void CrocoRandomEat()
    private void CrocoRandomEat()
    {
        if (Random.Range(1, 200) == 80) hunger = maxHunger;
    }
    #endregion

    #region CSAK LEOPÁRD ÉS OROSZLÁN
    /// <summary>
    /// Útvonal generálása hegyvidéki területeken.
    /// </summary>
    private Vector3 GenerateValidWanderPosition(Vector3 startPos, int diff)
    {
        Vector3 randomPos = Vector3.zero;

        bool isGood = false;

        do
        {
            float x = Random.Range(startPos.x - diff, (startPos.x + diff));
            float y = 6f;
            float z = Random.Range(startPos.z - diff, (startPos.z + diff));

            for (int i = 0; i < terrainColliders.Count; ++i)
            {
                if (terrainColliders[i].GetComponent<Collider>().bounds.Contains(new Vector3(x, y, z)))
                {
                    isGood = true;
                    randomPos = new Vector3(x, y, z);
                    break;
                }
            }
        } while (!isGood);
        return randomPos;
    }
    #endregion

    /// <summary>
    /// Vándorlási mozgás logikája.
    /// </summary>
    public IEnumerator AnimalMoving(float goalTime, Vector3 followPos)
    {
        this.state = AnimalStates.WANDER;
        Vector3 startPos = transform.position;
        Vector3 endPos = followPos;

        if (endPos.x > transform.position.x && endPos.z < transform.position.z) spriteRenderer.flipX = true;
        else if (endPos.x < transform.position.x && endPos.z > transform.position.z) spriteRenderer.flipX = false;
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
    /// Életciklus frissítése, mint öregedés, szomjúság, éhség.
    /// </summary>
    private void LifeFunctionUpdate()
    {

        if (isAlive)
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
                if(type != PredatorAnimalTypes.CROCO) thirst -= 2; // Krokodil a folyó mellett él, így erre nincs szerintem szükség
                else CrocoRandomEat(); // Csak azért, mert ritkán haladnak el állatok a folyó körül.

                

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

            //Életerõsség ellenõrzése
            if (hunger <= 0 || thirst <= 0 || age >= 30) isAlive = false;

            if (state == AnimalStates.CONSUME)
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
    /// UI frissítése az állapotoknak megfelelõen.
    /// </summary>
    void UpdateStatUI()
    {
        hungerSlider.value = hunger;
        thirstSlider.value = thirst;
        ageSlider.value = age;
    }

    /// <summary>
    /// Útvonal keresése ételhez vagy vízhez.
    /// </summary>
    private void FindPath()
    {
        if (isHungry && state != AnimalStates.GO_TO_FOOD)
        {
            pathFindingTrigger.SetActive(true);

            if (seenFoods.Count > 0)
            {
                GameObject tempFoodPos = seenFoods[0];
                for (int i = 0; i < seenFoods.Count; i++)
                {
                    if(seenFoods[i] != null)
                    {
                        if (Vector3.Distance(transform.position, tempFoodPos.transform.position) > Vector3.Distance(transform.position, seenFoods[i].transform.position))
                        {
                            tempFoodPos = seenFoods[i];
                        }
                    }
                }

                if (state != AnimalStates.GO_TO_FOOD)
                {
                    victimPlantEater = tempFoodPos;
                    cor = StartCoroutine(GoToFood(tempFoodPos));
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
    /// Ételfelé vagy vízfelé mozgás.
    /// </summary>
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
            if (!exploredFoods.Contains(destination)) exploredFoods.Add(destination);
            chosedFoodPlace = destination;
        }
        else if (isThirsty)
        {
            if (!exploredLakes.Contains(destination)) exploredLakes.Add(destination);
            chosedLakePlace = destination;
        }
        state = AnimalStates.CONSUME;
        waitToWanderFood = Random.Range(maxHunger - 40, maxHunger - 20);
        waitToWanderThirst = Random.Range(60, 80);
        if (waitToWanderThirst % 2 != 0) ++waitToWanderThirst;

    }

    /// <summary>
    /// Evés vagy ivás logikája.
    /// </summary>
    private void Consume()
    {
        if (isHungry)
        {
            if (hunger < maxHunger) // Ha majd meggondoljuk, akkor lehet majd ezzel is, de úgy bonyolultabb -> chosedFoodPlace.GetComponent<Plant>().count > 0
            {
                hunger += 10;
                if (hunger > maxHunger)
                {
                    hunger = maxHunger;
                    Destroy(victimPlantEater.gameObject);
                }
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
                if (thirst > maxThirst)
                {
                    thirst = maxThirst;
                }
            }
            else
            {
                isThirsty = false;
                if (!isHungry) state = AnimalStates.IDLE;
            }
        }
    }

    public void OnPathFindTriggerEnter(Collider collider)
    {
        if (collider.tag == "Tiere" && !seenFoods.Contains(collider.gameObject)) seenFoods.Add(collider.gameObject);
        if (collider.tag == "Lake" && !seenLakes.Contains(collider.gameObject)) seenLakes.Add(collider.gameObject);
        if (collider.tag == "Hill")
        {
            pathFindingTrigger = pathFindTriggers[1];
            isHillPath = true;
        }
    }
    
    public void OnGroupFindTriggerStay(Collider collider)
    {
        if (!otherAnimalsOfMySpecies.Contains(collider.gameObject) && collider.GetComponent<Predator>().type == this.type) otherAnimalsOfMySpecies.Add(collider.gameObject);
        if (collider.tag == "River")
        {
            isRiverPath = true;
        }
    }

    public void OnGroupFindTriggerExit(Collider collider)
    {
        if (elapsedSeconds > 0) if (otherAnimalsOfMySpecies.Contains(collider.gameObject) && collider.GetComponent<Predator>().type == this.type) otherAnimalsOfMySpecies.Remove(collider.gameObject);
        if (collider.tag == "Hill")
        {
            pathFindingTrigger = pathFindTriggers[0];
            isHillPath = false;
        }
        if (collider.tag == "River") isRiverPath = false;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }

    public void Breed()
    {
        if (otherAnimalsOfMySpecies.Count != 0 && hunger == birthRandom && birthCount > 0 && state == AnimalStates.IDLE && age > 15)
        {
            GameObject chosedLove = otherAnimalsOfMySpecies[Random.Range(0, otherAnimalsOfMySpecies.Count - 1)];
            if (chosedLove.GetComponent<PlantEaterAnimal>().birthCount > 0 && chosedLove.GetComponent<PlantEaterAnimal>().age > 15)
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

    public AnimalData GetAnimalData() // Módosítás - Persistence
    {
        return new AnimalData
        {
            type = this.type.ToString(),
            position = new Vector3Data(transform.position)
        };
    }
}
