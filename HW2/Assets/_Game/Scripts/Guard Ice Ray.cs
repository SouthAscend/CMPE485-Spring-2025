using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardIceRay : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Health health;

    bool bActive = false;

    void Update()
    {
        if (bActive)
        {
            health.Damage(Time.deltaTime * 10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !health.bInvincible)
        {
            bActive = true;
            rb.mass = 1f;
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
