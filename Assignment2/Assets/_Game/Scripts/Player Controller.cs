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
        CooldownController.UpdateUI();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void Update()
    {
        ApplyRotation();
        UpdateMaxSpeed();

        for (int i = 1; i <= 7; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + (i - 1)))
            {
                ApplyPickup(i);
            }
        }

        CooldownController.navigator_active = Input.GetKey(KeyCode.E);

        if (!CooldownController.navigator_active) CooldownController.Cooldown();
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
        float damage = lavaDamage * Time.deltaTime;
        damage += (damage * maxRunningSpeed) / Mathf.Max((lavaSpeedFactor * rb.velocity.magnitude), 0.2f);
        damage = Mathf.Min(7.5f * Time.deltaTime, damage);
        health.Damage(damage);
    }

    public void ApplyPickup(int slot)
    {
        ParentPickup pickup = PickupInventory.GetPickup(slot);

        if (pickup == null) return;

        switch (pickup.pickupType)
        {
            case "Empty Pickup":
                break;

            case "More HP":
                health.HealPickup();
                break;

            case "More Max HP":
                health.MaxHpPickup();
                break;

            case "Faster Regen":
                health.BoostRegeneration();
                break;

            case "More Seeing":
                CooldownController.MorePickup();
                break;

            default:
                break;
        }       
    }
}
