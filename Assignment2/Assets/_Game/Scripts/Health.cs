using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float regenerationFactor = 10f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private RectTransform fillRect;

    private float currentHealth = 100f;
    bool bCanRegenerate = false;

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

    void HealPickup()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void MaxHpPickup()
    {
        maxHealth *= 1.1f;
        currentHealth *= 1.1f;
        RectTransform sliderRect = healthSlider.GetComponent<RectTransform>();
        sliderRect.sizeDelta = new Vector2(sliderRect.sizeDelta.x * 1.1f, sliderRect.sizeDelta.y);
        UpdateHealthUI();
    }

    void Damage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        if (currentHealth < 0)
        {
            Lose();
        }
    }

    void RegenerationPickup()
    {
        bCanRegenerate = true;
    }

    void BoostRegeneration()
    {
        regenerationFactor *= 1.05f;
    }

    void Lose()
    {

    }

    void UpdateHealthUI()
    {
        fillRect.localScale = new Vector3(currentHealth / maxHealth * 1.1f, fillRect.localScale.y, fillRect.localScale.z);
    }
}
