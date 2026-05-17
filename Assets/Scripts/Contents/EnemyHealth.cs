using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;

    [Header("Knockback")]
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.15f;

    [Header("Hit Flash")]
    public Color hitFlashColor = Color.red;
    public float flashDuration = 0.08f;

    private int currentHealth;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public bool IsKnockedBack { get; private set; }
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isDead) return;

        currentHealth -= damage;

        ApplyKnockback(attackerPosition);

        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            StartCoroutine(DieCoroutine());
        }
    }

    private void ApplyKnockback(Vector2 attackerPosition)
    {
        if (rb == null) return;

        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(KnockbackCoroutine());
    }

    private IEnumerator KnockbackCoroutine()
    {
        IsKnockedBack = true;

        yield return new WaitForSeconds(knockbackDuration);

        IsKnockedBack = false;
    }

    private IEnumerator HitFlash()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = hitFlashColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }

    private IEnumerator DieCoroutine()
    {
        isDead = true;

        // 추적/공격 중단
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Rigidbody 정지
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // 죽는 색상 변화 (회색톤)
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
        }

        // 짧은 사망 딜레이
        yield return new WaitForSeconds(0.15f);

        // 간단한 페이드 아웃
        if (spriteRenderer != null)
        {
            Color fadeColor = spriteRenderer.color;

            float fadeTime = 0.25f;
            float timer = 0f;

            while (timer < fadeTime)
            {
                timer += Time.deltaTime;

                float alpha = Mathf.Lerp(1f, 0f, timer / fadeTime);

                spriteRenderer.color = new Color(
                    fadeColor.r,
                    fadeColor.g,
                    fadeColor.b,
                    alpha
                );

                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
