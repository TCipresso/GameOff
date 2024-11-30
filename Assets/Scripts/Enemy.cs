using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] public int speed;

    private int originalHealth;
    private int originalDamage;
    private int originalSpeed;

    private void Awake()
    {
        // Save the original stats
        originalHealth = health;
        originalDamage = damage;
        originalSpeed = speed;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public int GetAttack()
    {
        return damage;
    }

    public int GetHealth()
    {
        return health;
    }

    public bool IsDefeated()
    {
        return health <= 0;
    }

    /// <summary>
    /// Resets the enemy's stats to their original values.
    /// </summary>
    public void ResetStats()
    {
        health = originalHealth;
        damage = originalDamage;
        speed = originalSpeed;
        Debug.Log($"{name} stats have been reset: Health={health}, Damage={damage}, Speed={speed}");
    }
}


