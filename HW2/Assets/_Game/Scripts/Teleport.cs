using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float boundary = 40f;
    [SerializeField] private float fallDamage = 40f;

    private Rigidbody rb;
    private Health health;
    private List<MeshRenderer> breadCrumbs;

    public bool bTeleported = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        TrackBreadcrumbs();
    }

    void Update()
    {
        if (transform.position.y < -boundary)
        {
            TeleportPlayer();
        }
    }

    public void TeleportPlayer(bool bCaught = false)
    {
        transform.position = respawnPoint.position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        health.Damage(fallDamage);
        if (bCaught)
        {
            DisableBreadcrumbs();
            StaticVariables.bCaught = true;
        }
    }

    void DisableBreadcrumbs()
    {
        foreach (MeshRenderer breadCrumb in breadCrumbs)
        {
            breadCrumb.enabled = false;
        }
    }
    void TrackBreadcrumbs()
    {
        breadCrumbs = new List<MeshRenderer>();

        // Find all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Invisible"))
            {
                foreach (Transform child in obj.transform)
                {
                    if (child.name.Contains("Breadcrumb"))
                    {
                        MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                        if (renderer != null)
                        {
                            breadCrumbs.Add(renderer);
                        }
                    }
                }
            }
        }
    }
}
