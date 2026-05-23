using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerExperience playerExp =
            collision.GetComponent<PlayerExperience>();

        if (playerExp != null)
        {
            playerExp.AddExperience(expAmount);

            Destroy(gameObject);
        }
    }
}
