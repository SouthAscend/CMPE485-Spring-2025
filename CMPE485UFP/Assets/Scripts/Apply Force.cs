using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    [SerializeField] private Rigidbody rb; // Assign via GUI
    [SerializeField] private Vector3 forceDirection = new Vector3(0, 1, 0); // Default: Upward
    [SerializeField] private float forceAmount = 10f; // Adjustable in Inspector

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.AddForce(forceDirection.normalized * forceAmount, ForceMode.Force);
        }
    }
}
