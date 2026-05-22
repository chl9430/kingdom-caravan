using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform graphicsRoot;

    private MyPlayerController playerMovement;

    private void Awake()
    {
        playerMovement = GetComponentInParent<MyPlayerController>();
    }

    private void Update()
    {
        UpdateMovementAnimation();
        UpdateFlip();
    }

    private void UpdateMovementAnimation()
    {
        if (animator == null || playerMovement == null)
            return;

        Vector2 moveInput = playerMovement.MoveInput;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        animator.SetBool("1_Move", isMoving);
    }

    private void UpdateFlip()
    {
        if (playerMovement == null || graphicsRoot == null)
            return;

        float moveX = playerMovement.MoveInput.x;

        Vector3 scale = graphicsRoot.localScale;

        // SPUM 기준 반전
        if (moveX > 0.01f)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else if (moveX < -0.01f)
        {
            scale.x = Mathf.Abs(scale.x);
        }

        graphicsRoot.localScale = scale;
    }

    public void PlayAttack()
    {
        animator.SetTrigger("2_Attack");
    }
}
