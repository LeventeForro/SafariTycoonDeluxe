using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class PlayerControllerTests
{
    private PlayerController controller;

    [SetUp]
    public void SetUp()
    {
        var gameObject = new GameObject();
        controller = gameObject.AddComponent<PlayerController>();
        PlayerController.timeSpeed = 0;
        controller.ellapsedTime = 0f;
    }

    [Test]
    public void TimeHandle_HoursIncrease_WhenTimeSpeedIs1AndIntervalPassed()
    {
        controller.hours = 0;
        controller.days = 0;
        PlayerController.timeSpeed = 1; // fontos!
        controller.ellapsedTime = 1.0f; // pont elegendő az 1 órához

        var method = typeof(PlayerController).GetMethod("TimeHandle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(controller, new object[] { 1.0f });

        Assert.AreEqual(1, controller.hours);
    }

    [Test]
    public void TimeHandle_DaysIncrease_When24HoursPassed()
    {
        controller.hours = 23;
        controller.days = 0;
        PlayerController.timeSpeed = 1;
        controller.ellapsedTime = 1.0f;

        var method = typeof(PlayerController).GetMethod("TimeHandle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(controller, new object[] { 1.0f });

        Assert.AreEqual(0, controller.hours);
        Assert.AreEqual(1, controller.days);
    }

    [Test]
    public void TimeHandle_DaysIncrease_WhenTimeSpeedIs2AndIntervalPassed()
    {
        controller.days = 0;
        PlayerController.timeSpeed = 2;
        controller.ellapsedTime = 3.0f; // 3 mp szükséges 1 naphoz

        var method = typeof(PlayerController).GetMethod("TimeHandle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(controller, new object[] { 3.0f });

        Assert.AreEqual(1, controller.days);
    }

    [Test]
    public void TimeHandle_WeeksIncrease_WhenTimeSpeedIs3AndIntervalPassed()
    {
        controller.weeks = 0;
        PlayerController.timeSpeed = 3;
        controller.ellapsedTime = 5.0f; // 5 mp szükséges 1 hét növeléshez

        var method = typeof(PlayerController).GetMethod("TimeHandle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(controller, new object[] { 5.0f });

        Assert.AreEqual(1, controller.weeks);
        Assert.IsTrue(controller.oneWeekPassed);
    }



}
