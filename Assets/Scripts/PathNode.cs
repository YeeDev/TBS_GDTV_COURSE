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

    public int GetGCost => gCost;
    public int GetHCost => hCost;
    public int GetFCost => fCost;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition; 
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }
}
