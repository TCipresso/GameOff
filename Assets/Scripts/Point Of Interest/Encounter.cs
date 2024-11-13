using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Encounter")]
public class Encounter : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] string description = "No Description";
    [SerializeField] Sprite subjectSprite;

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSubjectSprite()
    {
        return subjectSprite;
    }
}
