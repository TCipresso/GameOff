using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Encounter")]
public class Encounter : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] string description = "This is a test encounter. Type continue to continue.";
    [SerializeField] Sprite subjectSprite;

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSubjectSprite()
    {
        return subjectSprite;
    }

    public void LeaveEncounter()
    {
        GameManager.instance.LeaveEnounter();
    }

    public bool IsEncounterKeyword(string[] tokens)
    {
        foreach(string token in tokens)
        {
            if (token.Equals("continue")) return true;
        }
        return false;
    }

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
            }
        }

        return $"Keyword not recognized for {name}.";
    }
}
