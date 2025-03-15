using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLava : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float waitDuration = 0.5f;
    [SerializeField] private float rotationSpeed = 30f;

    private Transform trap1;
    private Transform trap2;
    private Transform trap1Mesh;
    private Transform trap2Mesh;
    private Vector3 startPos1;
    private Vector3 startPos2;

    void Start()
    {
        trap1 = transform.Find("Trap1");
        trap2 = transform.Find("Trap2");

        startPos1 = trap1.localPosition;
        startPos2 = trap2.localPosition;

        trap1Mesh = trap1.GetChild(0);
        trap2Mesh = trap2.GetChild(0);

        StartCoroutine(MoveTrapCoroutine(trap1, startPos1));
        StartCoroutine(MoveTrapCoroutine(trap2, startPos2));

        StartCoroutine(RotateTrapCoroutine(trap1Mesh));
        StartCoroutine(RotateTrapCoroutine(trap2Mesh));
    }

    IEnumerator MoveTrapCoroutine(Transform trap, Vector3 startPos)
    {
        Vector3 endPos = new Vector3(startPos.x, startPos.y, -startPos.z);

        while (true)
        {
            yield return StartCoroutine(MoveOverTime(trap, endPos));
            yield return new WaitForSeconds(waitDuration);
            yield return StartCoroutine(MoveOverTime(trap, startPos));
            yield return new WaitForSeconds(waitDuration);
        }
    }

    IEnumerator MoveOverTime(Transform obj, Vector3 targetPos)
    {
        Vector3 start = obj.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            obj.localPosition = Vector3.Lerp(start, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.localPosition = targetPos;
    }

    IEnumerator RotateTrapCoroutine(Transform trapMesh)
    {
        while (true)
        {
            trapMesh.localRotation = Quaternion.Euler(
                trapMesh.localRotation.eulerAngles.x,
                (trapMesh.localRotation.eulerAngles.y + (rotationSpeed * Time.deltaTime)) % 360,
                trapMesh.localRotation.eulerAngles.z
            );

            yield return null;
        }
    }
}
