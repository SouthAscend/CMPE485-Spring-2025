using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private Transform playerTransform;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void Update()
    {
        ApplyRotation();
    }

    void ApplyMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A (-1) / D (1)
        float moveZ = Input.GetAxis("Vertical");   // W (1) / S (-1)
        float moveY = 0f;

        Vector3 moveDirection = (playerTransform.forward * moveZ) + (playerTransform.right * moveX) + (playerTransform.up * moveY);

        moveDirection.Normalize();

        rb.AddForce(moveDirection * moveForce, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void ApplyRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime * 1.5f;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        playerTransform.Rotate(Vector3.up * mouseX);

        float currentXRotation = cameraTransform.localEulerAngles.x;
        currentXRotation = (currentXRotation > 180) ? currentXRotation - 360 : currentXRotation;
        float newXRotation = Mathf.Clamp(currentXRotation - mouseY, -20f, 30f);

        cameraTransform.localRotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }
}
