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
    private bool isGodModeActive = false; 

    [Header("UI References")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public event Action OnHealthChanged;
    public GameObject gameOverScreen;

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

    void Start()
    {
        baseSpeed = 100;
        UpdateUI();
    }

    void UpdateUI()
    {
        hpText.text = $"{playerHP}";
        damageText.text = $"{dmg}";
        speedText.text = $"{speed}";
        DmgIndicator.instance.UpdateIndicatorColor();
    }

    public void TakeDamage(int amount)
    {
        playerHP -= amount;

        if (!isGodModeActive)
        {
            previousHP = playerHP;
        }

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

    public void Heal()
    {
        Heal(maxHP);
    }

    public void Heal(int amount)
    {
        playerHP += amount;
        if (playerHP > maxHP) playerHP = maxHP;
        UpdateUI();
    }

    public void Die()
    {
        Debug.Log("Player has died.");
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void AddDamage(int bonus)
    {
        dmg += bonus;
        Debug.Log($"Bonus damage added! Current damage: {dmg}");
        UpdateUI();
    }

    public void ResetDamage()
    {
        dmg = baseDmg;
        Debug.Log($"Damage reset to base value: {dmg}");
        UpdateUI();
    }

    public void IncreaseSpeed(int amount)
    {
        speed += amount;
        Debug.Log($"Speed increased by {amount}. Current speed: {speed}");
        UpdateUI();
    }

    public void ResetSpeed()
    {
        speed = baseSpeed;
        Debug.Log($"Speed reset to base value: {speed}");
        UpdateUI();
    }

    public void DecreaseSpeed(int amount)
    {
        speed -= amount;
        Debug.Log($"Speed decreased by {amount}. Current speed: {speed}");
        UpdateUI();
    }

    public void MegaDamage(int bonus)
    {
        dmg += bonus;
        Debug.Log($"Bonus damage added! Current damage: {dmg}");
        UpdateUI();
    }

    public void EnableGodMode()
    {
        isGodModeActive = true; 
        previousHP = playerHP;
        playerHP = 999999;
        Debug.Log($"GodMode enabled! Current HP: {playerHP}");
        UpdateUI();
    }

    public void DisableGodMode()
    {
        isGodModeActive = false; 
        Debug.Log("GodMode disabled.");
    }

    public void ResetCurrentHealth()
    {
        playerHP = previousHP;
        Debug.Log($"GodMode disabled! HP reset to previous value: {playerHP}");
        UpdateUI();
    }

    public void DecreaseTypingSpeed(float amount)
    {
        currentTypingSpeed -= amount;
        if (currentTypingSpeed < 0) currentTypingSpeed = 0;
        Debug.Log($"Typing speed decreased by {amount}. Current typing speed: {currentTypingSpeed}");
    }

    public void ResetTypingSpeed()
    {
        currentTypingSpeed = baseTypingSpeed;
        Debug.Log($"Typing speed reset to base value: {currentTypingSpeed}");
    }

    public void ResetStats()
    {
        Debug.Log("Resetting player stats to base values.");
        dmg = baseDmg;
        speed = baseSpeed;
        currentTypingSpeed = baseTypingSpeed;
        playerHP = previousHP;
        isGodModeActive = false;
        Debug.Log($"Stats reset. Current HP: {playerHP}, Damage: {dmg}, Speed: {speed}, Typing Speed: {currentTypingSpeed}");
        UpdateUI();
    }
}
