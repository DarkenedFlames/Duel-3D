using System;
using UnityEngine;

public class StatsHandler : MonoBehaviour
{
    [Header("Sliders")]
    [Tooltip("Do not add for AI players")]
    [SerializeField] private ResourceBarUI healthBarUI;
    [Tooltip("Do not add for AI players")]
    [SerializeField] private ResourceBarUI staminaBarUI;
    [Tooltip("Do not add for AI players")]
    [SerializeField] private ResourceBarUI manaBarUI;

    [Header("Max Stats")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float maxMana = 100f;

    [NonSerialized] public float health;
    [NonSerialized] public float stamina;
    [NonSerialized] public float mana;

    void Awake()
    {
        RefillHealth();
        RefillStamina();
        RefillMana();
        ClampStats();
    }

    // Update is called once per frame
    void Update()
    {
        ClampStats();
        if (health <= 0) Die();
        UpdateUI();
    }

    public void ClampStats()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        stamina = Mathf.Clamp(stamina, 0, maxStamina);       
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage!");
    }

    public void SpendStamina(float amount)
    {
        if (amount <= 0) return;
        stamina -= amount;
        Debug.Log($"{gameObject.name} spent {amount} stamina!");
    }

    public void SpendMana(float amount)
    {
        if (amount <= 0) return;
        mana -= amount;
        Debug.Log($"{gameObject.name} spent {amount} mana!");
    }

    public void RefillHealth() => health = maxHealth;
    public void RefillStamina() => stamina = maxStamina;
    public void RefillMana() => mana = maxMana;
    
    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }

    private void UpdateUI()
    {
        if (healthBarUI != null) healthBarUI.SetValue(health, maxHealth);
        if (staminaBarUI != null) staminaBarUI.SetValue(stamina, maxStamina);
        if (manaBarUI != null) manaBarUI.SetValue(mana, maxMana);
    }
}
