using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LibraryEncounter is a noncombat <see cref="Encounter"/>.
/// The player is prompted to read or ignore a library book.
/// If they read the book, they are given a minigame.
/// If they succeed in the minigame, they are told a cheat code.
/// If they fail in the minigame, they lose health and speed.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Library Encounter")]
public class LibraryEncounter : Encounter, Waiter
{
    [Header("Library Flavor")]
    [Tooltip("Allows the player to read output text before the minigame.")]
    [SerializeField] float waitBeforeReadingBook = 3;
    [TextArea(3, 10)][SerializeField] string readText;
    [TextArea(3, 10)][SerializeField] string readSuccess;
    [TextArea(3, 10)][SerializeField] string readFailure;
    [TextArea(3, 10)][SerializeField] string ignoreText;
    [SerializeField] bool waiting = false; //To make sure the player cannot enter any keywords during the reading wainting period.

    [Header("Minigame Failure Punishments")]
    [SerializeField] int damage = 1;
    [SerializeField] int speedLost = 5;
    [SerializeField] int typingSpeedLost = 1;

    private void OnDisable()
    {
        waiting = false;
    }

    public override void CompleteMinigame(MinigameStatus gameResult)
    {
        switch (gameResult)
        {
            case MinigameStatus.MISSBUMP:
            case MinigameStatus.LOST:
                MinigameLost();
                break;
            case MinigameStatus.FISTBUMP:
            case MinigameStatus.WIN:
                MinigameWin();
                break;
            default:
                TextOutput.instance.Print($"Unknown game status {gameResult}");
                break;
        }
    }

    /// <summary>
    /// Gives the player a cheat code.
    /// </summary>
    private void MinigameWin()
    {
        //TODO: This is where I'd give the player the cheat code.
        TextOutput.instance.Print(readSuccess);
        LeaveEncounter();
    }

    /// <summary>
    /// Damage player and lower both their speeds.
    /// </summary>
    private void MinigameLost()
    {
        PlayerStats.instance.DecreaseSpeed(speedLost);
        PlayerStats.instance.DecreaseTypingSpeed(typingSpeedLost);
        PlayerStats.instance.TakeDamage(damage);
        TextOutput.instance.Print(readFailure);
        LeaveEncounter();
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if (waiting) return false;
        if(base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "read":
                case "ignore":
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
                case "read":
                    return Read();
                case "ignore":
                    return Ignore();
            }
        }

        return $"Keyword not recognized.";
    }

    /// <summary>
    /// Start minigame to read book.
    /// </summary>
    /// <returns>Response to reading book.</returns>
    private string Read()
    {
        waiting = true;
        Wait.instance.WaitForSeconds(waitBeforeReadingBook, this);
        return readText;
    }

    /// <summary>
    /// Ignore the book.
    /// </summary>
    /// <returns>Null for spahgetti nonsense</returns>
    private string Ignore()
    {
        LeaveEncounter();
        return ignoreText;
    }

    public void WaitComplete()
    {
        waiting = false;
        StartMinigame();
    }
}
