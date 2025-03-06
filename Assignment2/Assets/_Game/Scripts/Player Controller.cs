using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] public float moveForce = 30f;
    [SerializeField] public float rotationSpeed = 500f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] public float maxWalkingSpeed = 3f;
    [SerializeField] public float maxRunningSpeed = 5f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float lavaDamage = 1f;
    [SerializeField] private float lavaSpeedFactor = 0.5f;

    private float maxSpeed = 3f;
    private Health health;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        maxSpeed = maxWalkingSpeed;
        health = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void Update()
    {
        ApplyRotation();
        UpdateMaxSpeed();
    }

    void ApplyMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        float moveY = 0f;

        Vector3 moveDirection = (playerTransform.forward * moveZ) + (playerTransform.right * moveX) + (playerTransform.up * moveY);
        moveDirection.Normalize();

        rb.AddForce(moveDirection * moveForce, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }


    void ApplyRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.deltaTime * 1.5f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.deltaTime;

        playerTransform.Rotate(Vector3.up * mouseX);

        float currentXRotation = cameraTransform.localEulerAngles.x;
        currentXRotation = (currentXRotation > 180) ? currentXRotation - 360 : currentXRotation;
        float newXRotation = Mathf.Clamp(currentXRotation - mouseY, -20f, 30f);

        cameraTransform.localRotation = Quaternion.Euler(newXRotation, 0f, 0f);
    }

    void UpdateMaxSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            maxSpeed = maxRunningSpeed;
        else
            maxSpeed = maxWalkingSpeed;
    }

    public void OnLava()
    {
        health.Damage(lavaDamage * Time.deltaTime * (maxRunningSpeed + lavaSpeedFactor - rb.velocity.magnitude));
    }
}
