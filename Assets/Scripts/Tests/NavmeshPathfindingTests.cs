
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/*
public class NavmeshPathfindingTests
{
    private Navmesh navmeshGrid;

    [SetUp]
    public void SetUp()
    {
        // Create a simple 3x3 grid centered at (0,0)
        GameObject gridGO = new GameObject("NavmeshGrid");
        navmeshGrid = gridGO.AddComponent<Navmesh>();
        navmeshGrid.InitializeSimpleGrid(navmeshGrid, 3, 3, 1f);
    }

    [Test]
    public void PathExists_ValidStartAndEnd_ReturnsPath()
    {
        Vector3 from = new Vector3(-1, 1);  // Top-left
        Vector3 to = new Vector3(1, -1);    // Bottom-right

        List<Vector3> path = navmeshGrid.GetPathFromTo(from, to);

        Assert.IsNotNull(path);
        Assert.IsTrue(path.Count > 0);
        Assert.AreEqual(navmeshGrid.GetClosestCell(to).position, path[^1]);
    }

    [Test]
    public void StartEqualsEnd_ReturnsEmptyOrSingleNodePath()
    {
        Vector3 pos = Vector3.zero;

        List<Vector3> path = navmeshGrid.GetPathFromTo(pos, pos);

        Assert.IsNotNull(path);
        Assert.IsTrue(path.Count == 0 || path.Count == 1);
    }

    [Test]
    public void NoPathExists_ReturnsNullOrEmpty()
    {
        // Disconnect one cell
        var cell = navmeshGrid.GetClosestCell(new Vector3(0, 0));
        cell.edges.Clear();

        List<Vector3> path = navmeshGrid.GetPathFromTo(cell.position, new Vector3(1, 1));

        Assert.IsTrue(path == null || path.Count == 0);
    }

    // Helper: Initializes a 2D grid with 4-directional connectivity
    
}*/