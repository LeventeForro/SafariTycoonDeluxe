using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RoadTileTests
{
    [SetUp]
    public void Setup()
    {
        RoadTile.allRoadTiles.Clear();
    }

    [Test]
    public void RoadTile_AddedToStaticList_OnAwake()
    {
        // Arrange
        GameObject go = new GameObject();
        RoadTile roadTile = go.AddComponent<RoadTile>();

        roadTile.Awake();

        // Assert
        Assert.Contains(roadTile, RoadTile.allRoadTiles);
        Assert.AreEqual(2, RoadTile.allRoadTiles.Count);
    }
}
