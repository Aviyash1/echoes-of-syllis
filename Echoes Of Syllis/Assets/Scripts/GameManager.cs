using UnityEngine;

/// <summary>
/// Manages the global gameplay state, tracks player performance metrics, 
/// and coordinates game-over transitions. Implements a Singleton pattern.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static globally accessible instance handle for the Singleton architecture.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Gameplay Metrics")]
    [Tooltip("The player's current cumulative score total.")]
    public int currentScore = 0;

    [Tooltip("Tracks whether the active gameplay loop is halted or running.")]
    public bool isGameOver = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Used here to initialize and enforce the Singleton pattern safeguard.
    /// </summary>
    private void Awake()
    {
        // Check if an instance of GameManager already exists in the scene context
        if (Instance == null)
        {
            // If none exists, this object becomes the definitive master instance
            Instance = this;
        }
        else
        {
            // Safeguard: If a duplicate manager attempts to spawn, destroy it instantly
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Safely increments the score metric and alerts dependent subsystems.
    /// </summary>
    /// <param name="amount">The integer value added to the score total.</param>
    public void AddScore(int amount)
    {
        // Enforce a condition guard rule: do not modify scores if the game state is over
        if (isGameOver) return;

        // Accumulate points into the tracking variable
        currentScore += amount;

        // Output state changes to the diagnostic console for runtime verification
        Debug.Log($"Score successfully updated. New Total: {currentScore}");
    }

    /// <summary>
    /// Halts active gameplay execution rules and flags the session as finished.
    /// </summary>
    public void TriggerGameOver()
    {
        // Flip the state flag to true, halting spawners and movement conditions
        isGameOver = true;

        Debug.Log("Game Over condition initiated. Preparing scene handoffs...");

        // Note: SceneManager.LoadScene("SceneTopScores") will be executed here 
        // once our canvas UI transition methods are structurally linked.
    }
}