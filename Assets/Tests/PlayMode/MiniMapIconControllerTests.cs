using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapIconControllerTests
{
    private GameObject controllerGO;
    private MiniMapIconController controller;

    [SetUp]
    public void SetUp()
    {
        controllerGO = new GameObject("MiniMapIconController");
        controller = controllerGO.AddComponent<MiniMapIconController>();

        // Mock Player és PlayerController
        var playerGO = new GameObject("Player");
        var playerController = playerGO.AddComponent<PlayerController>();
        controller.Player = playerGO;

        // Mock Minimap Icon
        controller.MinimapIcon = new GameObject("MiniMapIcon");

        // Mock Minimap Cameras
        controller.minimapCam = new GameObject("MinimapCam");
        controller.minimapCam.AddComponent<Camera>();

        controller.minimapCam2 = new GameObject("MinimapCam2");

        // Mock NavigateMinimap
        controller.navigateMinimap = new GameObject("NavigateMinimap");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerGO);
    }

    [Test]
    public void ShowMinimap_TogglesIsNavigateMap()
    {
        controller.isNavigateMap = false;

        controller.ShowMinimap();

        Assert.IsTrue(controller.isNavigateMap);
        Assert.IsTrue(controller.navigateMinimap.activeSelf);
        Assert.IsTrue(controller.Player.GetComponent<PlayerController>().isPaused);
        Assert.IsTrue(controller.Player.GetComponent<PlayerController>().inNavigateMinimap);

        controller.ShowMinimap();

        Assert.IsFalse(controller.isNavigateMap);
        Assert.IsFalse(controller.navigateMinimap.activeSelf);
        Assert.IsFalse(controller.Player.GetComponent<PlayerController>().isPaused);
        Assert.IsFalse(controller.Player.GetComponent<PlayerController>().inNavigateMinimap);
    }

    [Test]
    public void GetRenderRectFromRectTransform_ReturnsCorrectRect()
    {
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        var rectTransformGO = new GameObject("RectObject", typeof(RectTransform));
        rectTransformGO.transform.SetParent(canvas.transform);
        var rectTransform = rectTransformGO.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100);
        rectTransform.position = new Vector3(200, 200, 0);

        Rect rect = controller.GetRenderRectFromRectTransform(rectTransform);

        Assert.AreNotEqual(0, rect.width);
        Assert.AreNotEqual(0, rect.height);
    }
}
