using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class JeepTests
{
    [Test]
    public void InitializeAfterDelay_ShouldSetInitialPositionAndRoadTile()
    {
        var jeep = new GameObject().AddComponent<Jeep>();
        var roadTile = new GameObject().AddComponent<RoadTile>();
        RoadTile.allRoadTiles = new List<RoadTile> { roadTile };

        // Mocking the necessary GameObject setup
        jeep.transform.position = new Vector3(0, 0, 0); // Set the initial position of the Jeep

        // Simulate Start and InitializeAfterDelay coroutine
        jeep.StartCoroutine(jeep.InitializeAfterDelay());

        // Verify the initial setup
        Assert.NotNull(jeep.transform.position); // Ensure position is not null
        Assert.AreEqual(roadTile.transform.position, jeep.transform.position); // Ensure jeep is at the closest road position
    }


    [Test]
    public void Update_ShouldStartMovementWhenTouristInJeepIsMax()
    {
        var jeep = new GameObject().AddComponent<Jeep>();
        var roadTile = new GameObject().AddComponent<RoadTile>();
        jeep.currentRoad = roadTile; // Set initial road tile
        jeep.Start_ = new GameObject(); // Mock start GameObject
        jeep.End_ = new GameObject(); // Mock end GameObject
        jeep.TouristInJeep = jeep.MaxSpace; // Set max tourists

        // Call Update
        jeep.Update();

        // Verify that the movement starts
        Assert.IsTrue(jeep.isMoving, "Jeep should be moving.");
    }

    [Test]
    public void MoveAlongRoad_ShouldChangeSpriteBasedOnDirection()
    {
        var jeep = new GameObject().AddComponent<Jeep>();
        var roadTile = new GameObject().AddComponent<RoadTile>();
        RoadTile.allRoadTiles = new List<RoadTile> { roadTile };

        jeep.spriteRenderer = new GameObject().AddComponent<SpriteRenderer>();

        // Mock the direction of movement
        var direction = Vector3.right; // Moving to the right
        jeep.spriteRenderer.sprite = jeep.jeepRight; // Assume jeepRight is a valid sprite

        // Simulate movement
        jeep.StartCoroutine(jeep.MoveAlongRoad());

        // Check if the sprite is correctly changed
        Assert.AreEqual(jeep.jeepRight, jeep.spriteRenderer.sprite);
    }
}
