using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// A játékos vezérléséért felelős osztály.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Player Data")]
    /// <summary>
    /// A játékos pénzmennyisége.
    /// </summary>
    public int playerCash;

    /// <summary>
    /// Az eltelt hetek száma.
    /// </summary>
    public int weeks;

    /// <summary>
    /// Az eltelt napok száma.
    /// </summary>
    public int days;

    /// <summary>
    /// Az idő sebessége.
    /// </summary>
    public static int timeSpeed = 1;

    /// <summary>
    /// Az eltelt órák száma.
    /// </summary>
    public int hours;

    /// <summary>
    /// Meghatározza, hogy a játékos nyert-e.
    /// </summary>
    public bool hasWon = false;

    /// <summary>
    /// Meghatározza, hogy a játékos vesztett-e.
    /// </summary>
    public bool hasLose = false;

    /// <summary>
    /// Az eltelt hetek száma a feltételek teljesítéséhez.
    /// </summary>
    public int passedWeeks = 0;

    /// <summary>
    /// Meghatározza, hogy eltelt-e egy hét.
    /// </summary>
    public bool oneWeekPassed = false;

    /// <summary>
    /// Az utoljára ellenőrzött hét száma.
    /// </summary>
    public int lastCheckedWeek = -1;

    /// <summary>
    /// A turisták listája.
    /// </summary>
    public List<GameObject> tourists;

    /// <summary>
    /// Meghatározza, hogy a játék szünetel-e.
    /// </summary>
    public bool isPaused;

    /// <summary>
    /// A játék nehézségi szintje.
    /// </summary>
    public int gameDifficulty;

    /// <summary>
    /// Az eltelt idő.
    /// </summary>
    public float ellapsedTime = 0;


    [Header("Mozgás")]
    /// <summary>
    /// A kamera mozgási sebessége.
    /// </summary>
    public float camMoveSpeed;

    /// <summary>
    /// A kamera sprint sebessége.
    /// </summary>
    public float camSprintSpeed;

    /// <summary>
    /// Meghatározza, hogy a kamera mozgásának típusa aktív-e.
    /// </summary>
    public bool camMoveType = false;

    /// <summary>
    /// Az egér távolsága a képernyő szélétől, amely aktiválja a kamera mozgását.
    /// </summary>
    public float mouseDistance = Screen.width * 0.2f;

    /// <summary>
    /// Meghatározza, hogy a kamera éppen mozog-e.
    /// </summary>
    public bool isCameraMoving = false;

    /// <summary>
    /// Meghatározza, hogy az egér a játékterületen belül van-e.
    /// </summary>
    public bool isMouseInGameArea = false;

    /// <summary>
    /// A kamera maximális pozíciója.
    /// </summary>
    [SerializeField] private Vector3 camMaxPos;

    /// <summary>
    /// A kamera minimális pozíciója.
    /// </summary>
    [SerializeField] private Vector3 camMinPos;

    /// <summary>
    /// A kamera zoom állapota.
    /// </summary>
    public int cameraZoomState = 0;

    /// <summary>
    /// A kamera minimális zoom értéke.
    /// </summary>
    public int minZoom;

    /// <summary>
    /// A kamera maximális zoom értéke.
    /// </summary>
    public int maxZoom = 0;

    /// <summary>
    /// A kamera zoom állapotát megjelenítő szöveg.
    /// </summary>
    public TMP_Text camZoomStateText;

    /// <summary>
    /// A játékterületet megjelenítő nyers kép.
    /// </summary>
    public RawImage rawImage;

    /// <summary>
    /// Meghatározza, hogy a játékos a navigálható minimapben van-e.
    /// </summary>
    public bool inNavigateMinimap = false;

    [Header("Kurzor")]
    /// <summary>
    /// Az alapértelmezett kurzor textúrája.
    /// </summary>
    [SerializeField] private Texture2D defCursor;

    /// <summary>
    /// A kéz kurzor textúrája.
    /// </summary>
    [SerializeField] private Texture2D handCursor;

    /// <summary>
    /// A kamera mozgását jelző kurzor textúrája.
    /// </summary>
    [SerializeField] private Texture2D moveCamCursor;

    /// <summary>
    /// A kamera mozgását jelző kurzor forrópontja.
    /// </summary>
    [SerializeField] private Vector2 moveCamCursorHotspot;

    /// <summary>
    /// A kéz kurzor forrópontja.
    /// </summary>
    [SerializeField] private Vector2 handCursorHotspot;

    /// <summary>
    /// Az alapértelmezett kurzor forrópontja.
    /// </summary>
    [SerializeField] private Vector2 defCursorHotspot;

    /// <summary>
    /// A játékterület RectTransform objektuma.
    /// </summary>
    [SerializeField] private RectTransform playArea;

    /// <summary>
    /// A játék Canvas objektuma.
    /// </summary>
    [SerializeField] private Canvas gameCanvas;

    /// <summary>
    /// Meghatározza, hogy a kurzor a játékterületen belül van-e.
    /// </summary>
    public bool isCursorInPlayArea;

    [Header("Játék célja")]
    /// <summary>
    /// A turisták célértékei nehézségi szintenként.
    /// </summary>
    public readonly int[] goodVisitors = new int[] { 4, 100, 120 };

    /// <summary>
    /// A növényevők célértékei nehézségi szintenként.
    /// </summary>
    public readonly int[] goodPlantEaters = new int[] { 3, 40, 60 };

    /// <summary>
    /// A ragadozók célértékei nehézségi szintenként.
    /// </summary>
    public readonly int[] goodPredators = new int[] { 3, 30, 45 };

    /// <summary>
    /// A pénz célértékei nehézségi szintenként.
    /// </summary>
    public readonly int[] goodCashes = new int[] { 1000, 70000, 90000 };

    /// <summary>
    /// A cél eléréséhez szükséges hetek száma nehézségi szintenként.
    /// </summary>
    public readonly int[] goodPresetWeeks = new int[] { 3, 6, 12 };

    /// <summary>
    /// A kezdő pénzmennyiség nehézségi szintenként.
    /// </summary>
    public readonly int[] startMoneys = new int[] { 100000, 12500, 10000 };

    /// <summary>
    /// A célértékek turistákra vonatkozó értéke.
    /// </summary>
    public int goodVisitor;

    /// <summary>
    /// A célértékek növényevők számára vonatkozó értéke.
    /// </summary>
    public int goodPlantEater;

    /// <summary>
    /// A célértékek ragadozók számára vonatkozó értéke.
    /// </summary>
    public int goodPredator;

    /// <summary>
    /// A célértékek pénzre vonatkozó értéke.
    /// </summary>
    public int goodCash;

    /// <summary>
    /// A célértékek időre vonatkozó értéke.
    /// </summary>
    public int goodTime;

    /// <summary>
    /// A játék vége felület objektuma.
    /// </summary>
    public GameObject gameOverUI;

    /// <summary>
    /// A játék vége szöveg objektuma.
    /// </summary>
    public TMP_Text gameOverText;

    /// <summary>
    /// A kamera sebességeinek értékei különböző zoom szinteken.
    /// </summary>
    private readonly int[] camSpeeds = new int[] { 16, 13, 10, 6, 4 };


    /// <summary>
    /// Inicializálja a játékos vezérlőjét a játék indításakor.
    /// </summary>
    void Start()
    {
        //tempHour = hours;
        hours = 12;
        camMoveSpeed = camSpeeds[2];
        gameDifficulty = PlayerPrefs.GetInt("gameDifficulty");
        Debug.Log("J�t�k neh�zs�ge: " + gameDifficulty.ToString());

        goodVisitor = goodVisitors[gameDifficulty];
        goodPlantEater = goodPlantEaters[gameDifficulty];
        goodPredator = goodPredators[gameDifficulty];
        goodCash = goodCashes[gameDifficulty];
        goodTime = goodPresetWeeks[gameDifficulty];
        //goodReachingWeeks = goodPresetWeeks[gameDifficulty];
        playerCash = startMoneys[gameDifficulty];
        //months = 1;
        //tempMonths = 0;
    }

    /// <summary>
    /// Frissíti a játékos vezérlőjét minden képkockában.
    /// </summary>
    void Update()
    {
        if (!GameObject.Find("GameCanvas").GetComponent<UIController>().shopUI.activeSelf && !inNavigateMinimap && !GameObject.Find("GameCanvas").GetComponent<UIController>().pauseMenuUI.activeSelf)
        {
            CheckMousePos();
            SetCursor();
            CameraZoom();
            KeyboardCameraMovement();
            GameOver();
        }
        if (!isPaused) TimeUpdate();
        isMouseInGameArea = IsMouseInPlayArea();
    }

    /// <summary>
    /// Frissíti az időt a játékban.
    /// </summary>
    public void TimeUpdate()
    {
        if (!isPaused)
        {
            if (ellapsedTime >= 0)
            {
                ellapsedTime += Time.deltaTime;
                TimeHandle(ellapsedTime);
            }
        }
    }

    /// <summary>
    /// Módosítja az idő sebességét.
    /// </summary>
    /// <param name="speed">Az új idősebesség.</param>
    public void ChangeTimeSpeed(int speed)
    {
        timeSpeed = speed;
    }

    /// <summary>
    /// Kezeli az idő múlását a játékban.
    /// </summary>
    /// <param name="displayedTime">Az aktuálisan eltelt idő.</param>
    void TimeHandle(float displayedTime)
    {
        float interval = 0f;

        // Az időegység növekedésének intervalluma az idősebesség alapján
        switch (timeSpeed)
        {
            case 1: interval = 1f; break; // 1 másodpercenként 1 óra
            case 2: interval = 3f; break; // 3 másodpercenként 1 nap
            case 3: interval = 5f; break; // 5 másodpercenként 1 hét
        }

        if (displayedTime >= interval)
        {
            ellapsedTime = 0f;
            switch (timeSpeed)
            {
                case 1:
                    hours++;
                    oneWeekPassed = false;
                    if (hours >= 24)
                    {
                        hours = 0;
                        days++;
                        oneWeekPassed = false;
                    }
                    break;

                case 2:
                    days++;
                    oneWeekPassed = false;
                    break;

                case 3:
                    weeks++;
                    oneWeekPassed = true;
                    break;
            }

            if (days >= 7)
            {
                days = 0;
                weeks++;
                oneWeekPassed = true;
            }

            UpdateDirectionalLightRotation();
        }
    }

    /// <summary>
    /// Frissíti a Directional Light forgatását az idő alapján.
    /// </summary>
    void UpdateDirectionalLightRotation()
    {
        GameObject directionalLight = GameObject.Find("Directional Light");
        if (directionalLight != null)
        {
            float rotationAngle = (hours / 24f) * 360f;

            directionalLight.transform.rotation = Quaternion.Euler(new Vector3(rotationAngle - 90f, 170f, 0f));
        }
    }

    /// <summary>
    /// Mozgatja a kamerát a megadott irányba.
    /// </summary>
    /// <param name="direction">Az irány, amelybe a kamera mozog.</param>
    /// <param name="c">A mozgás mértéke.</param>
    void MoveCamera(bool direction, float c)
    {
        isCameraMoving = true;
        float horizontalMove = 0;
        float verticalMove = 0;
        if (direction) horizontalMove = c;
        else verticalMove = c;

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

        Vector3 moveDirection = (right * horizontalMove + forward * verticalMove).normalized;
        moveDirection.y = 0f;

        Vector3 limitedCamMove = transform.position + moveDirection * camMoveSpeed * Time.deltaTime;

        limitedCamMove.x = Mathf.Clamp(limitedCamMove.x, camMinPos.x, camMaxPos.x);
        limitedCamMove.z = Mathf.Clamp(limitedCamMove.z, camMinPos.z, camMaxPos.z);
        limitedCamMove.y = transform.position.y;

        transform.position = limitedCamMove;
    }

    /// <summary>
    /// Beállítja a kurzor megjelenését a kamera mozgása alapján.
    /// </summary>
    void SetCursor()
    {
        if (isCameraMoving) Cursor.SetCursor(moveCamCursor, moveCamCursorHotspot, CursorMode.Auto);
        else Cursor.SetCursor(defCursor, defCursorHotspot, CursorMode.Auto);
    }

    /// <summary>
    /// Visszaadja a RectTransform által lefedett területet a képernyőn.
    /// </summary>
    /// <param name="rectTransform">A RectTransform objektum.</param>
    /// <param name="cam">A kamera objektuma (opcionális).</param>
    /// <returns>A RectTransform által lefedett terület.</returns>
    public static Rect GetRenderRectFromRectTransform(RectTransform rectTransform, Camera cam = null)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 bottomLeft = cam != null ? cam.WorldToScreenPoint(corners[0]) : corners[0];
        Vector3 topRight = cam != null ? cam.WorldToScreenPoint(corners[2]) : corners[2];

        return new Rect(
            bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    /// <summary>
    /// Ellenőrzi az egér pozícióját, és mozgatja a kamerát, ha az egér a képernyő szélén van.
    /// </summary>
    void CheckMousePos()
    {
        bool moved = false;
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height) // <- HOGY NE MOZOGJON A KAMERA, HA AZ EG�R NINCS AZ ABLAKBAN.
        {

            // HA ITT CSAK IF-EKET HAGYUNK, AKKOR VAN OLDALIR�NY� KAMERAMOZG�S IS, AMIKOR A SAROKBA �R�NK
            // EZT MAJD M�G MEG KELL BESZ�LN�NK, HOGY MEGHAGYJUK-E

            Rect renderedGameArea = GetRenderRectFromRectTransform(rawImage.rectTransform, gameCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : gameCanvas.worldCamera);

            if ((Screen.height - mouseDistance) < Input.mousePosition.y && Input.mousePosition.x < renderedGameArea.width) { MoveCamera(false, 1); moved = true; } // fel
            if ((mouseDistance) > Input.mousePosition.y && Input.mousePosition.x < renderedGameArea.width) { MoveCamera(false, -1); moved = true; } // le
            if ((renderedGameArea.width - mouseDistance) < Input.mousePosition.x && Input.mousePosition.x < renderedGameArea.width) { MoveCamera(true, 1); moved = true; } // jobb
            if (mouseDistance > Input.mousePosition.x) { MoveCamera(true, -1); moved = true; } //bal

            //J�t�kmez� ellen�rz�se, hogy ne tudjunk lerakni �s r�kattintani semmrie sem, amikor nem a j�t�k renderelt r�sz�ben van az eg�r

        }
        isCameraMoving = moved;
    }


    /// <summary>
    /// Kezeli a kamera zoomolását az egér görgőjének segítségével.
    /// </summary>
    void CameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && cameraZoomState > minZoom)
        {
            cameraZoomState--;
            camZoomStateText.text = cameraZoomState.ToString() + " x";
            this.GetComponent<Camera>().orthographicSize += 2;
            camMoveSpeed = camSpeeds[cameraZoomState + 2];
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && cameraZoomState < maxZoom)
        {
            cameraZoomState++;
            camZoomStateText.text = cameraZoomState.ToString() + " x";
            this.GetComponent<Camera>().orthographicSize -= 2;
            camMoveSpeed = camSpeeds[cameraZoomState + 2];
        }
    }


    /// <summary>
    /// Ellenőrzi, hogy az egér a játékterületen belül van-e.
    /// </summary>
    /// <returns>Igaz, ha az egér a játékterületen belül van, különben hamis.</returns>
    bool IsMouseInPlayArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(playArea, Input.mousePosition, gameCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : gameCanvas.worldCamera);
    }

    /// <summary>
    /// Megjeleníti a játék vége képernyőt.
    /// </summary>
    public void ShowGameOver()
    {
        isPaused = true;
        gameOverUI.SetActive(!gameOverUI.activeSelf);

    }

    /// <summary>
    /// Kezeli a játék végét a feltételek alapján.
    /// </summary>
    void GameOver()
    {
        if (hasWon) return;
        if (hasLose) return;

        if (playerCash <= 0)
        {
            hasLose = true;
            gameOverText.text = "Vesztett�l!";
            ShowGameOver();
            return;
        }

        bool conditionsMet = playerCash >= goodCash &&
                             GridController.plantEaterCounter >= goodPlantEater &&
                             GridController.predatorCounter >= goodPredator &&
                             Jeep.satisfiedTourist >= goodVisitor;

        if (conditionsMet && weeks > lastCheckedWeek)
        {
            passedWeeks++;
            lastCheckedWeek = weeks;
            Debug.Log($"Felt�telek teljes�lnek! H�t sz�ml�l�: {passedWeeks}/{goodTime}");

            if (passedWeeks >= goodTime)
            {
                hasWon = true;
                gameOverText.text = "Nyert�l!";
                ShowGameOver();
            }
        }
        else if (weeks > lastCheckedWeek)
        {
            passedWeeks = 0;
            lastCheckedWeek = weeks;
        }
    }

    /// <summary>
    /// Kezeli a kamera mozgását a billentyűzet segítségével.
    /// </summary>
    void KeyboardCameraMovement()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical += 10f;
        if (Input.GetKey(KeyCode.S)) vertical -= 10f;
        if (Input.GetKey(KeyCode.D)) horizontal += 10f;
        if (Input.GetKey(KeyCode.A)) horizontal -= 10f;

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
        Vector3 direction = (forward * vertical + right * horizontal).normalized;

        if (direction.magnitude > 0f)
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? camSprintSpeed : camMoveSpeed;
            transform.position += direction * speed * Time.deltaTime;
            isCameraMoving = true;
        }
        else
        {
            isCameraMoving = false;
        }
    }
}
