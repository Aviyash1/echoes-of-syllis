using UnityEngine;

/// <summary>
/// Automates the instantiation of pre-configured prefabs across a randomized 
/// spatial coordinate plain on specified time frequencies.
/// </summary>
public class Spawner : MonoBehaviour
{
    [Header("Asset Assignment")]
    [Tooltip("The master prefab layout template to be instantiated into the scene space.")]
    public GameObject itemPrefab;

    [Header("Timing Metrics")]
    [Tooltip("Time delay in seconds before the first spawn sequence fires.")]
    public float initialDelay = 1.0f;

    [Tooltip("Frequency in seconds at which new items are generated.")]
    public float spawnInterval = 2.0f;

    [Header("Spatial Boundaries")]
    [Tooltip("Maximum distance from center on the horizontal X axis an item can spawn.")]
    public float spawnRangeX = 8.0f;

    [Tooltip("Maximum distance from center on the depth Z axis an item can spawn.")]
    public float spawnRangeZ = 8.0f;

    [Tooltip("Fixed vertical elevation offset height where items are placed.")]
    public float fixedSpawnHeight = 1.0f;

    /// <summary>
    /// Start is called before the first frame update. Used here to establish recurring execution routines.
    /// </summary>
    private void Start()
    {
        // Establish a managed repeating time invoke structure to cycle our generation logic
        InvokeRepeating(nameof(SpawnRandomItem), initialDelay, spawnInterval);
    }

    /// <summary>
    /// Calculates a random valid physical position vector and instantiates our target prefab template.
    /// </summary>
    private void SpawnRandomItem()
    {
        // Guard clause: immediately abort spawning routines if the master state indicates Game Over
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            // Terminate the active invoke sequence permanently to preserve processor overhead
            CancelInvoke(nameof(SpawnRandomItem));
            return;
        }

        // Calculate a pseudo-random positional offset point bounded within user-defined constraint domains
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        // Assemble data structures into a definitive spatial 3D Vector coordinate point
        Vector3 spawnPosition = new Vector3(randomX, fixedSpawnHeight, randomZ);

        // Instantiate the assigned prefab copy into the world scene space with identity (zeroed) rotations
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }
}