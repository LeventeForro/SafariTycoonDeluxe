using UnityEngine;

/// <summary>
/// A minimap ikon �s kamera kezel�s��rt felel�s oszt�ly.
/// </summary>
public class MiniMapIconController : MonoBehaviour
{
    /// <summary>
    /// A j�t�kos objektuma.
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// A minimap ikon objektuma.
    /// </summary>
    public GameObject MinimapIcon;

    /// <summary>
    /// A minimap els�dleges kamer�ja.
    /// </summary>
    public GameObject minimapCam;

    /// <summary>
    /// A minimap m�sodlagos kamer�ja.
    /// </summary>
    public GameObject minimapCam2;

    /// <summary>
    /// A minimap kamera sebess�ge.
    /// </summary>
    public Vector3 camVelocity = Vector3.zero;

    /// <summary>
    /// A kamera mozg�s�nak sim�t�si ideje.
    /// </summary>
    public float sDampTime = 0.3f;

    /// <summary>
    /// A minimap kamera maxim�lis poz�ci�ja.
    /// </summary>
    public Vector2 camMaxPos;

    /// <summary>
    /// A minimap kamera minim�lis poz�ci�ja.
    /// </summary>
    public Vector2 camMinPos;

    /// <summary>
    /// Meghat�rozza, hogy a minimap navig�lhat�-e.
    /// </summary>
    public bool isNavigateMap = false;

    /// <summary>
    /// A minimap minim�lis zoom �rt�ke.
    /// </summary>
    public int minZoom = -10;

    /// <summary>
    /// A minimap maxim�lis zoom �rt�ke.
    /// </summary>
    public int maxZoom = 10;

    /// <summary>
    /// A minimap navig�ci� sebess�ge.
    /// </summary>
    public float speed;

    /// <summary>
    /// A minimap RectTransform objektuma.
    /// </summary>
    public RectTransform minimapRect;

    /// <summary>
    /// A j�t�k Canvas objektuma.
    /// </summary>
    public Canvas gameCanvas;

    /// <summary>
    /// A minimap kamera zoom �llapota.
    /// </summary>
    public int cameraZoomState;

    /// <summary>
    /// A navig�lhat� minimap objektuma.
    /// </summary>
    public GameObject navigateMinimap;

    /// <summary>
    /// Megjelen�ti vagy elrejti a navig�lhat� minimapet.
    /// </summary>
    public void ShowMinimap()
    {
        isNavigateMap = !isNavigateMap;
        navigateMinimap.SetActive(isNavigateMap);
        Player.GetComponent<PlayerController>().isPaused = isNavigateMap;
        Player.GetComponent<PlayerController>().inNavigateMinimap = isNavigateMap;
    }

    /// <summary>
    /// Friss�ti a minimap ikonj�nak poz�ci�j�t �s kezeli a billenty�zet esem�nyeket.
    /// </summary>
    void Update()
    {
        MinimapIcon.transform.position = new Vector3(Player.transform.position.x + 50, 810, Player.transform.position.z + 50);
        HandleKeys();
        Zoom();
    }

    /// <summary>
    /// Kezeli a minimap zoomol�s�t.
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
    /// Kezeli a minimap navig�ci�s billenty�zet esem�nyeit.
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
    /// Visszaadja a RectTransform �ltal lefedett ter�letet.
    /// </summary>
    /// <param name="rectTransform">A RectTransform objektum.</param>
    /// <param name="cam">A kamera objektuma (opcion�lis).</param>
    /// <returns>A RectTransform �ltal lefedett ter�let.</returns>
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
    /// Friss�ti a minimap kamer�k poz�ci�j�t minden k�pkock�ban.
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
    /// Ellen�rzi, hogy az eg�rmutat� a minimap ter�let�n bel�l van-e.
    /// </summary>
    /// <returns>Igaz, ha az eg�rmutat� a minimap ter�let�n bel�l van, k�l�nben hamis.</returns>
    bool IsMouseInMinimapArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(minimapRect, Input.mousePosition, gameCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : gameCanvas.worldCamera);
    }
}
