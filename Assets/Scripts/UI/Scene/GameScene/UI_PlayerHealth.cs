using TMPro;
using UnityEngine;

public class UI_PlayerHealth : MonoBehaviour
{
    public MyPlayerController playerHealth;
    public TextMeshProUGUI hpText;

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
        hpText.text = "HP: " + currentHealth + " / " + maxHealth;
    }
}
