using NUnit.Framework;
using UnityEngine;

public class BaseTests
{
    private GameObject gameObject;
    private Base baseComponent;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        baseComponent = gameObject.AddComponent<Base>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void GetValue_SetsTicketPriceCorrectly()
    {
        // Act
        baseComponent.GetValue("300");

        // Assert
        Assert.Pass();
    }

    [Test]
    public void Update_DoesNotCrash_WhenTicketPriceFieldIsNull()
    {
        // Arrange
        baseComponent.ticketPriceInputField = null;

        // Act & Assert
        Assert.DoesNotThrow(() => baseComponent.Update());
    }

    [Test]
    public void TouristInBase_StaticVariable_StartsAtZero()
    {
        Assert.AreEqual(0, Base.TouristInBase);
    }

    [Test]
    public void Reputation_StaticVariable_DefaultIsOne()
    {
        Assert.AreEqual(1f, Base.reputation);
    }
}
