using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform playerTransform;
    [SerializeField] private float iceFactor = 2.5f;
    [SerializeField] Lava lavaComponent;
    [SerializeField] Rigidbody rb;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("IceDoor"))
        {
            if (playerTransform.position.z < -10.6)
            {
                GoIce();
            }
            else
            {
                GoNormal();
            }
        }
        else if (other.CompareTag("LavaDoor"))
        {
            if (playerTransform.position.x < -10.6)
            {
                GoLava();
            }
            else
            {
                GoNormal();
            }
        }
    }

    public void GoIce()
    {
        playerController.maxWalkingSpeed = iceFactor * 3f;
        playerController.maxRunningSpeed = iceFactor * 5f;
        playerController.moveForce = iceFactor * 30f;
        playerController.rotationSpeed = iceFactor * 500f;
        rb.mass = 10f;
        StaticVariables.player_ice = true;
        StaticVariables.player_normal = false;
        StaticVariables.player_lava = false;
    }

    public void GoNormal()
    {
        playerController.maxWalkingSpeed = 3f;
        playerController.maxRunningSpeed = 5f;
        playerController.moveForce = 30f;
        playerController.rotationSpeed = 500f;
        rb.mass = 1f;
        StaticVariables.player_normal = true;
        StaticVariables.player_ice = false;
        StaticVariables.player_lava = false;
        lavaComponent.setActive(false);
    }

    public void GoLava()
    {
        lavaComponent.setActive(true);
        StaticVariables.player_lava = true;
        StaticVariables.player_normal = false;
        StaticVariables.player_ice = false;
        rb.mass = 1f;
    }
}
