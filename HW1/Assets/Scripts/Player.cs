using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float verticalForce = 10f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxAngularSpeed = 10f;

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    void ApplyMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A (-1) / D (1)
        float moveZ = Input.GetAxis("Vertical");   // W (1) / S (-1)
        float moveY = 0f;

        if (Input.GetKey(KeyCode.E)) moveY = 1f; // Ascend
        if (Input.GetKey(KeyCode.Q)) moveY = -1f; // Descend

        Vector3 moveDirection = (cameraTransform.forward * moveZ) + (cameraTransform.right * moveX) + (cameraTransform.up * moveY);

        moveDirection.Normalize();

        rb.AddForce(moveDirection * moveForce, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void ApplyRotation()
    {
        if (Input.GetKey(KeyCode.C))
        {
            rb.AddTorque(Vector3.up * rotationSpeed, ForceMode.Force);
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxAngularSpeed);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            rb.AddTorque(Vector3.down * rotationSpeed, ForceMode.Force);
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, maxAngularSpeed);
        }
    }
}
