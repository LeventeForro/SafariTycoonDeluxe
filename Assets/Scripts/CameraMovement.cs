using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    /*
     * EZ CSAK EGY �TMENETI KAMERA KEZEL�
     * AZ ITT L�V� K�D MAJD A PLAYER SCRIPT-BE FOG �TMENNI
     * MIVEL EGYEL�RE AZ M�G NINCS K�SZ, EZ�RT ITT HAGYOM
     */
    [Header("Mozg�s")]
    public float camMoveSpeed;
    public float camSprintSpeed;
    public bool camMoveType = false;
    public float mouseDistance = Screen.width * 0.2f;
    public bool isCameraMoving = false;


    public int cameraZoomState = 0;
    public int minZoom, maxZoom = 0;
    public TMP_Text camZoomStateText;
    public RawImage rawImage;

    [Header("Kurzor")]
    [SerializeField] private Texture2D defCursor;
    [SerializeField] private Texture2D handCursor;
    [SerializeField] private Texture2D moveCamCursor;
    [SerializeField] private Vector2 moveCamCursorHotspot;
    [SerializeField] private Vector2 handCursorHotspot;
    [SerializeField] private Vector2 defCursorHotspot;

    // Start is called before the first frame update
    public void Start()
    {
        camZoomStateText.text = cameraZoomState.ToString() + " x";
    }



    public void ChangeCamMoveMode()
    {
        this.camMoveType = !this.camMoveType;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckMousePos();
        //SetCursor();
        CameraZoom();
        KeyboardCameraMovement();
    }

    /*
    void MoveCamera(bool direction, float c)
    {
        isCameraMoving = true;
        float horizontalMove = 0;
        float verticalMove = 0; // Input.GetAxis("Vertical");
        if (direction) horizontalMove = c;// = Input.GetAxis("Horizontal");
        else verticalMove = c;

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;

        Vector3 moveDirection = (right * horizontalMove + forward * verticalMove).normalized;
        moveDirection.y = 0f;

        //Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //float speed = Input.GetKey(KeyCode.LeftShift) ? camSprintSpeed : camMoveSpeed;



        transform.position += moveDirection * camMoveSpeed * Time.deltaTime;
    }
    
    void SetCursor()
    {
        if (isCameraMoving) Cursor.SetCursor(moveCamCursor, moveCamCursorHotspot, CursorMode.Auto);
        else Cursor.SetCursor(defCursor, defCursorHotspot, CursorMode.Auto);
    }

    void CheckMousePos()
    {
        bool moved = false;
        if (Input.mousePosition.x > 0 && Input.mousePosition.x < Screen.width && Input.mousePosition.y > 0 && Input.mousePosition.y < Screen.height) // <- HOGY NE MOZOGJON A KAMERA, HA AZ EG�R NINCS AZ ABLAKBAN.
        {
            // HA ITT CSAK IF-EKET HAGYUNK, AKKOR VAN OLDALIR�NY� KAMERAMOZG�S IS, AMIKOR A SAROKBA �R�NK
            // EZT MAJD M�G MEG KELL BESZ�LN�NK, HOGY MEGHAGYJUK-E

            if ((Screen.height - mouseDistance) < Input.mousePosition.y && Input.mousePosition.x < rawImage.rectTransform.rect.width) { MoveCamera(false, 1); moved = true; } // fel
            if ((mouseDistance) > Input.mousePosition.y && Input.mousePosition.x < rawImage.rectTransform.rect.width) { MoveCamera(false, -1); moved = true; } // le
            if ((rawImage.rectTransform.rect.width - mouseDistance) < Input.mousePosition.x && Input.mousePosition.x < rawImage.rectTransform.rect.width) { MoveCamera(true, 1); moved = true; } // jobb
            if (mouseDistance > Input.mousePosition.x) { MoveCamera(true, -1); moved = true; } //bal
        }
        isCameraMoving = moved;
    }
    */
    void CameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && cameraZoomState > minZoom)
        {
            cameraZoomState--;
            camZoomStateText.text = cameraZoomState.ToString() + " x";
            this.GetComponent<Camera>().orthographicSize += 2;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && cameraZoomState < maxZoom)
        {
            cameraZoomState++;
            camZoomStateText.text = cameraZoomState.ToString() + " x";
            this.GetComponent<Camera>().orthographicSize -= 2;
        }
    }

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
