using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInvisible : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private Teleport tp;

    private MeshRenderer meshRenderer;
    private bool bActive = false;
    private float radius = 1.5f;
    private Health health;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        meshRenderer = GetComponent<MeshRenderer>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        radius = sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        health = playerTransform.GetComponent<Health>();

        SetVisibility(0f); // Start invisible
    }

    void Update()
    {
        if (bActive)
        {
            Vector3 objectPos = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 playerPos = new Vector3(playerTransform.position.x, 0f, playerTransform.position.z);

            float distance = Vector3.Distance(objectPos, playerPos);
            float visibilityRate = Mathf.Clamp01((radius - distance - 0.3f) / (radius - 0.3f));

            if (distance < 0.3f && !health.bInvincible)
            {
                visibilityRate = 0f;
                bActive = false;
                tp.TeleportPlayer(true);
                Destroy(gameObject);
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