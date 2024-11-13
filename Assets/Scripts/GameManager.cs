using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// GameManager is the class that stores the current
/// state of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Starting POI")]
    [SerializeField] PointOfInterest currentPOI;
    public static GameManager instance { private set; get; }
    [SerializeField] private bool inEncounter = false; //Only serialized for debugging purposes.
    
    /// <summary>
    /// Singleton
    /// </summary>
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;

        inEncounter = currentPOI.HasEncounter();
    }

    /// <summary>
    /// Gets current <see cref="PointOfInterest"/> description.
    /// </summary>
    /// <returns>The description of current <see cref="PointOfInterest"/></returns>
    public string GetCurrentPOIDesc()
    {
        return currentPOI.GetDescription();
    }

    /// <summary>
    /// Gets current <see cref="PointOfInterest"/> image.
    /// </summary>
    /// <returns>The sprite of the current <see cref="PointOfInterest"/></returns>
    public Sprite GetCurrentPOIImage()
    {
        return currentPOI.GetImage();
    }

    /// <summary>
    /// Checks if current <see cref="PointOfInterest"/> or current POI's <see cref="Encounter"/> can parse the input.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>True if the current <see cref="PointOfInterest"/> or its <see cref="Encounter"/> can parse the input, false otherwise.</returns>
    public bool IsPOIKeyword(string[] tokens)
    {
        return (inEncounter && currentPOI.IsEncounterKeyword(tokens)) || currentPOI.IsPOIKeyword(tokens);
    }

    /// <summary>
    /// Parse the player's input using the current POI.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>A response message from <see cref="PointOfInterest"/> message parse.</returns>
    public string ParsePOIKeyword(string[] tokens)
    {
        return currentPOI.ParsePOIKeywords(tokens);
    }

    /// <summary>
    /// Attempts to move in the direction stated in the tokens.
    /// </summary>
    /// <param name="tokens">Tokens from the Move command.</param>
    /// <returns>The description of the new <see cref="PointOfInterest"/> or a line stating move failure.</returns>
    public string AttemptMove(string[] tokens)
    {
        if (inEncounter /*&& !noclipped*/) return "You cannot leave in the middle of an encounter!";

        PointOfInterest destination = currentPOI.Move(tokens);
        if(destination == null) return "Cannot move in that direction.";
        currentPOI = destination;
        inEncounter = currentPOI.HasEncounter();
        return currentPOI.GetDescription();
    }

    /// <summary>
    /// Tracks if the game is currently in an <see cref="Encounter"/>.
    /// </summary>
    /// <returns>True if the game is currently in an <see cref="Encounter"/>, false otherwise.</returns>
    public bool IsInEncounter()
    {
        return inEncounter;
    }

    /// <summary>
    /// Sets inEncounter to false.
    /// </summary>
    public void LeaveEnounter()
    {
        inEncounter = false;
    }
}
