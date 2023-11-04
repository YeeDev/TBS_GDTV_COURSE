using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro gCostTexT;
    [SerializeField] private TextMeshPro hCostTexT;
    [SerializeField] private TextMeshPro fCostTexT;
    [SerializeField] private SpriteRenderer walkableSprite;

    private PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        pathNode = (PathNode)gridObject;
        base.SetGridObject(gridObject);
    }

    protected override void Update()
    {
        base.Update();
        gCostTexT.text = pathNode.GCost.ToString();
        hCostTexT.text = pathNode.HCost.ToString();
        fCostTexT.text = pathNode.GetFCost.ToString();
        walkableSprite.color = pathNode.IsWalkable ? Color.green : Color.red;
    }
}
