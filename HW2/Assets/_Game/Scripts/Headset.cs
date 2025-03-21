using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headset : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    private AudioSource audioSource;
    private MeshRenderer[] meshRenderers;
    private Coroutine fadeCoroutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isPlaying = audioSource.isPlaying;
            if (isPlaying) audioSource.Pause();
            else audioSource.Play();

            float targetAlpha = isPlaying ? 0f : 1f;
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeVisibility(targetAlpha));
        }
    }

    IEnumerator FadeVisibility(float endAlpha)
    {
        float startAlpha = GetCurrentAlpha();
        float duration = fadeDuration;
        float progressMultiplier = Mathf.Abs(endAlpha - startAlpha); // Scale elapsed time
        float elapsed = duration * (1 - progressMultiplier); // Start from scaled progress

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetAlpha(alpha);
            elapsed += Time.deltaTime / progressMultiplier; // Adjust time scaling
            yield return null;
        }
    }

    float GetCurrentAlpha()
    {
        if (meshRenderers.Length > 0 && meshRenderers[0].materials.Length > 0)
        {
            return meshRenderers[0].materials[0].color.a;
        }
        return 1f; // Default to fully visible if something goes wrong
    }

    void SetAlpha(float alpha)
    {
        foreach (var meshRenderer in meshRenderers)
        {
            foreach (var mat in meshRenderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;
                }
            }
        }
    }
}
