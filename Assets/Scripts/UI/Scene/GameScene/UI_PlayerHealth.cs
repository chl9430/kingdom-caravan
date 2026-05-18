using TMPro;
using UnityEngine;

public class UI_PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI hpText;

    private MyPlayerController playerHealth;

    private void Awake()
    {
        hpText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        playerHealth = Managers.Object.MyPlayer;
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (currentHealth < 0)
            currentHealth = 0;

        hpText.text = "HP: " + currentHealth + " / " + maxHealth;
    }
}
