using System.Collections;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public SpawnPoint[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public float timeBetweenWaves = 5f;
    public int increasePerWave = 2;
    private bool wavesStarted = false;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !wavesStarted)
        {
            StartCoroutine(StartEnemyWaves());
            wavesStarted = true;
        }
    }

    private IEnumerator StartEnemyWaves()
    {
        int waveNumber = 1;

        while (true)
        {
            int enemiesToSpawn = waveNumber * increasePerWave;
            int remainingEnemies = SpawnEnemies(enemiesToSpawn);

            // Wait for the specified time between waves
            yield return new WaitForSeconds(timeBetweenWaves);

            // Wait until all enemies from the previous wave are destroyed
            while (remainingEnemies > 0)
            {
                remainingEnemies = CheckRemainingEnemies();
                yield return null;
            }

            waveNumber++;
        }
    }

    private int CheckRemainingEnemies()
    {
        // Check the number of enemies alive using the GameManager
        int remainingEnemies = gameManager.EnemiesLeft.Count;
        return remainingEnemies;
    }

    private int SpawnEnemies(int count)
    {
        int spawnedEnemies = 0;

        for (int i = 0; i < count; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            SpawnPoint spawnPointLocation = GetRandomSpawnLocation(spawnPoints);
            Vector3 randomSpawnPoint = GetRandomSpawnPoint(spawnPointLocation);

            GameObject spawnedEnemy = Instantiate(enemyPrefab, randomSpawnPoint, Quaternion.identity);

            if (gameManager != null)
            {
                gameManager.AddEnemyToList(spawnedEnemy.GetComponent<BaseAI>());
            }

            spawnedEnemies++;
        }

        return spawnedEnemies;
    }

    SpawnPoint GetRandomSpawnLocation(SpawnPoint[] spawnPoints)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points available.");
            return null;
        }
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    Vector3 GetRandomSpawnPoint(SpawnPoint spawnPoint)
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(-spawnPoint.spawnSize.x / 2f, spawnPoint.spawnSize.x / 2f),
            1f,
            Random.Range(-spawnPoint.spawnSize.z / 2f, spawnPoint.spawnSize.z / 2f)
        );

        return spawnPoint.spawnTransform.position + randomPoint;
    }

}

[System.Serializable]
public class SpawnPoint
{
    public Transform spawnTransform;
    public Vector3 spawnSize = new Vector3(15f, 1f, 5f); // Adjust the size of the cube
}
