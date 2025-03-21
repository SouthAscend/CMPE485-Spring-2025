using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLavaRay : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Health health;

    bool bActive = false;

    void Update()
    {
        if (bActive && rb.velocity.magnitude > 0.15f && !health.bInvincible)
        {
            rb.mass = 10000f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bActive = false;
        }
    }
}
