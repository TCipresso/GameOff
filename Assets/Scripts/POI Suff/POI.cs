using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will act as the Basic for the POI concept I made. 
/// For now I will just use strings for testing however will change this to something else. 
/// Interactions are just a string for now. We should change these to actually code logic of course. 
/// </summary>
[CreateAssetMenu(fileName = "NewPOI", menuName = "Scriptable Objects/Create New POI")]
public class POI : ScriptableObject
{
    public string poiName;
    public string description;
    public Sprite image;
    public string[] dialogueOptions;
    public string specialCheatCode;
    public string[] interactions;

    // Add any other fields unique to each POI
}
