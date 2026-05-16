using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class MyPlayerController : CreatureController
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    private Vector2 moveInput;

    public Vector2 LastMoveDir { get; private set; } = Vector2.down;

    public Transform attackPoint;
    public float attackRange = 0.6f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

    [Header("Health Settings")]
    public int maxHealth = 5;

    [Header("Invincibility")]
    public float invincibilityDuration = 0.7f;

    private int currentHealth;

    public bool IsDead { get; private set; }
    private bool isInvincible;

    private SpriteRenderer spriteRenderer;

    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        Managers.Object.MyPlayer = this;
    }

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        IsDead = false;
        isInvincible = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작 시 UI 초기화
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
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

    public void TakeDamage(int damage)
    {
        if (IsDead || isInvincible) return;

        currentHealth -= damage;

        // UI 갱신
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        if (spriteRenderer != null)
        {
            for (int i = 0; i < 3; i++)
            {
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(0.1f);

                spriteRenderer.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }

    private void Die()
    {
        IsDead = true;

        gameObject.SetActive(false);

        if (Managers.UI.SceneUI != null)
        {
            ((UI_GameScene)Managers.UI.SceneUI).GameOver();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
