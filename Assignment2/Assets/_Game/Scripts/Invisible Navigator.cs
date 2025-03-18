using System.Collections;
using UnityEngine;

public class InvisibleNavigator : MonoBehaviour
{
    private MeshRenderer targetMesh;
    private Coroutine fadeCoroutine;
    private int collided = 0;
    private bool isFading = false;

    void Start()
    {
        Transform parent = transform.parent;

        string targetMeshName = gameObject.name + "Mesh";
        Transform targetTransform = parent.Find(targetMeshName);

        targetMesh = targetTransform.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (collided > 0 && CooldownController.currentCD > 0.01f)
        {
            if (CooldownController.navigator_active) CooldownController.Drain();
            if (CooldownController.navigator_active && !isFading)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeVisibility(1f, 0.1f)); // Fade In
                isFading = true;
            }
            else if (!CooldownController.navigator_active && isFading)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeVisibility(0f, 0.1f)); // Fade Out
                isFading = false;
            }
        }
        else if (CooldownController.currentCD <= 0.01f && isFading)
        {
            NoMesh();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvisibleFloor") && targetMesh != null)
        {
            collided++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InvisibleFloor") && targetMesh != null)
        {
            collided--;
            if (collided == 0)
            {
                NoMesh();
            }
        }
    }

    void NoMesh()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeVisibility(0f, 0.1f)); // Fade Out
        isFading = false;
    }

    IEnumerator FadeVisibility(float endAlpha, float duration)
    {
        float startAlpha = GetCurrentAlpha();
        float elapsed = duration * (1 - Mathf.Abs(endAlpha - startAlpha));

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetAlpha(alpha);
            elapsed += Time.deltaTime / Mathf.Abs(endAlpha - startAlpha);
            yield return null;
        }

        SetAlpha(endAlpha);
    }

    float GetCurrentAlpha()
    {
        if (targetMesh != null && targetMesh.material.HasProperty("_Color"))
            return targetMesh.material.color.a;
        return 1f; // Default fully visible
    }

    void SetAlpha(float alpha)
    {
        if (targetMesh == null) return;
        Color color = targetMesh.material.color;
        color.a = alpha;
        targetMesh.material.color = color;
    }
}
