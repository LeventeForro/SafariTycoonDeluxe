using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A rácsrendszer kezeléséért felelős osztály.
/// </summary>
public class GridController : MonoBehaviour
{

    [Header("Alapvető pályaelemek")]
    /// <summary>
    /// A kamera objektum.
    /// </summary>
    public Camera cam;

    /// <summary>
    /// A kiválasztott elem indexe.
    /// </summary>
    public int selectedItem;


    /// <summary>
    /// Az elérhető elemek listája.
    /// </summary>
    public List<GameObject> items;

    /// <summary>
    /// A kiválasztó objektum.
    /// </summary>
    public GameObject selectObj;

    /// <summary>
    /// A terep objektum.
    /// </summary>
    public Terrain terrain;
    
    
    [Header("Entitások")]

    /// <summary>
    /// A játékos vezérlője.
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// A növényevők száma.
    /// </summary>
    public static int plantEaterCounter = 0;

    /// <summary>
    /// A ragadozók száma.
    /// </summary>
    public static int predatorCounter = 0;

    /// <summary>
    /// A növények listája.
    /// </summary>
    public List<GameObject> plants;

    /// <summary>
    /// A tavak listája.
    /// </summary>
    public List<GameObject> lakes;

    /// <summary>
    /// A Jeepek listája.
    /// </summary>
    public List<GameObject> jeeps;

    /// <summary>
    /// Az utak listája.
    /// </summary>
    public List<GameObject> roads;

    /// <summary>
    /// A folyók listája.
    /// </summary>
    public List<GameObject> rivers;

    /// <summary>
    /// A dombok listája.
    /// </summary>
    public List<GameObject> hills;

    /// <summary>
    /// A felhasználói felület vezérlője.
    /// </summary>
    public UIController uiController;

    /// <summary>
    /// A játékos által elhelyezett objektumok szülője.
    /// </summary>
    public GameObject PlayerPlacedObj;

    /// <summary>
    /// Az elemek árai.
    /// </summary>
    [SerializeField] private readonly int[] prices = new int[] { 1000, 2900, 1500, 2000, 3500, 3300, 4100, 12000, 14500, 11300, 10900, 18400, 5600, 1 }; // zebra, strucc, zsiráf, gazella, kroki, oroszlán, leopárd, akácfa, majomkenyérfa, bokor, fű, tó


    /// <summary>
    /// Az állatok nevei.
    /// </summary>
    [SerializeField] private readonly string[] animalTexts = new string[] { "Zebra", "Strucc", "Zsiráf", "Gazella", "Krokodil", "Oroszlán", "Leopárd", "Akácia fa", "Majomkenyérfa", "Bokor", "Fűcsomó", "Tó darab", "Dzsipp", "Út" };


    [Header("Grid beállítások")]


    /// <summary>
    /// Az állatok rétege.
    /// </summary>
    public LayerMask animalLayer;

    /// <summary>
    /// Meghatározza, hogy a rács engedélyezve van-e.
    /// </summary>
    public bool isGridEnabled = false;

    /// <summary>
    /// Meghatározza, hogy a rács látható-e.
    /// </summary>
    public bool gridOn = false;

    /// <summary>
    /// A rács vetítési objektuma.
    /// </summary>
    public GameObject gridProjection;


    /// <summary>
    /// A rács mérete.
    /// </summary>
    public float gridSize = 1f;

    /// <summary>
    /// A renderelt textúra szélessége.
    /// </summary>
    public int renderTextureWidth; //= 480;

    /// <summary>
    /// A renderelt textúra magassága.
    /// </summary>
    public int renderTextureHeight; //= 270;

    /// <summary>
    /// A kiválasztó színei.
    /// </summary>
    public Color[] selectColors;

    /// <summary>
    /// A renderelt kép objektuma.
    /// </summary>
    public RawImage renderedImage;

    /// <summary>
    /// Az animált szöveg objektuma vásárláskor.
    /// </summary>
    public GameObject buyAnimatedText;

    GameObject mouseSelectedAnimalToolTip;    
    
    Canvas gameUI;
    
    List<GameObject> placedObjects;

    /// <summary>
    /// Meghatározza, hogy a boltban van-e a játékos.
    /// </summary>
    public bool inShop = false;

    /// <summary>
    /// Meghatározza, hogy csalás aktív-e.
    /// </summary>
    public bool isCheating = false;

    /// <summary>
    /// A csalás panel objektuma.
    /// </summary>
    public GameObject cheatPanel;

    /// <summary>
    /// A folyókhoz tartozó kolliderek listája.
    /// </summary>
    public List<BoxCollider> riverColliders;

    /// <summary>
    /// Az elhelyezett utak pozícióinak halmaza.
    /// </summary>
    private HashSet<Vector3Int> placedRoadPositions = new HashSet<Vector3Int>();


    /// <summary>
    /// Inicializálja a rácsrendszert a játék indításakor.
    /// </summary>
    void Start()
    {
        gridProjection.SetActive(false);
        selectObj.SetActive(false);
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        player = FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
        gameUI = GameObject.Find("GameCanvas").GetComponent<Canvas>();
        placedObjects = new List<GameObject>();
        buyAnimatedText.GetComponent<TMP_Text>().text = string.Empty;
    }

    /// <summary>
    /// Frissíti a rácsrendszert minden képkockában.
    /// </summary>
    void Update()
    {
        if (isGridEnabled)
        {
            SelecterMove();

            if (!gridOn)
            {
                ShowGrid();
                gridOn = true;
            }
        }
        else if (!isGridEnabled)
        {
            if (gridOn)
            {
                HideGrid();
                gridOn = false;
            }
        }

        HandleKeys();
        ShowAnimalStats();

    }

    /// <summary>
    /// Kezeli a billentyűzet bemeneteket.
    /// </summary>
    void HandleKeys()
    {
        if (Input.GetKeyDown(KeyCode.B)) isGridEnabled = !isGridEnabled;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    isCheating = !isCheating;
                    cheatPanel.SetActive(!cheatPanel.gameObject.activeSelf);
                }
            }
        }
    }

    /// <summary>
    /// Csaláskódok kezelése.
    /// </summary>
    /// <param name="cheatCode">A megadott csaláskód.</param>
    public void Cheat(string cheatCode)
    {
        switch (cheatCode.ToLower())
        {
            case "suska":
                player.playerCash += 50000;
                break;

        }
    }

    /// <summary>
    /// Megjeleníti az állatok statisztikáit.
    /// </summary>
    void ShowAnimalStats()
    {
        Rect renderedGameArea = PlayerController.GetRenderRectFromRectTransform(renderedImage.rectTransform, gameUI.renderMode == RenderMode.ScreenSpaceOverlay ? null : gameUI.worldCamera);
        Vector3 mousePos = Input.mousePosition;

        float mouseX = ((mousePos.x / renderedGameArea.width) * renderTextureWidth);
        float mouseY = (mousePos.y / renderedGameArea.height) * renderTextureHeight;

        Vector3 realMousePos = new Vector3(mouseX, mouseY, mousePos.z);

        Ray ray = cam.ScreenPointToRay(realMousePos);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, animalLayer) && player.cameraZoomState > 0)
        {

            if(hit.collider.TryGetComponent<PlantEaterAnimal>(out var targetAnimal))
            {
                if (targetAnimal != null)
                {
                    if (mouseSelectedAnimalToolTip == null)
                    {
                        mouseSelectedAnimalToolTip = targetAnimal.myStatCanvas.gameObject;
                        mouseSelectedAnimalToolTip.SetActive(true);
                        
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        targetAnimal.SellMyself();
                        buyAnimatedText.GetComponent<TMP_Text>().text = $"Eladva +{targetAnimal.myPrice} Ft";
                        buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                    }
                }

            }
            else if(hit.collider.TryGetComponent<Predator>(out var targetAnimal2))
            {
                if (targetAnimal2 != null)
                {
                    if (mouseSelectedAnimalToolTip == null)
                    {
                        mouseSelectedAnimalToolTip = targetAnimal2.myStatCanvas.gameObject;
                        mouseSelectedAnimalToolTip.SetActive(true);
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        targetAnimal2.SellMyself();
                        buyAnimatedText.GetComponent<TMP_Text>().text = $"Eladva +{targetAnimal2.myPrice} Ft";
                        buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                    }
                }
            }


        }
        else
        {

            if (mouseSelectedAnimalToolTip != null)
            {
                mouseSelectedAnimalToolTip.SetActive(false);
                mouseSelectedAnimalToolTip = null;
            }

        }
    }

    /// <summary>
    /// Kezeli a kiválasztó mozgását.
    /// </summary>
    void SelecterMove()
    {
        Rect renderedGameArea = PlayerController.GetRenderRectFromRectTransform(renderedImage.rectTransform, gameUI.renderMode == RenderMode.ScreenSpaceOverlay ? null : gameUI.worldCamera);
        Vector3 mousePos = Input.mousePosition;

        float mouseX = ((mousePos.x / renderedGameArea.width) * renderTextureWidth);
        float mouseY = (mousePos.y / renderedGameArea.height) * renderTextureHeight;

        Vector3 realMousePos = new Vector3(mouseX, mouseY, mousePos.z);

        Ray ray = cam.ScreenPointToRay(realMousePos);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            GameObject targetHit = rayHit.transform.gameObject;
            Vector3 hitPos = rayHit.point;
            if (targetHit != null)
            {
                hitPos.x = Mathf.Ceil(hitPos.x / gridSize) * gridSize;
                hitPos.z = Mathf.Ceil(hitPos.z / gridSize) * gridSize;

                float terrainHeight = terrain.SampleHeight(hitPos);

                hitPos.y = terrainHeight + (transform.localScale.y / 2) + 0.5f;

                selectObj.transform.position = hitPos;

                if (targetHit.tag != "Wasser" || items[selectedItem].gameObject.name == "Road")
                {
                    ColoringCube(terrainHeight);
                }

                if (targetHit.tag == "Wasser" && items[selectedItem].gameObject.name != "Road")
                {
                    return;
                }

                if (items[selectedItem].gameObject.name == "Road" && Input.GetMouseButton(0) && player.isMouseInGameArea && !inShop)
                {
                    // Az utak esetében nem ellenőrizzük a magasságot
                    if (!IsValidRoadPosition(hitPos))
                    {
                        int tempPlayerCash = player.playerCash;
                        PlaceItem(hitPos);
                        if(player.playerCash < tempPlayerCash)
                        {
                            uiController.auSource.clip = uiController.soundEffects[1];
                            uiController.auSource.Play();
                        }
                    }
                    else
                    {
                        buyAnimatedText.GetComponent<TMP_Text>().text = "Már van itt út!";
                        buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                    }
                }
                else if (Input.GetMouseButtonDown(0) && TerrainHeightCheck(terrainHeight) && player.isMouseInGameArea && !inShop)
                {
                    int tempPlayerCash = player.playerCash;
                    PlaceItem(hitPos);
                    if (player.playerCash < tempPlayerCash)
                    {
                        uiController.auSource.clip = uiController.soundEffects[1];
                        uiController.auSource.Play();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Ellenőrzi, hogy a terep magassága osztható-e 3-mal.
    /// </summary>
    /// <param name="terrainHeight">A vizsgált terepmagasság.</param>
    /// <returns>Igaz, ha a magasság osztható 3-mal, egyébként hamis.</returns>
    bool TerrainHeightCheck(float terrainHeight)
    {
        return (Mathf.Round(terrainHeight) % 3 == 0);
    }


    /// <summary>
    /// Beállítja a kiválasztott objektum színét a terepmagasság alapján.
    /// </summary>
    /// <param name="terrainHeight">A terep magassága, amely meghatározza a színt.</param>
    void ColoringCube(float terrainHeight)
    {
        if (TerrainHeightCheck(terrainHeight))
        {
            selectObj.GetComponent<MeshRenderer>().material.color = selectColors[1];
        }
        else selectObj.GetComponent<MeshRenderer>().material.color = selectColors[0];
    }

    /// <summary>
    /// Egy objektum vagy állat lerakását végzi el a megadott pozícióban, ellenőrzi a feltételeket
    /// (pl. pénz, elérhetőség, érvényes pozíció), valamint megjeleníti a felhasználónak szánt visszajelzéseket.
    /// </summary>
    /// <param name="hitPos">A pozíció, ahova az objektumot le szeretnénk helyezni.</param>
    public void PlaceItem(Vector3 hitPos)
    {
        Debug.Log("Lerakva");

        if (items[selectedItem].gameObject.name == "Jeep")
        {
            if (!IsValidRoadPosition(hitPos))
            {
                buyAnimatedText.GetComponent<TMP_Text>().text = $"Érvénytelen pozíció!";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                return;
            }
            else
            {
                Base.isJeepAvailable = true;
            }
        }

        Vector3Int gridPos = new Vector3Int(Mathf.RoundToInt(hitPos.x), 0, Mathf.RoundToInt(hitPos.z));

        if (items[selectedItem].name == "Road")
        {
            if (placedRoadPositions.Contains(gridPos))
            {
                buyAnimatedText.GetComponent<TMP_Text>().text = $"Már van itt út!";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                return;
            }
            else
            {
                placedRoadPositions.Add(gridPos);
            }

            hitPos.y = 2f;
        }


        {
            float terrainHeight = terrain.SampleHeight(hitPos);
        }
        if (items[selectedItem].name == "Road" && IsValidRoadPosition(hitPos))
        {
            buyAnimatedText.GetComponent<TMP_Text>().text = $"Már van itt út!";
            buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
            return;
        }
        switch (items[selectedItem].gameObject.name)
        {
            case "AkaciaFa":
                hitPos.y = hitPos.y + 3.8f;
                break;
            case "Bush":
                hitPos.y = hitPos.y + 0.3f;
                break;
            case "Grass":
                hitPos.y = hitPos.y + 0.9f;
                break;
            case "Majomkenyerfa":
                hitPos.y = hitPos.y + 3.6f;
                break;
            case "LakePiece":
                hitPos.y = hitPos.y - 0.9f;
                break;
        }
        if (items[selectedItem].GetComponent<PlantEaterAnimal>() != null)
        {
            if (player.playerCash - prices[selectedItem] >= 0)
            {
                GameObject placedAnimal = Instantiate(items[selectedItem], new Vector3(hitPos.x, hitPos.y + 1, hitPos.z), Quaternion.identity, GameObject.FindGameObjectWithTag(items[selectedItem].GetComponent<PlantEaterAnimal>().type.ToString()).transform);
                placedObjects.Add(placedAnimal);
                placedAnimal.name = items[selectedItem].gameObject.GetComponent<PlantEaterAnimal>().type.ToString();
                placedAnimal.GetComponent<PlantEaterAnimal>().age = 15;
                buyAnimatedText.GetComponent<TMP_Text>().text = $"{animalTexts[selectedItem]} lerakva.  -{prices[selectedItem]} Ft";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                player.playerCash -= prices[selectedItem];
                plantEaterCounter++;
            }
            else
            {
                buyAnimatedText.GetComponent<TMP_Text>().text = $"Nincs elég pénzed!";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
            }
        }
        else if (items[selectedItem].GetComponent<Predator>() != null)
        {
            if (items[selectedItem].GetComponent<Predator>().type == PredatorAnimalTypes.CROCO)
            {
                if (player.playerCash - prices[selectedItem] >= 0)
                {
                    for (int i = 0; i < riverColliders.Count; ++i)
                    {
                        if (riverColliders[i].bounds.Contains(selectObj.transform.position))
                        {
                            GameObject placedAnimal = Instantiate(items[selectedItem], new Vector3(hitPos.x, hitPos.y + 1, hitPos.z), Quaternion.identity, GameObject.FindGameObjectWithTag(items[selectedItem].GetComponent<Predator>().type.ToString()).transform);
                            placedObjects.Add(placedAnimal);
                            placedAnimal.name = items[selectedItem].gameObject.GetComponent<Predator>().type.ToString();
                            placedAnimal.GetComponent<Predator>().age = 15;
                            buyAnimatedText.GetComponent<TMP_Text>().text = $"{animalTexts[selectedItem]} lerakva.  -{prices[selectedItem]} Ft";
                            buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                            player.playerCash -= prices[selectedItem];
                            predatorCounter++;
                            break;
                        }
                        else buyAnimatedText.GetComponent<TMP_Text>().text = $"{animalTexts[selectedItem]}t csak folyó mellé tudod lehelyezni!";
                        buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                    }
                }
                else
                {
                    buyAnimatedText.GetComponent<TMP_Text>().text = $"Nincs elég pénzed!";
                    buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                }
            }
            else
            {
                if (player.playerCash - prices[selectedItem] >= 0)
                {
                    GameObject placedAnimal = Instantiate(items[selectedItem], new Vector3(hitPos.x, hitPos.y + 1, hitPos.z), Quaternion.identity, GameObject.FindGameObjectWithTag(items[selectedItem].GetComponent<Predator>().type.ToString()).transform);
                    placedObjects.Add(placedAnimal);
                    placedAnimal.name = items[selectedItem].gameObject.GetComponent<Predator>().type.ToString();
                    placedAnimal.GetComponent<Predator>().age = 15;
                    buyAnimatedText.GetComponent<TMP_Text>().text = $"{animalTexts[selectedItem]} lerakva.  -{prices[selectedItem]} Ft";
                    buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                    player.playerCash -= prices[selectedItem];
                    predatorCounter++;
                }
                else
                {
                    buyAnimatedText.GetComponent<TMP_Text>().text = $"Nincs elég pénzed!";
                    buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                }
            }

        }
        else if (items[selectedItem].gameObject.name != "Jeep" && items[selectedItem].gameObject.name != "Road")
        {
            if (player.playerCash - prices[selectedItem] >= 0)
            {
                Debug.Log("Rakódik a dolog");
                GameObject placedItem = Instantiate(items[selectedItem], new Vector3(hitPos.x, hitPos.y, hitPos.z), Quaternion.identity, PlayerPlacedObj.transform);
                placedObjects.Add(placedItem);
                buyAnimatedText.GetComponent<TMP_Text>().text = $"{animalTexts[selectedItem]} lerakva.  -{prices[selectedItem]} Ft";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                placedObjects.Add(placedItem);
                player.playerCash -= prices[selectedItem];
            }
            else
            {
                buyAnimatedText.GetComponent<TMP_Text>().text = $"Nincs elég pénzed!";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
            }
        }
        else
        {
            if (player.playerCash - prices[selectedItem] >= 0)
            {
                GameObject placedItem = Instantiate(items[selectedItem], new Vector3(hitPos.x, hitPos.y + 1, hitPos.z), Quaternion.identity, PlayerPlacedObj.transform);
                placedObjects.Add(placedItem);
                buyAnimatedText.GetComponent<TMP_Text>().text = $"{animalTexts[selectedItem]} lerakva.  -{prices[selectedItem]} Ft";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
                player.playerCash -= prices[selectedItem];
            }
            else
            {
                buyAnimatedText.GetComponent<TMP_Text>().text = $"Nincs elég pénzed!";
                buyAnimatedText.GetComponent<Animator>().Play("buyTextAppearAnim", -1, 0f);
            }
        }
    }

    /// <summary>
    /// Újrapozicionálja az összes lehelyezett objektumot egy fix Y-magasságra az objektum típusától függően,
    /// majd törli a `placedObjects` listát.
    /// </summary>
    public void OrganizePlacedItems()
    {
        for (int i = 0; i < placedObjects.Count; i++)
        {
            Vector3 newPos = placedObjects[i].transform.position;
            if (placedObjects[i].gameObject.name.Contains("AkaciaFa")) newPos.y = 10.35f;
            else if (placedObjects[i].gameObject.name.Contains("Bush")) newPos.y = 7f;
            else if (placedObjects[i].gameObject.name.Contains("Grass")) newPos.y = 7.9f;
            else if (placedObjects[i].gameObject.name.Contains("Majomkenyerfa")) newPos.y = 10.7f;
            else if (placedObjects[i].gameObject.name.Contains("LakePiece")) newPos.y = 6.1f;
            placedObjects[i].gameObject.transform.position = newPos;
        }
        placedObjects.Clear();
    }

    /// <summary>
    /// Ellenőrzi, hogy az adott pozíció érvényes-e út lerakásához.
    /// Engedélyezett, ha a pozíció nagyon közel van egy már meglévő úthoz vagy a startmezőhöz.
    /// </summary>
    /// <param name="position">A pozíció, amelyet ellenőrizni kell.</param>
    /// <returns>Igaz, ha érvényes útpozíció, egyébként hamis.</returns>
    bool IsValidRoadPosition(Vector3 position)
    {
        foreach (var road in RoadTile.allRoadTiles)
        {
            if (road.name == "Start" && Vector3.Distance(road.transform.position, position) < 1.5f)
            {
                return true;
            }
            if (Vector3.Distance(road.transform.position, position) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Aktiválja a rács módot és kiválasztja a boltban kijelölt elemet.
    /// </summary>
    /// <param name="itemID">A boltban kiválasztott elem indexe.</param>
    public void WorkingOnTheGridWithShop(int itemID)
    {
        selectedItem = itemID;
        isGridEnabled = true;
    }


    /// <summary>
    /// Megjeleníti a rács vizualizációját és a kijelölt objektumot.
    /// </summary>
    void ShowGrid()
    {
        gridProjection.SetActive(true);
        selectObj.gameObject.SetActive(true);
    }


    /// <summary>
    /// Elrejti a rács vizualizációját és a kijelölt objektumot.
    /// </summary>
    void HideGrid()
    {
        gridProjection.SetActive(false);
        selectObj.gameObject.SetActive(false);
    }

}