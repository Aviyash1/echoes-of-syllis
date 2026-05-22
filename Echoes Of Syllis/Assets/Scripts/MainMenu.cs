using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Core structural architecture for handling state-switching, native UI interaction,
/// and smooth cinematic transitions between gameplay and sub-menu contexts.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Audio Components")]
    [Tooltip("Central audio controller for dispatching sudden audio cues (SFX).")]
    public AudioSource sfxSource;

    [Tooltip("Triggered during UI element pointer-enter highlights.")]
    public AudioClip hoverSFX;

    [Tooltip("Triggered during explicit UI form submissions/clicks.")]
    public AudioClip clickSFX;

    [Header("Cinematic Transitions")]
    [Tooltip("Direct handle to the overlay blocking panel to manipulate visual transparency.")]
    public CanvasGroup fadeCanvasGroup;

    [Tooltip("The designated time horizon (in seconds) for executing interpolation operations.")]
    public float fadeDuration = 0.2f;

    /// <summary>
    /// Requirement: Accessibility and Audio Feedback.
    /// Intercepts UI PointerEvent data to trigger contextual audio clips.
    /// </summary>
    public void PlayHoverSound()
    {
        if (sfxSource && hoverSFX)
        {
            sfxSource.PlayOneShot(hoverSFX);
        }
    }

    /// <summary>
    /// Requirement: Scene Pipeline - Launch Primary Game Context.
    /// Initiates a routine to switch states into the core action phase.
    /// </summary>
    public void PlayGame()
    {
        StartCoroutine(FadeAndLoadScene("Level01"));
    }

    /// <summary>
    /// Requirement: Scene Pipeline - Route to Options Display.
    /// Switches viewport scope to the specialized close-up layout configurations.
    /// </summary>
    public void OpenOptions()
    {
        StartCoroutine(FadeAndLoadScene("SceneOptions"));
    }

    /// <summary>
    /// Requirement: Scene Pipeline - Route to Leaderboard.
    /// Transitions runtime control into the high-score parsing sub-module.
    /// </summary>
    public void OpenLeaderboard()
    {
        StartCoroutine(FadeAndLoadScene("SceneTopScores"));
    }

    /// <summary>
    /// Requirement: Bidirectional UI State Traversal.
    /// Invoked by sub-scene exit nodes to safely roll back layout trees to the home state.
    /// </summary>
    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeAndLoadScene("MainMenu"));
    }

    /// <summary>
    /// Requirement: Lifecycle Management - Clean Garbage and Exit.
    /// Shuts down the engine instance safely. (Ignored inside the Unity editor runtime).
    /// </summary>
    public void QuitGame()
    {
        if (sfxSource && clickSFX) sfxSource.PlayOneShot(clickSFX);

        Debug.Log("Application termination request processed safely.");
        Application.Quit();
    }

    /// <summary>
    /// Requirement: Asynchronous State Intercept and Core Pipeline Swapping.
    /// Prevents sudden frame drops or layout pop glitches by buffering the switch behind a solid mask.
    /// </summary>
    /// <param name="sceneName">The literal target identifier inside your project build list.</param>
    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        // 1. Audio Confirmation Trigger
        if (sfxSource && clickSFX) sfxSource.PlayOneShot(clickSFX);

        // 2. Linear Interpolation loop running for exactly 0.2 seconds
        yield return StartCoroutine(Fade(1f));

        // 3. Execution of scene context switch inside memory arrays
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Requirement: Interpolation Math and Coroutine Mechanics.
    /// Linearly shifts transparency targets frame-by-frame to keep screen rendering fluid.
    /// </summary>
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Evaluates frame time ratios to scale rendering outputs linearly
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null; // Returns execution context to the master loop engine
        }

        fadeCanvasGroup.alpha = targetAlpha; // Rigidly locks destination boundaries to resolve floating point drift
    }
}