using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;

    public float spawnInterval = 3f;
    public int maxEnemies = 10;

    [Header("Spawn Area")]
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    [Header("Player")]
    public Transform player;

    [Header("Spawn Distance")]
    public float minSpawnDistance = 5f;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        int currentEnemyCount =
            FindObjectsOfType<EnemyHealth>().Length;

        if (currentEnemyCount >= maxEnemies)
            return;

        Vector2 spawnPosition = GetRandomSpawnPosition();

        Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 randomPosition;

        int safety = 0;

        do
        {
            float randomX = Random.Range(
                spawnAreaMin.x,
                spawnAreaMax.x
            );

            float randomY = Random.Range(
                spawnAreaMin.y,
                spawnAreaMax.y
            );

            randomPosition = new Vector2(randomX, randomY);

            safety++;

            // 무한 루프 방지
            if (safety > 50)
            {
                break;
            }

        } while (
            Vector2.Distance(
                randomPosition,
                player.position
            ) < minSpawnDistance
        );

        return randomPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 center =
            (spawnAreaMin + spawnAreaMax) / 2f;

        Vector2 size =
            spawnAreaMax - spawnAreaMin;

        Gizmos.DrawWireCube(center, size);
    }
}
