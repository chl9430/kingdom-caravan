using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
    }

    // Animation Event에서 호출
    public void PerformAttackHit()
    {
        if (playerAttack != null)
        {
            playerAttack.PerformAttackHit();
        }
    }
}
