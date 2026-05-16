using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public UI_GameOver GameOverUI { get; private set; }
    public UI_PlayerHealth PlayerHealthUI { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        GameOverUI = GetComponentInChildren<UI_GameOver>();
        PlayerHealthUI = GetComponentInChildren<UI_PlayerHealth>();

        GameOverUI.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        if (GameOverUI != null)
        {
            GameOverUI.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
