using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CooldownController
{
    public static float maxCD = 100f;
    private static float cdDownFactor = 1f;
    private static float cdUpFactor = 6f;
    public static float currentCD = 100f;
    public static bool navigator_active = false;
    private static RectTransform fullBarRect;
    private static RectTransform cdBarRect;
    public static void Drain()
    {
        currentCD -= Time.deltaTime * cdDownFactor;
        currentCD = Mathf.Max(currentCD, 0);
        UpdateUI();
    }

    public static void Cooldown()
    {
        currentCD += Time.deltaTime * cdUpFactor;
        currentCD = Mathf.Min(currentCD, maxCD);
        UpdateUI();
    }

    public static void UpdateUI()
    {
        if (!fullBarRect || !cdBarRect)
        {
            fullBarRect = GameObject.Find($"Canvas/Cooldown Bar").GetComponent<RectTransform>();
            cdBarRect = fullBarRect.GetChild(0).GetComponent<RectTransform>();
        }
        float ratio = currentCD / maxCD;
        cdBarRect.localScale = new Vector3(ratio, cdBarRect.localScale.y, cdBarRect.localScale.z);
    }

    public static void MorePickup()
    {
        maxCD *= 1.1f;
        currentCD *= 1.1f;
        fullBarRect.localScale = new Vector3(fullBarRect.localScale.x * 1.1f, fullBarRect.localScale.y, fullBarRect.localScale.z);
        UpdateUI();
    }

}
