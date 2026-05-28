using UnityEngine;

/// <summary>
/// Attached to pickup items. Detects physical overlap via trigger matrices,
/// allocates points to the GameManager, and handles self-destruction.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    [Header("Value Configuration")]
    [Tooltip("The raw score value awarded to the player upon successful pickup.")]
    public int pointValue = 10;

    /// <summary>
    /// Callback triggered automatically by Unity when another Collider enters this object's trigger zone.
    /// </summary>
    /// <param name="other">The external Collider component initiating the intersection.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Optimization check: Use CompareTag instead of .tag comparison to avoid heap allocations
        if (other.CompareTag("Player"))
        {
            // Verify that the GameManager singleton instance is active and reachable
            if (GameManager.Instance != null)
            {
                // Execute score accumulation routine on the master controller
                GameManager.Instance.AddScore(pointValue);
            }

            // Purge this instance from the active engine hierarchy memory to clean up the scene
            Destroy(gameObject);
        }
    }
}