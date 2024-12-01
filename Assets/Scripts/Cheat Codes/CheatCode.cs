using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public enum EffectType { GodMode, MegaDamages, SuperSpeed, Foresight , NoClip, Restart, UnlockAll, MatPat }
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
            case EffectType.Restart:
                Debug.Log("Noclip effect executed.");
                Restart();
                break;
            case EffectType.UnlockAll:
                Debug.Log("Noclip effect executed.");
                UnlockAll();
                break;
            case EffectType.MatPat:
                Debug.Log("Noclip effect executed.");
                MatPat();
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
        if (GameManager.instance.IsInEncounter())
        {
            Encounter current = GameManager.instance.GetCurrentPOI().GetEncounter();

            if (current.IsCombatEncounter())
            {
                Combat.instance.EndCombat(true);
                Combat.instance.SetCombatStateToEnd();
                Debug.Log("NoClip: Combat encounter ended, state set to EndCombat.");
            }
            else
            {
                current.LeaveEncounter();
                Debug.Log("NoClip: Non-combat encounter skipped.");
            }
        }
        else
        {
            Debug.Log("NoClip: No encounter to skip.");
        }
    }


    private void Restart()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void UnlockAll()
    {
        if (CheatCodeManager.instance == null)
        {
            Debug.LogError("CheatCodeManager instance is missing!");
            return;
        }

        int cheatCount = CheatCodeManager.instance.undiscoveredCheatCodes.Count;

        if (cheatCount > 0)
        {
            foreach (CheatCode cheat in CheatCodeManager.instance.undiscoveredCheatCodes)
            {
                CheatCodeManager.instance.AddCheatToDiscovered(cheat);
            }

            CheatCodeManager.instance.undiscoveredCheatCodes.Clear();

            Debug.Log($"Unlocked all cheats! {cheatCount} cheats have been moved to discovered.");
            TextOutput.instance.Print($"Unlocked all cheats! {cheatCount} cheats are now available.");
        }
        else
        {
            Debug.Log("No cheats left to unlock!");
            TextOutput.instance.Print("No cheats left to unlock!");
        }
    }

    private void MatPat()
    {
        TextOutput.instance.Print($"You seek lore I see. Then you may have it. The FitnessGram Pacer Test is a multistage aerobic capacity test that progressively gets more difficult as it continues. The 20 meter pacer test will begin in 30 seconds. Line up at the start. The running speed starts slowly but gets faster each minute after you hear this signal bodeboop. A sing lap should be completed every time you hear this sound. ding Remember to run in a straight line and run as long as possible. The second time you fail to complete a lap before the sound, your test is over. The test will begin on the word start. On your mark. Get ready!… Start. ding﻿");
    }
}


