using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An Encounter is an activity that is stored in a <see cref="PointOfInterest"/>.
/// </summary>
/*[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Test Encounter")] DO NOT USE THIS ENCOUNTER FOR ANYTHING OTHER THAN TESTS
 * ALL EXTENSIONS OF ENCOUNTER SHOULD HAVE menuName = "Scriptable Objects/Encounters/abc FOR ITS CreateAssetMenu"*/
public class Encounter : ScriptableObject, MinigameCaller
{
    [TextArea(3, 10)]
    [SerializeField] string description = "This is a test encounter. Type \"continue\" to continue.";
    [SerializeField] Sprite subjectSprite;
    [SerializeField] Minigame minigame;

    /// <summary>
    /// Get the description of the encounter.
    /// </summary>
    /// <returns>The description of the encounter.</returns>
    public string GetDescription()
    {
        return description;
    }

    /// <summary>
    /// Get the sprite that represents the encounter's subject/character.
    /// </summary>
    /// <returns>A <see cref="Sprite"/> of the encounter's subject/character.</returns>
    public Sprite GetSubjectSprite()
    {
        return subjectSprite;
    }

    /// <summary>
    /// Clean/Update game state and leave the encounter.
    /// </summary>
    public void LeaveEncounter()
    {
        GameManager.instance.LeaveEnounter();
    }

    /// <summary>
    /// Checks if encounter can handle player's input.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>True if encounter can handle input, false otherwise.</returns>
    public bool IsEncounterKeyword(string[] tokens)
    {
        foreach(string token in tokens)
        {
            switch(token)
            {
                case "continue":
                case "play":
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Handles player input.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>A response message from activity.</returns>
    public string ParseEncounterKeywords(string[] tokens)
    {
        foreach(string token in tokens)
        {
            switch(token)
            {
                case "continue":
                    LeaveEncounter();
                    Debug.Log("Leaving Encounter");
                    return "Leaving Encounter";
                case "play":
                    StartMinigame();
                    if(minigame != null) Debug.Log($"{name} starting {minigame.name}");
                    return "Starting Minigame";
            }
        }

        return $"Keyword not recognized for {name}.";
    }

    public void StartMinigame()
    {
        if(minigame == null)
        {
            Debug.LogError($"{name} does not have a minigame and you are trying to start one!");
            return;
        }
        minigame.StartMinigame(this);
    }

    public void CompleteMinigame(MinigameStatus gameResult)
    {
        switch(gameResult)
        {
            case MinigameStatus.LOST:
                TextOutput.instance.Print("Game lost.");
                break;
            case MinigameStatus.WIN:
                TextOutput.instance.Print("Game won.");
                break;
            default:
                TextOutput.instance.Print($"Unknown game status {gameResult}");
                break;
        }
    }
}
