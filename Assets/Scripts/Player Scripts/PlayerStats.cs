using System;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    public int maxHP = 100;
    public int playerHP = 100;
    public int baseDmg = 15; // Base damage for the player
    public int dmg = 15;     // Current damage (can increase during mini-game)
    public int speed = 5;
    public float baseTypingSpeed = 5f;
    public bool isDefending = false;

    [Header("UI References")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public event Action OnHealthChanged;//this flashes the UI red when player takes damage.

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

    private void Awake()
    {
        if (instance == null) instance = this; //Can't destroy as this is with every other major system :(.
    }

    void Start()
    {
        UpdateUI();
    }

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
        OnHealthChanged?.Invoke();//this flashes the UI red when player takes damage.
    }

    /// <summary>
    /// Heals player to full HP.
    /// </summary>
    public void Heal()
    {
        Heal(maxHP);
    }

    /// <summary>
    /// Heals player by provided amount.
    /// </summary>
    /// <param name="amount">Amount to heal player.</param>
    public void Heal(int amount)
    {
        playerHP += amount;
        if (playerHP > maxHP) playerHP = maxHP;
        UpdateUI();
        //OnHealthChanged?.Invoke(); //I don't know what this is, so I'm not gonna use it just in case. I'll let you properly handle that.
    }

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

    public void Heal()
    {
        Heal(maxHP);
    }

    /// <summary>
    /// Heals player by provided amount.
    /// </summary>
    /// <param name="amount">Amount to heal player.</param>
    public void Heal(int amount)
    {
        playerHP += amount;
        if (playerHP > maxHP) playerHP = maxHP;
        UpdateUI();
    }

}
