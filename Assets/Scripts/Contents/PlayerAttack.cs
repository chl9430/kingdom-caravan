using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.6f;
    public int attackDamage = 1;
    public float attackCooldown = 0.35f;
    public LayerMask enemyLayer;

    private MyPlayerController playerMovement;

    private float lastAttackTime;

    private void Awake()
    {
        playerMovement = GetComponent<MyPlayerController>();
    }

    private void Update()
    {
        UpdateAttackPoint();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // 쿨다운 체크
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Attack();
    }

    private void UpdateAttackPoint()
    {
        if (attackPoint == null || playerMovement == null) return;

        Vector2 dir = playerMovement.LastMoveDir;
        attackPoint.localPosition = dir * 0.6f;
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(
                    attackDamage,
                    transform.position
                );
            }
        }

        Debug.Log("Player Attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
