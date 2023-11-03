using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;

    private PathNode cameFromPathNode;

    public int GCost { get => gCost; set => gCost = value; }
    public int HCost { get => hCost; set => hCost = value; }
    public int GetFCost => gCost + hCost;
    public GridPosition GetGridPosition => gridPosition;

    public PathNode CameFromPathNode { get => cameFromPathNode; set => cameFromPathNode = value; }
    public void ResetCameFromPathNode() => cameFromPathNode = null;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition; 
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }
}
