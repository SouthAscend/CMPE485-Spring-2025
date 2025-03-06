using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitedIce : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.maxWalkingSpeed = 3f;
            playerController.maxRunningSpeed = 5f;
            playerController.moveForce = 30f;
            playerController.rotationSpeed = 500f;
        }
    }
}
