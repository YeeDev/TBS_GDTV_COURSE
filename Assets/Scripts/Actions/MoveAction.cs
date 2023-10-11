using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxMoveDistance = 4;

    private Vector3 targetPosition;

    public override string GetActionName() => "Move";

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    private void Update()
    {
        if (!isActive) { return; }

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float stoppingDistance = 0.1f;
        if ((transform.position - targetPosition).sqrMagnitude > stoppingDistance)
        {
            float moveSpeed = 4;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition;

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) { continue; }
                if (unitGridPosition == testGridPosition) { continue; }
                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) { continue; }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
