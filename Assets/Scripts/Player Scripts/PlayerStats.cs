using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerHP = 100;  
    public int dmg = 10; 
    public int speed = 5;
    public float baseTypingSpeed = 5f;

    public void TakeDamage(int amount)
    {
        playerHP -= amount;
        if (playerHP <= 0)
        {
            playerHP = 0;
            Die();
        }
    }

    public void IncreaseDamage(int amount)
    {
        dmg += amount;
    }

    public void DecreaseDamage(int amount)
    {
        dmg = Mathf.Max(0, dmg - amount);
    }

    public void IncreaseSpeed(int amount)
    {
        speed += amount;
    }

    public void DecreaseSpeed(int amount)
    {
        speed = Mathf.Max(0, speed - amount);
    }

    private void Die()
    {
        if (playerHP <= 0)
        {
            Debug.Log("Player has died.");
            //Game Over Screen Here.
        
        }
}
}
