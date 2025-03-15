using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform playerTransform;
    [SerializeField] private float iceFactor = 2.5f;
    [SerializeField] Lava lavaComponent;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IceDoor"))
        {
            if (playerTransform.position.z < -10.6)
            {
                playerController.maxWalkingSpeed = iceFactor * 3f;
                playerController.maxRunningSpeed = iceFactor * 5f;
                playerController.moveForce = iceFactor * 30f;
                playerController.rotationSpeed = iceFactor * 500f;
            }
            else
            {
                playerController.maxWalkingSpeed = 3f;
                playerController.maxRunningSpeed = 5f;
                playerController.moveForce = 30f;
                playerController.rotationSpeed = 500f;
            }
        }
        else if (other.CompareTag("LavaDoor"))
        {
            if (playerTransform.position.x < 10.6)
            {
                lavaComponent.setActive(true);
            }
            else
            {
                lavaComponent.setActive(false);
            }
        }
    }
}
