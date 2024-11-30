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
    [SerializeField] public string description = "This is a test encounter. Type \"continue\" to continue.";
    [SerializeField] int subjectSprite = -1;
    [SerializeField] int minigame;

    [Header("Encounter Details")]
    [SerializeField] string encounterName; // Name of the encounter
    [SerializeField] int enemyIndex; // Index of the enemy in EncounterManager
    [SerializeField] bool isCombat; // Whether this is a combat encounter
    [SerializeField] int enemySpeedMin = 3; // Minimum speed of the enemy
    [SerializeField] int enemySpeedMax = 7; // Maximum speed of the enemy
    [SerializeField] List<string> keywords = new List<string>(); //Only to be shown to the player.

    [Header("Enemy Attack Dialogue")]
    [TextArea(3, 10)]
    [SerializeField] List<string> attackDialogues = new List<string>(); // List of attack dialogues

    private int enemySpeed; // Speed of the enemy for this encounter

    /// <summary>
    /// Get the description of the encounter.
    /// </summary>
    /// <returns>The description of the encounter.</returns>
    public virtual string GetDescription()
    {
        EncounterSpriteManager.instance.ActivateSprite(subjectSprite);
        Debug.Log("Getting Description");
        return description;
    }

    /// <summary>
    /// Get the sprite that represents the encounter's subject/character.
    /// </summary>
    /// <returns>The index of the encounter's subject/character.</returns>
    public int GetSubjectSprite()
    {
        return subjectSprite;
    }

    /// <summary>
    /// Get the name of the encounter.
    /// </summary>
    /// <returns>The name of the encounter.</returns>
    public string GetEncounterName()
    {
        return encounterName;
    }

    /// <summary>
    /// Get the index of the enemy prefab for this encounter.
    /// </summary>
    /// <returns>The index of the enemy.</returns>
    public int GetEnemyIndex()
    {
        return enemyIndex;
    }

    /// <summary>
    /// Check if this encounter is a combat encounter.
    /// </summary>
    /// <returns>True if it is a combat encounter, false otherwise.</returns>
    public bool IsCombatEncounter()
    {
        return isCombat;
    }

    /// <summary>
    /// Initialize the enemy speed within the defined range.
    /// </summary>
    public void InitializeEnemySpeed()
    {
        enemySpeed = Random.Range(enemySpeedMin, enemySpeedMax + 1);
    }

    /// <summary>
    /// Get the speed of the enemy for this encounter.
    /// </summary>
    /// <returns>The speed of the enemy.</returns>
    public int GetEnemySpeed()
    {
        return enemySpeed;
    }

    /// <summary>
    /// Get the list of attack dialogues for this encounter.
    /// </summary>
    /// <returns>A list of attack dialogues.</returns>
    public List<string> GetAttackDialogues()
    {
        return attackDialogues;
    }

    /// <summary>
    /// Clean/Update game state and leave the encounter.
    /// </summary>
    public virtual void LeaveEncounter()
    {
        EncounterSpriteManager.instance.DeactivateSprite(subjectSprite);
        GameManager.instance.LeaveEnounter();
    }

    /// <summary>
    /// Gets keywords of the specific encounter to be shown to the player.
    /// </summary>
    /// <returns>The list of keywords.</returns>
    public virtual List<string> GetEncounterKeywords()
    {
        List<string> ans = new List<string>();
        ans.AddRange(keywords);
        return ans;
    }

    /// <summary>
    /// Checks if encounter can handle player's input.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>True if encounter can handle input, false otherwise.</returns>
    public virtual bool IsEncounterKeyword(string[] tokens)
    {
        return false;
    }

    /// <summary>
    /// Handles player input.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>A response message from activity.</returns>
    public virtual string ParseEncounterKeywords(string[] tokens)
    {
        return $"Keyword not recognized.";
    }

    /// <summary>
    /// Starts the <see cref="Minigame"/> this encounter is associated with
    /// through the <see cref="MinigameManager"/>
    /// </summary>
    public void StartMinigame()
    {
        MinigameManager.instance.PlayMinigame(minigame, this);
    }

    /// <summary>
    /// Handle the <see cref="MinigameStatus"/> from the <see cref="Minigame"/> I started.
    /// </summary>
    /// <param name="gameResult"></param>
    public virtual void CompleteMinigame(MinigameStatus gameResult)
    {
        switch (gameResult)
        {
            case MinigameStatus.MISSBUMP:
            case MinigameStatus.LOST:
                TextOutput.instance.Print("Game lost.");
                break;
            case MinigameStatus.FISTBUMP:
            case MinigameStatus.WIN:
                TextOutput.instance.Print("Game won.");
                LeaveEncounter();
                break;
            default:
                TextOutput.instance.Print($"Unknown game status {gameResult}");
                break;
        }
    }
}
