using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WizardEncounter is a noncombat <see cref="Encounter"/>
/// Combines the Orb Room and Wizard from design doc.
/// The wizard offers the player to chant in order to stare into the orb.
/// If the player fails the chant, they take damage to health and stats.
/// If the player succeeds the chant, they learn Foresight.
/// If the player has tobacco, they can skip the chant and stare into the orb.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Wizard Encounter")]
public class WizardEncounter : Encounter, Waiter
{
    [Header("Wizard Flavor")]
    [SerializeField] float chantWait = 3f;
    [TextArea(3, 10)][SerializeField] string chantText;
    [TextArea(3, 10)][SerializeField] string ignoreText;
    [TextArea(3, 10)][SerializeField] string smokeText;
    [TextArea(3, 10)][SerializeField] string chantPassText;
    [TextArea(3, 10)][SerializeField] string chantFailText;

    [Header("Wizard Cheat")]
    [SerializeField] CheatCode cheatCode;

    [Header("Wizard Punishment")]
    [SerializeField] int speedLost = 10;
    [SerializeField] int typingSpeedLost = 5;
    [SerializeField] int damage = 10;
    public override void CompleteMinigame(MinigameStatus gameResult)
    {
        switch (gameResult)
        {
            case MinigameStatus.MISSBUMP:
            case MinigameStatus.LOST:
                ChantFail();
                break;
            case MinigameStatus.FISTBUMP:
            case MinigameStatus.WIN:
                ChantPass();
                break;
            default:
                TextOutput.instance.Print($"Unknown game status {gameResult}");
                break;
        }
    }

    private void ChantPass()
    {
        TextOutput.instance.Print(chantPassText);
        Foresight();
        CheatCodeManager.instance.AddCheatToDiscovered(cheatCode);
        LeaveEncounter();
    }

    /// <summary>
    /// Lists the encounters of all future routes.
    /// </summary>
    private void Foresight()
    {
        List<Route> foresight = GameManager.instance.GetCurrentPOIRoutes();
        foreach(Route route in foresight)
        {
            PointOfInterest destination = route.GetDestination();
            string futureEncounterName = destination.HasEncounter() ?
                destination.GetEncounter().GetEncounterName() :
                "nothing";
            TextOutput.instance.Print($"To the {route.GetDirection()}, there is {futureEncounterName}.");
        }
    }

    private void ChantFail()
    {
        TextOutput.instance.Print(chantFailText);
        
        //I could change these to the exact punishments in the doc, but these are the ones that are currently implemented and we do not have a lot of time.
        PlayerStats.instance.DecreaseSpeed(speedLost);
        PlayerStats.instance.DecreaseTypingSpeed(typingSpeedLost);
        PlayerStats.instance.TakeDamage(damage);
        LeaveEncounter();
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if(base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "chant":
                case "pass":
                case "smoke":
                    return true;
            }
        }
        return false;
    }

    public override string ParseEncounterKeywords(string[] tokens)
    {
        string baseResponse = base.ParseEncounterKeywords(tokens);
        if(!baseResponse.Equals("Keyword not recognized.")) return baseResponse;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "chant":
                    return Chant();
                case "pass":
                    return Pass();
                case "smoke":
                    return Smoke();
            }
        }

        return $"Keyword not recognized.";
    }

    private string Chant()
    {
        Wait.instance.WaitForSeconds(chantWait, this);
        return chantText;
    }

    private string Pass()
    {
        LeaveEncounter();
        return ignoreText;
    }

    private string Smoke()
    {
        try
        {
            PlayerInventory.instance.RemoveSmoke();
        } catch (InvalidOperationException e)
        {
            TextOutput.instance.Print(e.Message);
            return null;
        }
        TextOutput.instance.Print(smokeText);
        ChantPass();
        return null;
    }

    public void WaitComplete()
    {
        StartMinigame();
    }
}
