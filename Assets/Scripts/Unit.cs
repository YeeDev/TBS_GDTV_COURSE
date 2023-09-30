using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Vector3 targetPosition;

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if ((transform.position - targetPosition).sqrMagnitude > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            animator.SetBool("IsWalking", true);
        }
        else { animator.SetBool("IsWalking", false); }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
