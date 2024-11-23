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

    public void Die()
    {
        Debug.Log("Player has died.");
    }
}
