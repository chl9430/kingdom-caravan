using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [Header("Arrow Settings")]
    public int damage = 1;
    public float lifetime = 3f;

    private Vector2 moveDirection;
    private float speed;

    public void Initialize(Vector2 direction, float arrowSpeed, int attackDamage)
    {
        moveDirection = direction.normalized;
        speed = arrowSpeed;
        damage = attackDamage;

        Destroy(gameObject, lifetime);

        // 방향 회전 (스프라이트 방향 맞춤)
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 적 맞음
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage, transform.position);

            Destroy(gameObject);
            return;
        }

        // 벽 맞으면 제거
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
