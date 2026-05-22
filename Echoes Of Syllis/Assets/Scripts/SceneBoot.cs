using UnityEngine;
using System.Collections;

/// <summary>
/// Handles scene-entry lifecycle actions, providing isolated fade configurations 
/// to ensure structural modularity across multiple distinct scenes.
/// </summary>
public class SceneBoot : MonoBehaviour
{
    [Header("Local Scene Transitions")]
    [Tooltip("The blocking graphic overlay specific to this scene hierarchy layer.")]
    public CanvasGroup fadeCanvasGroup;

    [Tooltip("Execution speed window for the entrance fade sequence.")]
    public float fadeDuration = 0.2f;

    /// <summary>
    /// Requirement: Lifecycle Initialisation Hook.
    /// Runs instantly as the object is initialized inside memory arrays upon loading.
    /// </summary>
    private void Start()
    {
        if (fadeCanvasGroup)
        {
            // Requirement: Defensive Programming. Rigidly forces opacity to full black 
            // to eliminate accidental background graphic popping on initial execution frames.
            fadeCanvasGroup.alpha = 1f;
            StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// Requirement: Inverse Linear Interpolation Process.
    /// Smoothly peels away the dark mask to introduce the newly processed UI setup elements.
    /// </summary>
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null; // Suspends execution cycle until the following rendering step
        }

        fadeCanvasGroup.alpha = 0f; // Assures full transparency is achieved cleanly
    }
}