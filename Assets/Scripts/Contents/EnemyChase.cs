using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float moveSpeed = 3f;
    public float detectionRange = 6f;
    public float stopDistance = 1.2f;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 감지 범위 밖이면 멈춤
        if (distance > detectionRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 너무 가까우면 멈춤
        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 추적
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    // 감지 범위 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
