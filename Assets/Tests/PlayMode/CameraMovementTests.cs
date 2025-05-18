using NUnit.Framework;
using UnityEngine;
using TMPro;

public class CameraMovementTests
{
    private GameObject cameraObject;
    private CameraMovement cameraMovement;
    private TMP_Text zoomText;
    private Camera cameraComponent;

    [SetUp]
    public void SetUp()
    {
        cameraObject = new GameObject();
        cameraMovement = cameraObject.AddComponent<CameraMovement>();
        
        cameraComponent = cameraObject.AddComponent<Camera>();

        GameObject textObject = new GameObject();
        zoomText = textObject.AddComponent<TextMeshProUGUI>();
        cameraMovement.camZoomStateText = zoomText;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(cameraObject);
    }

    [Test]
    public void ChangeCamMoveMode_TogglesCamMoveType()
    {
        Assert.IsFalse(cameraMovement.camMoveType);

        cameraMovement.ChangeCamMoveMode();

        Assert.IsTrue(cameraMovement.camMoveType);

        cameraMovement.ChangeCamMoveMode();

        Assert.IsFalse(cameraMovement.camMoveType);
    }

    [Test]
    public void Start_SetsZoomTextCorrectly()
    {
        cameraMovement.cameraZoomState = 3;

        cameraMovement.Start();

        Assert.AreEqual("3 x", zoomText.text);
    }
}
