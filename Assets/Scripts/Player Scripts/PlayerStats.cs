using System;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    //Hey, you should separate these into base stats and current stats.
    public int maxHP = 100;
    public int playerHP = 100;
    public int dmg = 10;
    public int speed = 5;
    public float baseTypingSpeed = 5f;
    public bool isDefending = false;

    [Header("UI References")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public event Action OnHealthChanged;

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
        OnHealthChanged?.Invoke();
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
}
