using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A point of interest is the current location state of the game.
/// It defines the location and interactions available.
/// </summary>
[CreateAssetMenu(menuName = "Scriptable Objects/Point of Interest")]
public class PointOfInterest : ScriptableObject
{
    [SerializeField] List<Route> routes = new List<Route>();
    [SerializeField] Sprite image;
    [SerializeField] Encounter encounter;
    [TextArea(3, 10)]
    [SerializeField] string noEncounterString = "This room is empty. You are safe.";

    /// <summary>
    /// States if the POI has an <see cref="Encounter"/>.
    /// </summary>
    /// <returns>True if POI has an <see cref="Encounter"/>, false otherwise.</returns>
    public bool HasEncounter()
    {
        return encounter != null;
    }

    /// <summary>
    /// Get the description of the current POI if it has one.
    /// </summary>
    /// <returns>The description (or lack of) of the POI.</returns>
    public string GetDescription()
    {
        if (encounter == null) return noEncounterString;
        return encounter.GetDescription();
    }

    /// <summary>
    /// Gets the Image of the current POI if it has one.
    /// </summary>
    /// <returns>The Image of the POI if present or the global default image.</returns>
    public Sprite GetImage()
    {
        if (image == null) return null; //We should define a global default image.
        return image;
    }

    /// <summary>
    /// Checks if tokens contains a keyword for the POI.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>True if POI can parse input, false otherwise.</returns>
    public bool IsPOIKeyword(string[] tokens)
    {
        foreach (string token in tokens)
        {
            if (token.Equals("search")) return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if tokens contains a keyword for POI's <see cref="Encounter"/>.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>True if POI's <see cref="Encounter"/> can parse input, false otherwise.</returns>
    public bool IsEncounterKeyword(string[] tokens)
    {
        if (encounter == null) return false;
        return encounter.IsEncounterKeyword(tokens);
    }

    /// <summary>
    /// Reads player's input tokens. POI or it <see cref="Encounter"/> handles input depending on
    /// the game state and who can handle it.
    /// </summary>
    /// <param name="tokens">Tokens from player input.</param>
    /// <returns>A response message from activity.</returns>
    public string ParsePOIKeywords(string[] tokens)
    {
        if (GameManager.instance.IsInEncounter() && (encounter != null && encounter.IsEncounterKeyword(tokens)))
        {
            return encounter.ParseEncounterKeywords(tokens);
        }
        else
        {
            foreach (string token in tokens)
            {
                switch (token)
                {
                    case "search":
                        return GameManager.instance.IsInEncounter() ? 
                            (encounter != null ? encounter.GetDescription() : "How are we in an encounter but POI doesn't have one???") 
                            : noEncounterString;
                }
            }
        }

        return $"Keyword not recognized for {name}.";
    }

    /// <summary>
    /// Move through a route.
    /// </summary>
    /// <param name="tokens">Tokens defining what route to take.</param>
    /// <returns>The <see cref="PointOfInterest"/> if the route is vaild, null otherwise.</returns>
    public PointOfInterest Move(string[] tokens)
    {
        foreach(Route route in routes)
        {
            foreach(string token in tokens)
            {
                if (token.Equals(route.GetDirection().ToLower().Trim()) && route.CanTravel()) return route.GetDestination();
            }
        }
        return null;
    }
}
