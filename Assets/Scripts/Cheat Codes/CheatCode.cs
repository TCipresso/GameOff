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
            return false; 
        }

        if (effect != null)
        {
            effect.ApplyEffect();
            currentUses--; 
            TextOutput.instance.Print($"{cheatName} has {currentUses} charges remaining.");
            return true; 
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
            currentUses = maxUses;
        }
    }

}


[System.Serializable]
public class CheatEffect
{
    public enum EffectType { GodMode, MegaDamages, SuperSpeed, Foresight , NoClip }
    public EffectType effectType;

    [Header("Effect Values")]
    public int intValue;
    public float floatValue;

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
            case EffectType.MegaDamages:
                PlayerStats.instance.MegaDamage(intValue); // Increase damage by x times (2)
                Debug.Log($"Added {intValue} extra damage.");
                break;
            case EffectType.SuperSpeed:
                PlayerStats.instance.IncreaseSpeed(intValue); // Increase speed to x (2)
                Debug.Log($"Added {intValue} extra speed.");
                break;
            case EffectType.Foresight:
                Debug.Log("Foresight effect executed.");
                //Hans put FORESIGHT CALL HERE
                break;
            case EffectType.NoClip:
                Debug.Log("Foresight effect executed.");
                //Hans put whatever u want NoClip to do outisde combat here.
                //Tommy put whatever u want NoClip to do inside combat here.
                break;
            default:
                Debug.LogWarning("No valid effect type found");
                break;
        }
    }
}
