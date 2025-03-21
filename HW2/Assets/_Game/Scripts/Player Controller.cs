using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private GameObject prefab;
    [SerializeField] private Door door;
    [SerializeField] private Material lavaKeyMaterial;
    [SerializeField] private Material iceKeyMaterial;
    [SerializeField] private Material invisibleKeyMaterial;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private List<Message> messages;
    private bool bCheckpointAvailable = false;
    private Transform checkpointTransform;
    private GameObject checkpointObject;

    private float maxSpeed = 3f;
    private Health health;

    private bool bGameEnded = false;

    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        maxSpeed = maxWalkingSpeed;
        health = GetComponent<Health>();
        CooldownController.UpdateUI();
        winUI.SetActive(false);
        loseUI.SetActive(false);
        replayButton.SetActive(false);
        quitButton.SetActive(false);
    }

    void FixedUpdate()
    {
        if (bGameEnded) return;

        ApplyMovement();
    }

    void Update()
    {
        if (bGameEnded) return;

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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadCheckpoint();
        }
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

    private void DropCheckpoint()
    {
        Quaternion rotatedY = transform.rotation * Quaternion.Euler(0, 180, 0);

        checkpointObject = Instantiate(prefab, transform.position, rotatedY);
        bCheckpointAvailable = true;

        checkpointTransform = new GameObject("CheckpointTransform").transform;
        checkpointTransform.position = transform.position;
        checkpointTransform.rotation = rotatedY;

        StaticVariables.DroppedCheckpoint();
    }

    public void Win()
    {
        GameObject door = GameObject.Find("Final_Door");
        StartCoroutine(LowerDoor(door));
    }

    private void Won()
    {
        StaticVariables.ResetVariables();
        PickupInventory.ResetVariables();
        foreach (Message msg in messages)
        {
            msg.DeleteMessage();
        }
        bGameEnded = true;

        winUI.SetActive(true);
        replayButton.SetActive(true);
        quitButton.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    public void Lost()
    {
        StaticVariables.ResetVariables();
        PickupInventory.ResetVariables();
        foreach (Message msg in messages)
        {
            msg.DeleteMessage();
        }
        bGameEnded = true;

        loseUI.SetActive(true);
        replayButton.SetActive(true);
        quitButton.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }

    IEnumerator LowerDoor(GameObject door)
    {
        while (door.transform.position.y > -2.5f)
        {
            door.transform.position = new Vector3(0f, door.transform.position.y - Time.deltaTime * 2.5f, 0f);
            yield return null;
        }
        Won();
    }

    public void ChangeKeyMaterial(string objectPath, string source)
    {
        GameObject keyslot = GameObject.Find(objectPath);

        if (keyslot != null && keyslot.TryGetComponent(out MeshRenderer renderer))
        {
            Material newMaterial = null;
            switch (source)
            {
                case "lava":
                    newMaterial = lavaKeyMaterial;
                    break;

                case "ice":
                    newMaterial = iceKeyMaterial;
                    break;

                case "invisible":
                    newMaterial = invisibleKeyMaterial;
                    break;

                default:
                    break;
            }

            renderer.material = newMaterial;
        }
        else
        {
            Debug.LogError($"Key object not found: {objectPath}");
        }
    }

    private void LoadCheckpoint()
    {
        if (!bCheckpointAvailable) return;
        transform.position = checkpointTransform.position;
        transform.rotation = checkpointTransform.rotation;
        bCheckpointAvailable = false;
        Destroy(checkpointObject);

        if (StaticVariables.checkpoint_lava)
        {
            door.GoLava();
        }
        else if (StaticVariables.checkpoint_ice)
        {
            door.GoIce();
        }
        else
        {
            door.GoNormal();
        }
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

            case "Faster Seeing":
                CooldownController.FasterPickup();
                break;

            case "Invincible":
                StartCoroutine(health.InvincibilityPickup());
                break;

            case "Checkpoint":
                DropCheckpoint();
                break;
            
            default:
                break;
        }       
    }
}
