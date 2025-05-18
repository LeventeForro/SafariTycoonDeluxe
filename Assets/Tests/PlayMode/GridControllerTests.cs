using NUnit.Framework;
using UnityEngine;

public class GridControllerTests
{
    private GridController gridController;
    private GameObject controllerGO;

    [SetUp]
    public void SetUp()
    {
        controllerGO = new GameObject("GridController");
        gridController = controllerGO.AddComponent<GridController>();
        gridController.player = new GameObject("Player").AddComponent<PlayerController>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(controllerGO);
    }

    [TestCase(0f, ExpectedResult = true)]
    [TestCase(3f, ExpectedResult = true)]
    [TestCase(4f, ExpectedResult = false)]
    [TestCase(6f, ExpectedResult = true)]
    public bool TerrainHeightCheck_ReturnsExpected(float height)
    {
        var method = typeof(GridController).GetMethod("TerrainHeightCheck", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)method.Invoke(gridController, new object[] { height });
    }

    [Test]
    public void Cheat_Suska_AddsMoney()
    {
        // Arrange
        gridController.player.playerCash = 0;

        // Act
        gridController.Cheat("suska");

        // Assert
        Assert.AreEqual(50000, gridController.player.playerCash);
    }

    [Test]
    public void Cheat_WrongCode_DoesNotAddMoney()
    {
        // Arrange
        gridController.player.playerCash = 0;

        // Act
        gridController.Cheat("nincsilyen");

        // Assert
        Assert.AreEqual(0, gridController.player.playerCash);
    }
}
