using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject bulletProjectilePrefab;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletProjectilePrefab, muzzle.position, Quaternion.identity);

        BulletProjectile bulletProjectile = bullet.GetComponent<BulletProjectile>();

        Vector3 targetUnittShootAtPosition = e.targetUnit.GetWorldPosition;
        targetUnittShootAtPosition.y = muzzle.position.y;
        bulletProjectile.Setup(targetUnittShootAtPosition);
    }
}
