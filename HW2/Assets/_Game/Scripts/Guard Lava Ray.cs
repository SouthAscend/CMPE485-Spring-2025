using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLavaRay : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    bool bActive = false;

    void Update()
    {
        if (bActive && rb.velocity.magnitude > 0.15f)
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
