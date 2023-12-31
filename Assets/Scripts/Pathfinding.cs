using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIALGONAL_COST = 14  ;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstacleLayerMask;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;
    public bool IsWalkableGridPosition(GridPosition gridPosition) { return gridSystem.GetGridObject(gridPosition).IsWalkable; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one Pathfinding!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float offset = 5;

                if (Physics.Raycast(worldPosition + Vector3.down * offset, Vector3.up, offset * 2, obstacleLayerMask))
                {
                    GetNode(x, z).IsWalkable = false;
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPosition);
        PathNode endNode = gridSystem.GetGridObject(endPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth; x++)
        {
            for (int z = 0; z < gridSystem.GetHeight; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.GCost = int.MaxValue;
                pathNode.HCost = 0;
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateDistance(startPosition, endPosition);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.GetFCost;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) { continue; }
                if (!neighbourNode.IsWalkable) { closedList.Add(neighbourNode); continue; }


                int tentativeGCost = currentNode.GCost + CalculateDistance(currentNode.GetGridPosition, neighbourNode.GetGridPosition);

                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.CameFromPathNode = currentNode;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistance(neighbourNode.GetGridPosition, endPosition);

                    if (!openList.Contains(neighbourNode)) { openList.Add(neighbourNode); }
                }
            }
        }

        pathLength = 0;
        return null;
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a - b;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIALGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost < lowestFCostPathNode.GetFCost)
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private List<PathNode> GetNeighbourList(PathNode currenNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currenNode.GetGridPosition;

        if (gridPosition.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0) { neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1)); }
            if (gridPosition.z + 1 < gridSystem.GetHeight) { neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1)); }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth)
        {
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z));
            if (gridPosition.z - 1 >= 0) { neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1)); }
            if (gridPosition.z + 1 < gridSystem.GetHeight) { neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1)); }
        }

        if (gridPosition.z - 1 >= 0) { neighbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1)); }
        if (gridPosition.z + 1 < gridSystem.GetHeight) { neighbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1)); }
        return neighbourList;
    }

    private PathNode GetNode(int x, int z) => gridSystem.GetGridObject(new GridPosition(x, z));

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;
        while (currentNode.CameFromPathNode != null)
        {
            pathNodeList.Add(currentNode.CameFromPathNode);
            currentNode = currentNode.CameFromPathNode;
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition);
        }

        return gridPositionList;
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null; 
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
