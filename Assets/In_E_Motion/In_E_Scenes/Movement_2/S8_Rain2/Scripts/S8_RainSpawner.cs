using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    public GameObject rainPrefab; // The prefab to spawn
    public float spawnRate = 0.1f; // Time interval between spawns
    public Vector3 spawnArea = new Vector3(5, 1, 5); // Spawn area size relative to the cloud

    void Start()
    {
        // Start spawning objects repeatedly
        InvokeRepeating(nameof(SpawnRainObject), 0f, spawnRate);
    }

    void SpawnRainObject()
    {
        // Random position below the cloud within the spawn area
        Vector3 spawnPosition = new Vector3(
            transform.position.x + Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            transform.position.y - spawnArea.y,
            transform.position.z + Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );

        Instantiate(rainPrefab, spawnPosition, Quaternion.identity);
    }
}
