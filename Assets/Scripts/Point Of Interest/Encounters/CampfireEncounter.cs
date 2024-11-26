using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CampfireEncounter is a noncombat <see cref="Encounter"/>.
/// It has limited uses, but heals the player every use. Once 
/// all the uses are used, the fire burns out. There is a 
/// 50/50 chance that an enemy ambushes the player when the 
/// fire burns out.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Encounters/Campfire Encounter")]
public class CampfireEncounter : Encounter
{
    [Header("Campfire stats")]
    [SerializeField] int healAmount = 5;
    [SerializeField] int uses = 3;
    int usesLeft;
    [SerializeField] bool isEvil;
    [SerializeField] bool isSetUp = false; //Serialized for debugging

    [Header("Campfire Flavor")]
    [TextArea(3, 10)] [SerializeField] string restText;
    [TextArea(3, 10)] [SerializeField] string burnOutText;
    [TextArea(3, 10)] [SerializeField] string ambushText;
    [TextArea(3, 10)] [SerializeField] string leaveText;

    private void OnDisable()
    {
        isSetUp = false;
    }

    public override string GetDescription()
    {
        if(isSetUp) return base.GetDescription();

        usesLeft = uses;
        if (Random.Range(0, 2) == 0) isEvil = true;
        else isEvil = false;
        isSetUp = true;
        return base.GetDescription();
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if(base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "rest":
                case "leave":
                    return true;
            }
        }
        return false;
    }

    public override string ParseEncounterKeywords(string[] tokens)
    {
        string baseResponse = base.ParseEncounterKeywords(tokens);
        if (!baseResponse.Equals("Keyword not recognized.")) return baseResponse;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "rest":
                    return Rest();
                case "leave":
                    return Leave();
            }
        }

        return $"Keyword not recognized.";
    }

    /// <summary>
    /// Heal at the campfire.
    /// Use up a use.
    /// Burn out the flame if all uses are used.
    /// Ambush the player if the campfire is evil.
    /// </summary>
    /// <returns>Response to resting.</returns>
    private string Rest()
    {
        PlayerStats.instance.Heal(healAmount);
        usesLeft--;

        if(usesLeft > 0)
        {
            return restText;
        }

        if(isEvil)
        {
            Combat.instance.InitiateCombat(this);
            TextOutput.instance.Print(burnOutText);
            return ambushText;
        }

        GameManager.instance.LeaveEnounter();
        return burnOutText;
    }

    /// <summary>
    /// Leave the campfire.
    /// </summary>
    /// <returns>Response to leaving.</returns>
    private string Leave()
    {
        GameManager.instance.LeaveEnounter();
        return leaveText;
    }

    public override void LeaveEncounter()
    {
        isSetUp = false;
        base.LeaveEncounter();
    }
}
