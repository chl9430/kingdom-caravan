using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    public EnemySpawner enemySpawner;

    [Header("Wave Settings")]
    public int currentWave = 1;

    public float waveDuration = 20f;

    private float waveTimer;

    private void Start()
    {
        ApplyWaveSettings();
    }

    private void Update()
    {
        waveTimer += Time.deltaTime;

        if (waveTimer >= waveDuration)
        {
            waveTimer = 0f;

            NextWave();
        }
    }

    private void NextWave()
    {
        currentWave++;

        Debug.Log("Wave " + currentWave);

        ApplyWaveSettings();
    }

    private void ApplyWaveSettings()
    {
        // 웨이브 증가할수록 스폰 빨라짐
        //enemySpawner.spawnInterval =
        //    Mathf.Max(0.5f, 3f - currentWave * 0.2f);

        //// 최대 적 수 증가
        //enemySpawner.maxEnemies =
        //    5 + currentWave * 2;
        // 최대 적 수 증가
        enemySpawner.maxEnemies += 1;
    }
}
