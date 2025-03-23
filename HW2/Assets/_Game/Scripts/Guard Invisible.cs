using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardInvisible : MonoBehaviour
{
    private bool bActive = false;
    private Transform playerTransform;
    private Vector3 guardForward;
    [SerializeField] private Teleport tp;
    private Health health;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        health = playerTransform.GetComponent<Health>();

        guardForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
    }

    void Update()
    {
        if (bActive && !health.bInvincible)
        {
            Vector3 playerForward = new Vector3(playerTransform.forward.x, 0f, playerTransform.forward.z).normalized;

            float angle = Vector3.Angle(playerForward, guardForward);

            if (angle < 135f)
            {
                tp.TeleportPlayer(true);
                bActive = false;
            }
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
        }
    }
}
