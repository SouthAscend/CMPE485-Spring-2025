using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredIce : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private float factor = 2.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.maxWalkingSpeed = factor * 3f;
            playerController.maxRunningSpeed = factor * 5f;
            playerController.moveForce = factor * 30f;
            playerController.rotationSpeed = factor * 500f;
        }
    }
}
