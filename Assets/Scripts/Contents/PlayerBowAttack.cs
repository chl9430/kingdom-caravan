using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBowAttack : MonoBehaviour
{
    [Header("Bow Settings")]
    public GameObject arrowPrefab;
    public Transform firePoint;

    public float arrowSpeed = 10f;
    public int attackDamage = 1;

    public float attackCooldown = 0.55f;

    private MyPlayerController playerMovement;

    private float lastAttackTime;

    private void Awake()
    {
        playerMovement = GetComponent<MyPlayerController>();
    }

    private void Update()
    {
        UpdateFirePoint();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        FireArrow();
    }

    private void UpdateFirePoint()
    {
        if (firePoint == null || playerMovement == null) return;

        Vector2 dir = playerMovement.LastMoveDir;

        firePoint.localPosition = dir * 0.6f;
    }

    private void FireArrow()
    {
        if (arrowPrefab == null || firePoint == null) return;

        Vector2 shootDirection = playerMovement.LastMoveDir;

        GameObject arrow = Instantiate(
            arrowPrefab,
            firePoint.position,
            Quaternion.identity
        );

        ArrowProjectile projectile = arrow.GetComponent<ArrowProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(
                shootDirection,
                arrowSpeed,
                attackDamage
            );
        }

        Debug.Log("Arrow Fired");
    }
}
