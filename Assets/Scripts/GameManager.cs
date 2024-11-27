using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField] private bool inEncounter = false; // Only serialized for debugging purposes.

    [Header("Route Prompt Info")]
    [SerializeField] string routePromptStart;
    [SerializeField] string routePromptEnd;
    StringBuilder routePrompt = new StringBuilder();
    [TextArea(3, 10)][SerializeField] string noRoutePrompt;

    /// <summary>
    /// Singleton
    /// </summary>
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;

        inEncounter = currentPOI.HasEncounter();
        CheckEncounterType(); // Check the encounter type at the start of the game
        TextOutput.instance.Print(GetCurrentPOIDesc());
        if (!inEncounter) PromptRoutes();
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
    /// Gets the routes of the current POI.
    /// </summary>
    /// <returns></returns>
    public List<Route> GetCurrentPOIRoutes()
    {
        return currentPOI.GetRoutes();
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
    /// Gets all keywords to be shown to the player.
    /// </summary>
    /// <returns>A list of keywords.</returns>
    public List<string> GetKeywords()
    {
        List<string> response = currentPOI.GetPOIKeywords();
        if(response == null) response = new List<string>();

        //These keywords are found in the InputParser.
        //They're defined here due to the natural flow from Encounter -> POI -> GameManager due their these object's relationships.
        //The code is noodling as we speak.
        response.Add("inventory");
        response.Add("move");
        response.Add("color");

        return response;
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
        if (tokens.Length == 1) return "Usage: move <direction>\ndirection: designates which route you want to go down.";

        if (inEncounter /*&& !noclipped*/) return "You cannot leave in the middle of an encounter!";

        PointOfInterest destination = currentPOI.Move(tokens);
        if (destination == null) return "Cannot move in that direction.";
        currentPOI = destination;
        inEncounter = currentPOI.HasEncounter();

        CheckEncounterType(); // Check the encounter type after moving

        //Routes need to be prompted *after* description, so just wait a frame. Spaghetti code at its finest.
        if (!inEncounter) StartCoroutine(PromptRoutesNextFrame()); 
        return currentPOI.GetDescription();
    }

    /// <summary>
    /// Prompting Routes mess a lot of things up, so just wait until everything is done this frame and do the routes next frame.
    /// </summary>
    /// <returns></returns>
    IEnumerator PromptRoutesNextFrame()
    {
        yield return null;
        PromptRoutes();
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
        StartCoroutine(PromptRoutesNextFrame());
    }

    /// <summary>
    /// Prompts the player about which directions they can move.
    /// </summary>
    public void PromptRoutes()
    {
        List<Route> routes = currentPOI.GetRoutes();
        int routeCount = routes.Count;
        if (routeCount <= 0) {
            TextOutput.instance.Print(noRoutePrompt);
            return;
        }

        TextOutput.instance.Print(BuildRoutePrompt(routes, routeCount));
    }

    /// <summary>
    /// Builds the move prompt for the player.
    /// </summary>
    /// <param name="routes">List of <see cref="Route"/>s the player can go.</param>
    /// <param name="routeCount">The number of routes.</param>
    /// <returns></returns>
    private string BuildRoutePrompt(List<Route> routes, int routeCount)
    {
        ///"You encounter 2 routes. Which path do you want to go down: x, y, z?"
        routePrompt.Clear();
        routePrompt.Append(routePromptStart.TrimEnd());
        routePrompt.Append(" ");
        routePrompt.Append(routeCount);
        routePrompt.Append(" ");

        if (routeCount > 1) routePrompt.Append("routes. ");
        else routePrompt.Append("route. ");

        routePrompt.Append(routePromptEnd.TrimEnd());
        routePrompt.Append(" ");

        for (int i = 0; i < routeCount; i++)
        {
            routePrompt.Append($"<b>{routes[i].GetDirection()}</b>");
            if (i < routeCount - 1) routePrompt.Append(", ");
        }
        routePrompt.Append("?");

        return routePrompt.ToString();
    }

    /// <summary>
    /// Checks the type of encounter at the current POI and prints whether it's combat or non-combat.
    /// </summary>
    private void CheckEncounterType()
    {
        if (currentPOI == null)
        {
            Debug.LogError("GameManager: currentPOI is not assigned.");
            return;
        }

        Debug.Log("GameManager: currentPOI is assigned.");

        if (!inEncounter)
        {
            Debug.Log("GameManager: No encounter at the current POI.");
            return;
        }

        Encounter encounter = currentPOI.GetEncounter();
        if (encounter == null)
        {
            Debug.LogWarning("GameManager: Current POI has an encounter flag but no encounter object.");
            return;
        }

        Debug.Log("GameManager: Encounter retrieved successfully.");

        if (encounter.IsCombatEncounter())
        {
            Debug.Log("GameManager: Combat encounter started.");
            if (Combat.instance == null)
            {
                Debug.LogError("GameManager: Combat.instance is null. Ensure Combat is in the scene.");
                return;
            }
            Combat.instance.InitiateCombat(encounter);
        }
        else
        {
            Debug.Log("GameManager: Non-combat encounter.");
        }
    }
}
