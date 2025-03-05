using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotControl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 2f;

    void LateUpdate()
    {
        transform.position = player.position;

        float yaw = Input.GetAxis("Mouse X") * rotationSpeed;
        float pitch = -Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.up, yaw, Space.World);
        transform.Rotate(Vector3.right, pitch, Space.Self);
    }
}
