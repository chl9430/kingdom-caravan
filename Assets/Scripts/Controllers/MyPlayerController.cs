using System;
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
    }

    protected override void UpdateIdle()
    {
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
}
