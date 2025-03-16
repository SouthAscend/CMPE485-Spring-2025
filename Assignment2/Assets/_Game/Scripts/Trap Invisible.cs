using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInvisible : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Teleport tp;

    private MeshRenderer meshRenderer;
    private bool bActive = false;
    private float radius = 1.5f;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        radius = sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        SetVisibility(0f); // Start invisible
    }

    void Update()
    {
        if (bActive && playerTransform != null)
        {
            Vector3 objectPos = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 playerPos = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);

            float distance = Vector3.Distance(objectPos, playerPos);
            float visibilityRate = Mathf.Clamp01((radius - distance - 0.15f) / (radius - 0.15f));

            if (distance < 0.15f)
            {
                visibilityRate = 0f;
                bActive = false;
                Destroy(gameObject);
                tp.TeleportPlayer(true);
                return;
            }

            SetVisibility(visibilityRate);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bActive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bActive = false;
            SetVisibility(0f); // Reset to invisible
        }
    }

    private void SetVisibility(float alpha)
    {
        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = alpha;
            meshRenderer.material.color = color;
        }
    }
}