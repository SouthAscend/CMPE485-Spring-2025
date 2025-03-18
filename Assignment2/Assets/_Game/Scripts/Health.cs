using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float regenerationFactor = 5f;
    [SerializeField] private RectTransform fullBarRect;
    [SerializeField] private RectTransform healthBarRect;

    private float currentHealth;
    private bool bCanRegenerate = false;
    private bool bInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (bCanRegenerate)
        {
            currentHealth += Time.deltaTime * regenerationFactor;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateHealthUI();
        }
    }

    public void HealPickup()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void MaxHpPickup()
    {
        maxHealth *= 1.1f;
        currentHealth *= 1.1f;
        fullBarRect.localScale = new Vector3(fullBarRect.localScale.x * 1.1f, fullBarRect.localScale.y, fullBarRect.localScale.z);
        UpdateHealthUI();
    }

    public void Damage(float damage)
    {
        if (bInvincible) return;
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Lose();
        }
        UpdateHealthUI();
    }

    public void RegenerationPickup()
    {
        bCanRegenerate = true;
    }

    public void BoostRegeneration()
    {
        regenerationFactor *= 1.05f;
    }

    void Lose()
    {
        // Define lose behavior here
    }

    void UpdateHealthUI()
    {
        float healthRatio = currentHealth / maxHealth;
        healthBarRect.localScale = new Vector3(healthRatio, healthBarRect.localScale.y, healthBarRect.localScale.z);
    }

    public IEnumerator InvincibilityPickup()
    {
        bInvincible = true;
        yield return new WaitForSeconds(10f);
        bInvincible = false;
    }
}
