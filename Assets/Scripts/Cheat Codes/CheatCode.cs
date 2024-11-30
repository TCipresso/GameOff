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
                Foresight();
                break;
            case EffectType.NoClip:
                Debug.Log("Noclip effect executed.");
                NoClip();
                break;
            default:
                Debug.LogWarning("No valid effect type found");
                break;
        }

    }

    private void Foresight()
    {
        List<Route> foresight = GameManager.instance.GetCurrentPOIRoutes();
        foreach (Route route in foresight)
        {
            PointOfInterest destination = route.GetDestination();
            string futureEncounterName = destination.HasEncounter() ?
                destination.GetEncounter().GetEncounterName() :
                "nothing";
            TextOutput.instance.Print($"To the {route.GetDirection()}, there is {futureEncounterName}.");
        }
    }
    
    private void NoClip()
    {
        if(GameManager.instance.IsInEncounter())
        {
            Encounter current = GameManager.instance.GetCurrentPOI().GetEncounter();
            if (current.IsCombatEncounter()) Combat.instance.EndCombat(true); //Tommy put whatever u want NoClip to do inside combat here.
            else current.LeaveEncounter(); //Hans put whatever u want NoClip to do outisde combat here.
        }   
    }
}
