using System;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int maxHP = 100;
    public int playerHP = 100;
    public int baseDmg = 15; 
    public int dmg = 15;     
    public int speed = 5;
    private int baseSpeed; 
    public float baseTypingSpeed = 99999f; 
    private float currentTypingSpeed;      
    private int previousHP;                
    public bool isDefending = false;

    [Header("UI References")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public event Action OnHealthChanged; // Flashes the UI red when player takes damage.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate PlayerStats detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    void UpdateUI()
    {
        hpText.text = $"{playerHP}";
        damageText.text = $"{dmg}";
        speedText.text = $"{speed}";
    }

    void Start()
    {
        baseSpeed = speed; 
        currentTypingSpeed = baseTypingSpeed;
        UpdateUI();
    }

    /// <summary>
    /// Reduces the player's HP by the given amount and updates the UI.
    /// Triggers health change events.
    /// </summary>
    /// <param name="amount">Amount of damage to deal.</param>
    public void TakeDamage(int amount)
    {
        playerHP -= amount;
        if (playerHP <= 0)
        {
            playerHP = 0;
            Die();
        }
        else
        {
            Debug.Log($"Player took {amount} damage. Remaining HP: {playerHP}");
        }
        UpdateUI();
        OnHealthChanged?.Invoke(); // Flashes the UI red when player takes damage.
    }

    /// <summary>
    /// Heals player to full HP.
    /// </summary>
    public void Heal()
    {
        Heal(maxHP);
    }

    /// <summary>
    /// Heals player by the specified amount.
    /// </summary>
    /// <param name="amount">Amount to heal the player.</param>
    public void Heal(int amount)
    {
        playerHP += amount;
        if (playerHP > maxHP) playerHP = maxHP;
        UpdateUI();
    }

    /// <summary>
    /// Handles player death.
    /// </summary>
    public void Die()
    {
        Debug.Log("Player has died.");
    }

    /// <summary>
    /// Adds bonus damage to the player's current damage and updates the UI.
    /// </summary>
    /// <param name="bonus">Amount of bonus damage to add.</param>
    public void AddDamage(int bonus)
    {
        dmg += bonus;
        Debug.Log($"Bonus damage added! Current damage: {dmg}");
        UpdateUI();
    }

    /// <summary>
    /// Resets the player's damage to the base value and updates the UI.
    /// </summary>
    public void ResetDamage()
    {
        dmg = baseDmg;
        Debug.Log($"Damage reset to base value: {dmg}");
        UpdateUI();
    }

    /// <summary>
    /// Increases the player's speed by the specified amount and updates the UI.
    /// </summary>
    /// <param name="amount">Amount to increase speed by.</param>
    public void IncreaseSpeed(int amount)
    {
        speed += amount;
        Debug.Log($"Speed increased by {amount}. Current speed: {speed}");
        UpdateUI();
    }

    /// <summary>
    /// Resets the player's speed to the base value and updates the UI.
    /// </summary>
    public void ResetSpeed()
    {
        speed = baseSpeed;
        Debug.Log($"Speed reset to base value: {speed}");
        UpdateUI();
    }

    /// <summary>
    /// Increases the player's speed by the specified amount and updates the UI.
    /// </summary>
    /// <param name="amount">Amount to increase speed by.</param>
    public void DecreaseSpeed(int amount)
    {
        speed -= amount;
        Debug.Log($"Speed decreased by {amount}. Current speed: {speed}");
        UpdateUI();
    }

    /// <summary>
    /// Enables GodMode by setting the player's HP to a very high value.
    /// </summary>
    public void EnableGodMode()
    {
        previousHP = playerHP; 
        playerHP = 999999; 
        Debug.Log($"GodMode enabled! Current HP: {playerHP}");
        UpdateUI();
    }

    /// <summary>
    /// Resets the player's HP to its previous state before GodMode.
    /// </summary>
    public void ResetCurrentHealth()
    {
        playerHP = previousHP;
        Debug.Log($"GodMode disabled! HP reset to previous value: {playerHP}");
        UpdateUI();
    }

    /// <summary>
    /// Decreases the player's typing speed by the specified amount.
    /// </summary>
    /// <param name="amount">Amount to decrease typing speed by.</param>
    public void DecreaseTypingSpeed(float amount)
    {
        currentTypingSpeed -= amount;
        if (currentTypingSpeed < 0) currentTypingSpeed = 0;
        Debug.Log($"Typing speed decreased by {amount}. Current typing speed: {currentTypingSpeed}");
    }

    /// <summary>
    /// Resets the player's typing speed to the base value.
    /// </summary>
    public void ResetTypingSpeed()
    {
        currentTypingSpeed = baseTypingSpeed;
        Debug.Log($"Typing speed reset to base value: {currentTypingSpeed}");
    }
}
