using System.Collections;
using UnityEngine;

public class InvisibleNavigator : MonoBehaviour
{
    private MeshRenderer targetMesh;
    private Coroutine fadeCoroutine;
    private int collided = 0;

    void Start()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            Debug.LogError("InvisibleNavigator: No parent found!");
            return;
        }

        string targetMeshName = gameObject.name + "Mesh";
        Transform targetTransform = parent.Find(targetMeshName);

        if (targetTransform != null)
            targetMesh = targetTransform.GetComponent<MeshRenderer>();
        else
            Debug.LogError($"InvisibleNavigator: No matching mesh found for {gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvisibleFloor") && targetMesh != null)
        {
            collided++;

            if (collided == 1)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeVisibility(1f, 0.1f)); // Fade In
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InvisibleFloor") && targetMesh != null)
        {
            collided--;
            if (collided == 0)
            {
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeVisibility(0f, 0.1f)); // Fade Out
            }
        }
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
