using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class PickupInventory
{
    static public List<ParentPickup> pickups = new List<ParentPickup>();

    public static void NewPickup(string pickupType)
    {
        int index = -1;
        for (int i = 0; i < pickups.Count; i++)
        {
            if (pickups[i].pickupType.Equals(pickupType))
            {
                index = i; 
                break;
            }
        }
        if (index != -1)
        {
            pickups[index].amount++;
            GameObject countObj = GameObject.Find($"Canvas/Image {index + 1}/Count");
            Text textComponent = countObj.GetComponent<Text>();
            textComponent.text = pickups[index].amount + "";
        }
        else
        {
            pickups.Add(new ParentPickup(pickupType));
            PickupInventory.UpdateUI(1, pickupType);
            GameObject countObj = GameObject.Find($"Canvas/Image 1/Count");
            Text textComponent = countObj.GetComponent<Text>();
            textComponent.text = "1";
        }
    }

    public static void UpdateUI(int slot, string pickupType)
    {
        GameObject imageObj = GameObject.Find($"Canvas/Image {slot}");

        if (imageObj == null)
        {
            Debug.LogError($"UpdateUI: No UI element found for slot {slot}");
            return;
        }

        Image imageComponent = imageObj.GetComponent<Image>();
        if (imageComponent == null)
        {
            Debug.LogError($"UpdateUI: No Image component found on {imageObj.name}");
            return;
        }

        Sprite newSprite = GetSprite(pickupType);
        if (newSprite != null)
        {
            imageComponent.sprite = newSprite;
        }
    }

    private static Sprite GetSprite(string pickupType)
    {
        string path = $"Assets/_Game/UI/Pickup Frame/{pickupType}.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

        if (sprite == null)
        {
            Debug.LogError($"GetSprite: No image found at {path}");
        }

        return sprite;
    }

    public static ParentPickup GetPickup(int slot)
    {
        if (slot > pickups.Count) return null;
        ParentPickup thePickup = pickups[slot-1];
        thePickup.amount = Mathf.Max(0, thePickup.amount - 1);

        GameObject widget = GameObject.Find($"Canvas/Image {slot}");
        Image imageComp = widget.GetComponent<Image>(); 
        GameObject count = GameObject.Find($"Canvas/Image {slot}/Count");
        Text text = count.GetComponent<Text>();
        
        if (thePickup.amount == 0)
        {
            text.text = "0";
            imageComp.sprite = GetSprite("Empty Pickup");
            pickups.RemoveAt(slot-1);
            UpdateProgress();
        }
        else
        {
            text.text = thePickup.amount + "";
        }

            return thePickup;
    }

    public static void UpdateProgress()
    {
        int totalSlots = 7; // Assuming 8 pickup slots

        // Update UI for existing pickups
        for (int i = 0; i < pickups.Count; i++)
        {
            GameObject imageObj = GameObject.Find($"Canvas/Image {i + 1}");
            GameObject countObj = GameObject.Find($"Canvas/Image {i + 1}/Count");

            if (imageObj != null && countObj != null)
            {
                Image imageComponent = imageObj.GetComponent<Image>();
                Text textComponent = countObj.GetComponent<Text>();

                if (imageComponent != null)
                    imageComponent.sprite = GetSprite(pickups[i].pickupType);

                if (textComponent != null)
                    textComponent.text = pickups[i].amount.ToString();
            }
        }

        // Clear remaining slots
        for (int i = pickups.Count; i < totalSlots; i++)
        {
            GameObject imageObj = GameObject.Find($"Canvas/Image {i + 1}");
            GameObject countObj = GameObject.Find($"Canvas/Image {i + 1}/Count");

            if (imageObj != null && countObj != null)
            {
                Image imageComponent = imageObj.GetComponent<Image>();
                Text textComponent = countObj.GetComponent<Text>();

                if (imageComponent != null)
                    imageComponent.sprite = GetSprite("Empty Pickup");

                if (textComponent != null)
                    textComponent.text = "";
            }
        }
    }


}
