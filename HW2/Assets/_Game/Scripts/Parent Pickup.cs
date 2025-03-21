using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPickup
{
    public string pickupType = "";
    public int amount = 1;

    public ParentPickup(string pickupType, int amount = 1)
    {
        this.pickupType = pickupType;
        this.amount = amount;
    }
}
