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

}
