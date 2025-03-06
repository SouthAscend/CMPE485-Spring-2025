using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float boundary = 40f;
    [SerializeField] private float fallDamage = 40f;

    private Rigidbody rb;
    private Health health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (transform.position.y < -boundary)
        {
            TeleportPlayer();
        }
    }

    void TeleportPlayer()
    {
        transform.position = respawnPoint.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        health.Damage(fallDamage);
    }
}
