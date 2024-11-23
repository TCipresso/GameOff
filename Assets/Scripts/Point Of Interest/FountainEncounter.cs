using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Encounters/Fountain Encounter")]
public class FountainEncounter : Encounter
{
    [Header("Fountain Flavor")]
    [TextArea(3, 10)]
    [SerializeField] string batheString;
    [TextArea(3, 10)]
    [SerializeField] string ignoreString;

    public override string GetDescription()
    {
        return base.GetDescription();
    }

    public override bool IsEncounterKeyword(string[] tokens)
    {
        if(base.IsEncounterKeyword(tokens)) return true;
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "bathe":
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
                case "bathe":
                    Debug.Log("Accepted Fountain");
                    return AcceptFountain();
                case "ignore":
                    Debug.Log("Ignored Fountain");
                    return DenyFountain();
            }
        }

        return $"Keyword not recognized.";
    }

    private string AcceptFountain()
    {
        PlayerStats.instance.playerHP = PlayerStats.instance.maxHP;
        GameManager.instance.LeaveEnounter();
        return batheString;
    }

    private string DenyFountain()
    {
        GameManager.instance.LeaveEnounter();
        return ignoreString;
    }
}
