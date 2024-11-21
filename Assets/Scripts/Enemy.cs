using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int damage;
    [SerializeField] public int speed;

    /// <summary>
    /// Reduces the enemy's health.
    /// </summary>
    /// <param name="damage">Amount of damage taken.</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    /// <summary>
    /// Gets the enemy's attack value.
    /// </summary>
    /// <returns>The attack value.</returns>
    public int GetAttack()
    {
        return damage;
    }

    /// <summary>
    /// Gets the current health of the enemy.
    /// </summary>
    /// <returns>The current health.</returns>
    public int GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Checks if the enemy is defeated.
    /// </summary>
    /// <returns>True if health is 0 or below, false otherwise.</returns>
    public bool IsDefeated()
    {
        return health <= 0;
    }
}

