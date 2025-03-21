using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPath : MonoBehaviour
{
    [SerializeField] private float stepDistance = 12f;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float rotationDuration = 0.5f;

    void Start()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Move forward
            yield return StartCoroutine(MoveForward());

            // Rotate 90 degrees to the right
            yield return StartCoroutine(RotateRight());
        }
    }

    IEnumerator MoveForward()
    {
        Vector3 direction = transform.forward;
        direction.y = 0; // Ensure no vertical movement
        direction.Normalize(); // Normalize to keep the step length consistent

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + (direction * stepDistance); // Move exactly 12 units in world space

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }

    IEnumerator RotateRight()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}