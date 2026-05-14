using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class MyPlayerController : CreatureController
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    public Vector2 LastMoveDir { get; private set; } = Vector2.down;

    public Transform attackPoint;
    public float attackRange = 0.6f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

    protected override void Init()
    {
        base.Init();

        rb = GetComponent<Rigidbody2D>();
    }

    protected override void UpdateController()
    {
        base.UpdateController();

        UpdateAttackPoint();
    }

    protected override void UpdateIdle()
    {
    }

    void UpdateAttackPoint()
    {
        attackPoint.localPosition = LastMoveDir * 0.6f;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveInput = moveInput.normalized;

        if (moveInput != Vector2.zero)
        {
            LastMoveDir = moveInput;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Attack();
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
