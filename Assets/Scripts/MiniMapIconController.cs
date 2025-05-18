using UnityEngine;

/// <summary>
/// A minimap ikon és kamera kezeléséért felelõs osztály.
/// </summary>
public class MiniMapIconController : MonoBehaviour
{
    /// <summary>
    /// A játékos objektuma.
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// A minimap ikon objektuma.
    /// </summary>
    public GameObject MinimapIcon;

    /// <summary>
    /// A minimap elsõdleges kamerája.
    /// </summary>
    public GameObject minimapCam;

    /// <summary>
    /// A minimap másodlagos kamerája.
    /// </summary>
    public GameObject minimapCam2;

    /// <summary>
    /// A minimap kamera sebessége.
    /// </summary>
    public Vector3 camVelocity = Vector3.zero;

    /// <summary>
    /// A kamera mozgásának simítási ideje.
    /// </summary>
    public float sDampTime = 0.3f;

    /// <summary>
    /// A minimap kamera maximális pozíciója.
    /// </summary>
    public Vector2 camMaxPos;

    /// <summary>
    /// A minimap kamera minimális pozíciója.
    /// </summary>
    public Vector2 camMinPos;

    /// <summary>
    /// Meghatározza, hogy a minimap navigálható-e.
    /// </summary>
    public bool isNavigateMap = false;

    /// <summary>
    /// A minimap minimális zoom értéke.
    /// </summary>
    public int minZoom = -10;

    /// <summary>
    /// A minimap maximális zoom értéke.
    /// </summary>
    public int maxZoom = 10;

    /// <summary>
    /// A minimap navigáció sebessége.
    /// </summary>
    public float speed;

    /// <summary>
    /// A minimap RectTransform objektuma.
    /// </summary>
    public RectTransform minimapRect;

    /// <summary>
    /// A játék Canvas objektuma.
    /// </summary>
    public Canvas gameCanvas;

    /// <summary>
    /// A minimap kamera zoom állapota.
    /// </summary>
    public int cameraZoomState;

    /// <summary>
    /// A navigálható minimap objektuma.
    /// </summary>
    public GameObject navigateMinimap;

    /// <summary>
    /// Megjeleníti vagy elrejti a navigálható minimapet.
    /// </summary>
    public void ShowMinimap()
    {
        isNavigateMap = !isNavigateMap;
        navigateMinimap.SetActive(isNavigateMap);
        Player.GetComponent<PlayerController>().isPaused = isNavigateMap;
        Player.GetComponent<PlayerController>().inNavigateMinimap = isNavigateMap;
    }

    /// <summary>
    /// Frissíti a minimap ikonjának pozícióját és kezeli a billentyûzet eseményeket.
    /// </summary>
    void Update()
    {
        MinimapIcon.transform.position = new Vector3(Player.transform.position.x + 50, 810, Player.transform.position.z + 50);
        HandleKeys();
        Zoom();
    }

    /// <summary>
    /// Kezeli a minimap zoomolását.
    /// </summary>
    void Zoom()
    {
        if (isNavigateMap)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && cameraZoomState > minZoom)
            {
                cameraZoomState--;
                minimapCam.GetComponent<Camera>().orthographicSize += 2;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0 && cameraZoomState < maxZoom)
            {
                cameraZoomState++;
                minimapCam.GetComponent<Camera>().orthographicSize -= 2;
            }
        }
    }

    /// <summary>
    /// Kezeli a minimap navigációs billentyûzet eseményeit.
    /// </summary>
    void HandleKeys()
    {
        if (isNavigateMap)
        {
            if (Input.GetMouseButton(0) && IsMouseInMinimapArea())
            {
                Vector3 pos = minimapCam.transform.position;
                pos.x -= Input.GetAxis("Mouse X") * speed * Time.deltaTime;
                pos.z -= Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

                pos.x = Mathf.Clamp(pos.x, camMinPos.x, camMaxPos.x);
                pos.z = Mathf.Clamp(pos.z, camMinPos.y, camMaxPos.y);

                minimapCam.transform.position = pos;
            }
        }
    }

    /// <summary>
    /// Visszaadja a RectTransform által lefedett területet.
    /// </summary>
    /// <param name="rectTransform">A RectTransform objektum.</param>
    /// <param name="cam">A kamera objektuma (opcionális).</param>
    /// <returns>A RectTransform által lefedett terület.</returns>
    public Rect GetRenderRectFromRectTransform(RectTransform rectTransform, Camera cam = null)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector3 bottomLeft = cam != null ? cam.WorldToScreenPoint(corners[0]) : corners[0];
        Vector3 topRight = cam != null ? cam.WorldToScreenPoint(corners[2]) : corners[2];

        return new Rect(
            bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    /// <summary>
    /// Frissíti a minimap kamerák pozícióját minden képkockában.
    /// </summary>
    private void LateUpdate()
    {
        if (!isNavigateMap)
        {
            Vector3 minimapCamVel = Vector3.SmoothDamp(minimapCam.transform.position, MinimapIcon.transform.position, ref camVelocity, sDampTime);

            float clampedX = Mathf.Clamp(minimapCamVel.x, camMinPos.x, camMaxPos.x);
            float clampedZ = Mathf.Clamp(minimapCamVel.z, camMinPos.y, camMaxPos.y);

            minimapCam2.transform.position = new Vector3(clampedX, minimapCam.transform.position.y, clampedZ);
        }
    }

    /// <summary>
    /// Ellenõrzi, hogy az egérmutató a minimap területén belül van-e.
    /// </summary>
    /// <returns>Igaz, ha az egérmutató a minimap területén belül van, különben hamis.</returns>
    bool IsMouseInMinimapArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(minimapRect, Input.mousePosition, gameCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : gameCanvas.worldCamera);
    }
}
