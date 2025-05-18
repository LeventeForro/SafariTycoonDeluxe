using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A ragadoz� �llat t�pus�t jel�li (oroszl�n, leop�rd, krokodil).
/// </summary>
public enum PredatorAnimalTypes
{
    LION,
    LEOPARD,
    CROCO
}


/// <summary>
/// Pred�tor �llat oszt�ly, amely kezeli a mozg�st, �hs�get, szomj�s�got, szaporod�st �s interakci�kat.
/// </summary>
public class Predator : MonoBehaviour
{

    [Header("Alapvet� adatok")]
    /// <summary> Az �llat �letkora. </summary>
    public int age;
    /// <summary> A ragadoz� t�pusa. </summary>
    public PredatorAnimalTypes type;
    /// <summary> �hs�g szint. </summary>
    public int hunger;
    /// <summary> Szomj�s�g szint. </summary>
    public int thirst;
    /// <summary> Az �llat �letben van-e. </summary>
    public bool isAlive;
    /// <summary> Maxim�lis �hs�g. </summary>
    public int maxHunger;
    /// <summary> Maxim�lis szomj�s�g. </summary>
    public int maxThirst;
    /// <summary> Az �llat aktu�lis �llapota. </summary>
    public AnimalStates state;
    /// <summary> Az �llat elad�si �ra. </summary>
    public int myPrice;

    [Header("Mozg�s")]
    /// <summary> Mozg�si sebess�g. </summary>
    public float moveSpeed;
    /// <summary> J�t�kos vez�rl� referencia. </summary>
    [SerializeField]PlayerController playerController;
    Rigidbody rb;
    Terrain terrain;
    Collider oceanCollider;
    List<GameObject> terrainColliders;
    private SpriteRenderer spriteRenderer;
    /// <summary> Foly� �tvonalon mozog-e. </summary>
    [SerializeField] bool isRiverPath;
    /// <summary> Hegyes �tvonalon mozog-e. </summary>
    [SerializeField] bool isHillPath;

    [Header("Kaja �s pia")]
    /// <summary> L�tott �telek list�ja. </summary>
    public List<GameObject> seenFoods;
    /// <summary> L�tott tavak list�ja. </summary>
    public List<GameObject> seenLakes;
    /// <summary> Felfedezett tavak list�ja. </summary>
    public List<GameObject> exploredLakes;
    /// <summary> Felfedezett �telek list�ja. </summary>
    public List<GameObject> exploredFoods;
    /// <summary> Kiv�lasztott �tel helysz�ne. </summary>
    public GameObject chosedFoodPlace;
    /// <summary> Kiv�lasztott t� helysz�ne. </summary>
    public GameObject chosedLakePlace;
    public List<GameObject> pathFindTriggers = new List<GameObject>();
    public GameObject pathFindingTrigger;
    /// <summary> �hes-e az �llat. </summary>
    public bool isHungry;
    /// <summary> Szomjas-e az �llat. </summary>
    public bool isThirsty;
    /// <summary> A hely, ahol az �llat �ppen fogyaszt. </summary>
    public Vector3 consumePlace;

    [Header("Szaporod�s")]
    /// <summary> Megsz�letett ut�dok sz�ma. </summary>
    public int birthCount;
    /// <summary> V�letlenszer� szaporod�si �rt�k. </summary>
    public int birthRandom;
    /// <summary> Az �llat ut�da (prefab). </summary>
    public GameObject myBaby;

    [Header("�llat UI")]
    /// <summary> A statisztikai canvas UI. </summary>
    public Canvas myStatCanvas;
    /// <summary> Az �hs�get mutat� cs�szka. </summary>
    public Slider hungerSlider;
    /// <summary> A szomj�s�got mutat� cs�szka. </summary>
    public Slider thirstSlider;
    /// <summary> Az �letkort mutat� cs�szka. </summary>
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

    //Csak a krokodil v�ltoz�i
    [SerializeField]private GameObject myRiver;
    [SerializeField]private BoxCollider[] myRiverColliders;
    [SerializeField] private GameObject[] mainRivers;


    /// <summary>
    /// Inicializ�l�s, komponensek �s v�ltoz�k be�ll�t�sa.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// Kezdeti �rt�kek be�ll�t�sa �s referencia keres�sek.
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


        waitToWanderFood = Random.Range(maxHunger - 40, maxHunger - 20); // El�sz�r 60-t�l  indul, mert ahogy �regszik, ez a sz�m v�ltozik �s 80-ig megy.
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
            Debug.Log("Foly� Collider-ek be�ll�t�sa");
            myRiverColliders = myRiver.GetComponentsInChildren<BoxCollider>();
        }
    }



    /// <summary>
    /// �llapotfriss�t�s frame-enk�nt, m�sodperc alap� logik�val.
    /// </summary>
    private void Update() // Renderhez k�t�tt friss�t�s
    {
        if (!playerController.isPaused)
        {
            realTimer += Time.deltaTime;
            elapsedSeconds = Mathf.FloorToInt(realTimer % 60);

            if (Time.time >= nextUpdateTime)// M�sodpercenk�nt egyszer fogja megh�vni az itt l�v� cuccokat <- �J ID�Z�T�
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
    /// Az �llat elad�sa, p�nz j�v��r�sa a j�t�kosnak.
    /// </summary>
    public void SellMyself()
    {
        playerController.playerCash += myPrice;
        Destroy(gameObject);
    }

    #region CSAK CROCO

    /// <summary>
    /// Krokodilhoz tartoz�: foly� collider ellen�rz�s.
    /// </summary>
    private bool RiverColliderCheck(Collider col)
    {
        if (col.bounds.Contains(this.transform.position)) return true;
        else return false;
    }

    /// <summary>
    /// Krokodilhoz tartoz�: random poz�ci� gener�l�sa foly� ment�n.
    /// </summary>
    private Vector3 GenerateRiverPosition()
    {
        BoxCollider chosedRiverPos = myRiverColliders[Random.Range(0, myRiverColliders.Length)];
        Vector3 chosedRiverLocalPos = new Vector3(Random.Range(-chosedRiverPos.size.x / 2f, chosedRiverPos.size.x / 2f), Random.Range(-chosedRiverPos.size.y / 2f, chosedRiverPos.size.y / 2f), Random.Range(-chosedRiverPos.size.z / 2f, chosedRiverPos.size.z / 2f));
        Vector3 realChosedPos = chosedRiverPos.transform.TransformPoint(chosedRiverPos.center + chosedRiverLocalPos);
        return new Vector3(realChosedPos.x, transform.position.y, realChosedPos.z);
    }

    /// <summary>
    /// Krokodilhoz tartoz�: v�letlenszer� ev�s.
    /// </summary>    private void CrocoRandomEat()
    private void CrocoRandomEat()
    {
        if (Random.Range(1, 200) == 80) hunger = maxHunger;
    }
    #endregion

    #region CSAK LEOP�RD �S OROSZL�N
    /// <summary>
    /// �tvonal gener�l�sa hegyvid�ki ter�leteken.
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
    /// V�ndorl�si mozg�s logik�ja.
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
        this.state = AnimalStates.IDLE; // Ha m�g nem �hes vagy szomis
    }

    /// <summary>
    /// �letciklus friss�t�se, mint �reged�s, szomj�s�g, �hs�g.
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
            // Adatok friss�t�se
            if (elapsedSeconds % 2 == 0 && state != AnimalStates.CONSUME)
            {
                --hunger;
                if(type != PredatorAnimalTypes.CROCO) thirst -= 2; // Krokodil a foly� mellett �l, �gy erre nincs szerintem sz�ks�g
                else CrocoRandomEat(); // Csak az�rt, mert ritk�n haladnak el �llatok a foly� k�r�l.

                

                Breed();


            }
            //�reged�s
            if (playerController != null)
            {
                if (tempWeeks != playerController.weeks) // El�g csak ennyi, mert az �llatok hetente �regednek
                {
                    ++age;
                    this.maxHunger = hunger + (age * 4);
                    hungerSlider.maxValue = this.maxHunger;
                    tempWeeks = playerController.weeks;
                }
            }

            //�leter�ss�g ellen�rz�se
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
    /// UI friss�t�se az �llapotoknak megfelel�en.
    /// </summary>
    void UpdateStatUI()
    {
        hungerSlider.value = hunger;
        thirstSlider.value = thirst;
        ageSlider.value = age;
    }

    /// <summary>
    /// �tvonal keres�se �telhez vagy v�zhez.
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
        
        
        //IV�S
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
            else // M�g semmit sem fedezett fel, ez�rt megn�zi, hogy mik vannak k�r�l�tte
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
    /// �telfel� vagy v�zfel� mozg�s.
    /// </summary>
    private IEnumerator GoToFood(GameObject destination)
    {
        state = AnimalStates.GO_TO_FOOD;
        consumePlace = destination.transform.position;
        pathFindingTrigger.SetActive(false);
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(Random.Range((int)destination.transform.position.x - 2, (int)destination.transform.position.x + 2), transform.position.y, Random.Range((int)destination.transform.position.z - 2, (int)destination.transform.position.z + 2));

        float goalTime = Vector3.Distance(transform.position, endPos) / 5; // Fizika 5-�s

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
    /// Ev�s vagy iv�s logik�ja.
    /// </summary>
    private void Consume()
    {
        if (isHungry)
        {
            if (hunger < maxHunger) // Ha majd meggondoljuk, akkor lehet majd ezzel is, de �gy bonyolultabb -> chosedFoodPlace.GetComponent<Plant>().count > 0
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
            if (thirst < maxThirst) // Ha majd meggondoljuk, akkor lehet majd ezzel is, de �gy bonyolultabb -> chosedFoodPlace.GetComponent<Plant>().count > 0
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

                Debug.Log("Megvolt a dolog �s most lett egy �j gyerek");
                --birthCount;
            }
        }
    }

    public AnimalData GetAnimalData() // M�dos�t�s - Persistence
    {
        return new AnimalData
        {
            type = this.type.ToString(),
            position = new Vector3Data(transform.position)
        };
    }
}
