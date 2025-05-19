using NUnit.Framework;
using System.Collections.Generic;
using System;

using UnityEngine;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;


public class NavmeshCell
{
    public NavmeshCell(Vector3 pos)
    {
        position = pos;
        edges = new List<NavmeshCell>();
    }
    public float hScore;
    public float gScore;
    public NavmeshCell previousOnPath;

    public float GetFScore()
    {
        return hScore + gScore;
    }
    public Vector3 position;
    public List<NavmeshCell> edges;
    public override bool Equals(object obj)
    {
        if (obj is NavmeshCell other)
        {
            return position == other.position;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}

public class Navmesh 
{
    private float edgeLength;
    private Vector2 levelDimensions;
    private Vector3 levelCenter;
    private float distanceToFlyzoneEdge;
    private LevelConfig levelConfig;
    private NavmeshCell[,] cells;

    private static Navmesh _instance;
    public static Navmesh instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Navmesh();
                return _instance;
            }
            return _instance;
        }
    }
    private Navmesh()
    { 
        LevelConfig levelConfig = LevelConfig.instance;
        Debug.Assert(levelConfig != null);
        edgeLength = levelConfig.navmeshEdgeLength;
        levelCenter = levelConfig.backgroundCenterPositon;
        levelDimensions = levelConfig.backgroundDimensions;
        distanceToFlyzoneEdge = levelConfig.flyZoneDepth;
        LayerMask obstacleMask = LayerMask.GetMask("Obstacle");

        float raycastStartZ = levelCenter.z - distanceToFlyzoneEdge;
        float raycastEndZ = levelCenter.z;

        // create a 2D list of cells for each potential cell
        // we can use this later to translate the location of a bee in the world to a node in the graph
        int nbCellsX = (int) (levelDimensions.x / edgeLength);
        float gapFromStartX = levelDimensions.x % edgeLength / 2;
        
        int nbCellsY = (int) (levelDimensions.y / edgeLength);
        float gapFromStartY = (int)levelDimensions.y % edgeLength / 2;

        float firstX = gapFromStartX + levelCenter.x - levelDimensions.x / 2;
        float firstY = levelCenter.y + levelDimensions.y / 2 - gapFromStartY;

        cells = new NavmeshCell[nbCellsY, nbCellsX];

        for (int y = 0; y < nbCellsY; y++)
        {
            float posY = firstY - y * edgeLength;
            for (int x = 0; x < nbCellsX; x++)
            {
                float posX = firstX + x * edgeLength;
                Vector2 xyPosition = new Vector2(posX, posY);
                Vector3 raycastStart = new Vector3(posX, posY, raycastStartZ);
                Vector3 raycastEnd = new Vector3(posX, posY, raycastEndZ);
                // now we check whether there is anything at this location

                if (Physics.Raycast(raycastStart, raycastEnd - raycastStart, distanceToFlyzoneEdge, obstacleMask)) continue;

                cells[y, x] = new NavmeshCell(raycastEnd);
            }
        }
        // now construct the edges between cells

        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };

        for (int y = 0; y < nbCellsY; y++)
        {
            for (int x = 0; x < nbCellsX; x++)
            {
                if (cells[y, x] != null)
                {
                    for (int dir = 0; dir < 8; dir++)
                    {
                        int nx = x + dx[dir];
                        int ny = y + dy[dir];

                        if (nx >= 0 && nx < nbCellsX && ny >= 0 && ny < nbCellsY && cells[ny, nx] != null)
                        {
                            if (!Physics.Linecast(cells[y, x].position, cells[ny, nx].position, obstacleMask))
                            {
                                cells[y, x].edges.Add(cells[ny, nx]);
                            }
                        }
                    }
                }
            }
        }

    }

    public void InitializeSimpleGrid(Navmesh grid, int width, int height, float cellSize)
    {
        grid.levelCenter = Vector3.zero;
        grid.levelDimensions = new Vector2(width * cellSize, height * cellSize);
        grid.edgeLength = cellSize;
        grid.cells = new NavmeshCell[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = new Vector3(x * cellSize - width / 2f * cellSize + cellSize / 2f,
                                      y * -cellSize + height / 2f * cellSize - cellSize / 2f,
                                      0);
                grid.cells[y, x] = new NavmeshCell(pos);
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = grid.cells[y, x];
                if (x > 0) cell.edges.Add(grid.cells[y, x - 1]);
                if (x < width - 1) cell.edges.Add(grid.cells[y, x + 1]);
                if (y > 0) cell.edges.Add(grid.cells[y - 1, x]);
                if (y < height - 1) cell.edges.Add(grid.cells[y + 1, x]);
            }
        }
    }


    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {

        if (!Application.isPlaying) return;

        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                if (cells[y, x] != null)
                {
                    Vector3 pos = cells[y, x].position;
                    Gizmos.DrawSphere(pos, 0.2f);
                    foreach (var neighbor in cells[y, x].edges)
                    {
                        Vector3 otherPos = neighbor.position;
                        Gizmos.DrawLine(otherPos, pos);
                    }
                }
            }
        }
    }

    public List<Vector3> GetPathFromTo(Vector3 from, Vector3 to)
    {
        // first find the closest nodes to the from and to locations
        NavmeshCell startCell = GetClosestCell(from);
        NavmeshCell endCell = GetClosestCell(to);
        Debug.Log("Start cell position:" + startCell.position);
        Debug.Log("End cell position:" + endCell.position);
        var openSet = new PriorityQueue<NavmeshCell>();
        var closedSet = new HashSet<NavmeshCell>();

        startCell.gScore = 0;
        startCell.hScore = CalculateHeuristic(startCell, endCell);
        startCell.previousOnPath = null;

        openSet.Enqueue(startCell, startCell.GetFScore());
        
        while (openSet.Count > 0)
        {
            var cell = openSet.Dequeue();
            if (cell == endCell)
            {
                return WalkBackToGeneratePath(endCell, startCell);
            }
            // don't forget these two checks
            if (closedSet.Contains(cell))
                continue;

            closedSet.Add(cell);

            foreach (var neighbor in cell.edges)
            {
                //don't forget these two checks
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }
                float potentialGScore = cell.gScore +
                 Vector2.Distance(cell.position, neighbor.position);
                float potentialHScore = CalculateHeuristic(neighbor, endCell);
                float potentialFScore = potentialGScore + potentialHScore;

                if (openSet.Contains(neighbor))
                {
                    if (potentialGScore < neighbor.gScore)
                    {
                        openSet.UpdatePriority(neighbor, potentialFScore);
                        neighbor.gScore = potentialGScore;
                        neighbor.hScore = potentialHScore;
                        neighbor.previousOnPath = cell;
                    }

                }
                else 
                {
                    neighbor.gScore = potentialGScore;
                    neighbor.hScore = potentialHScore;
                    openSet.Enqueue(neighbor, neighbor.GetFScore());
                    neighbor.previousOnPath = cell;
                }
            }
        }

        return new List<Vector3>();
    }

    public NavmeshCell GetClosestCell(Vector3 worldPos)
    {
        float halfWidth = levelDimensions.x / 2f;
        float halfHeight = levelDimensions.y / 2f;

        float startX = levelCenter.x - halfWidth;
        float startY = levelCenter.y + halfHeight;

        int xIndex = Mathf.FloorToInt((worldPos.x - startX) / edgeLength);
        int yIndex = Mathf.FloorToInt((startY - worldPos.y) / edgeLength);

        // Clamp to grid bounds
        xIndex = Mathf.Clamp(xIndex, 0, cells.GetLength(1) - 1);
        yIndex = Mathf.Clamp(yIndex, 0, cells.GetLength(0) - 1);

        return cells[yIndex, xIndex];
    }

    private List<Vector3> WalkBackToGeneratePath(NavmeshCell end, NavmeshCell start)
    {
        // remmeber we are not adding start
        var previous = end;
        var path = new List<Vector3>();

        while (previous != start)
        {
            Debug.Assert(previous != null);
            path.Add(previous.position);
            previous = previous.previousOnPath;
        }
        path.Reverse();
        return path;
    }



    private float CalculateHeuristic(NavmeshCell from, NavmeshCell to)
    {
        return Vector2.Distance(from.position, to.position);
    }

}
