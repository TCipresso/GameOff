using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Cheat Code")]
public class CheatCode : ScriptableObject
{
    public string cheatName;
    public string[] alternateSpellings;
    public int maxUses;
    private int currentUses;
    public int minigameIndex;

    [Header("Cheat Effect Settings")]
    public CheatEffect effect;

    private void OnEnable()
    {
        currentUses = maxUses;
    }

    /// <summary>
    /// Executes the special effect associated with this cheat, if charges remain.
    /// </summary>
    public bool ExecuteEffect()
    {
        if (currentUses <= 0)
        {
            Debug.Log("Nice try, cheater! No more charges left.");
            return false; // Indicate that the cheat couldn't be executed
        }

        if (effect != null)
        {
            effect.ApplyEffect();
            currentUses--; // Decrease charges
            TextOutput.instance.Print($"{cheatName} has {currentUses} charges remaining.");
            return true; // Indicate success
        }
        else
        {
            Debug.LogWarning($"No effect defined for cheat: {cheatName}");
            return false;
        }
    }

    /// <summary>
    /// Resets the charges to maxUses.
    /// </summary>
    public void ResetCharges()
    {
        currentUses = maxUses;
        Debug.Log($"{cheatName} charges reset to {currentUses}.");
    }

    /// <summary>
    /// Gets the remaining charges for this cheat.
    /// </summary>
    public int GetRemainingCharges()
    {
        return currentUses;
    }

    public void AddCharge(int amount)
    {
        currentUses += amount;
        if (currentUses > maxUses)
        {
            currentUses = maxUses; // Cap the charges to the maximum
        }
    }

}


[System.Serializable]
public class CheatEffect
{
    public enum EffectType { GodMode, ExtraDamage, FreeAction, Heal, Custom }
    public EffectType effectType;

    [Header("Effect Values")]
    public int intValue;   // For effects like extra damage or heal amount
    public float floatValue; // For effects like speed multipliers

    /// <summary>
    /// Applies the specified effect based on the type.
    /// </summary>
    public void ApplyEffect()
    {
        switch (effectType)
        {
            case EffectType.GodMode:
                PlayerStats.instance.EnableGodMode(); // Example: Set health to 9999
                Debug.Log("God mode enabled.");
                break;
           /* case EffectType.ExtraDamage:
                PlayerStats.instance.AddDamage(intValue); // Increase damage
                Debug.Log($"Added {intValue} extra damage.");
                break;
            case EffectType.FreeAction:
                Debug.Log("Free action granted!");
                // Add logic for free actions here
                break;
            case EffectType.Heal:
                PlayerStats.instance.Heal(intValue); // Heal the player
                Debug.Log($"Player healed by {intValue} points.");
                break;
            case EffectType.Custom:
                Debug.Log("Custom effect executed.");
                // Implement custom logic here
              */  break;
            default:
                Debug.LogWarning("No valid effect type found!");
                break;
        }
    }
}
