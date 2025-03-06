using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private bool bActive = false;
    private Health health;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        LavaDamage();
    }

    void LavaDamage()
    {
        if (bActive)
        {
            playerController.OnLava();
        }
    }

    public void setActive(bool bActive)
    {
        this.bActive = bActive;
    }
}
