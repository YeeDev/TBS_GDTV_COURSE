using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostTexT;
    [SerializeField] private TextMeshPro hCostTexT;
    [SerializeField] private TextMeshPro fCostTexT;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        pathNode = (PathNode)gridObject;
        base.SetGridObject(gridObject);
    }

    protected override void Update()
    {
        base.Update();
        gCostTexT.text = pathNode.GetGCost.ToString();
        hCostTexT.text = pathNode.GetHCost.ToString();
        fCostTexT.text = pathNode.GetFCost.ToString();
    }
}
