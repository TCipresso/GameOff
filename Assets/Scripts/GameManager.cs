using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager is the class that stores the current
/// state of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Starting POI")]
    [SerializeField] PointOfInterest currentPOI;
    public static GameManager instance { private set; get; }
    
    /// <summary>
    /// Singleton
    /// </summary>
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    /// <summary>
    /// Gets current POI description.
    /// </summary>
    /// <returns></returns>
    public string GetCurrentPOIDesc()
    {
        return currentPOI.GetDescription();
    }

    /// <summary>
    /// Gets current POI image.
    /// </summary>
    /// <returns></returns>
    public Sprite GetCurrentPOIImage()
    {
        return currentPOI.GetImage();
    }

    /// <summary>
    /// Attempts to move in the direction stated in the tokens.
    /// </summary>
    /// <param name="tokens">Tokens from the Move command.</param>
    /// <returns>The description of the new POI or a line stating move failure.</returns>
    public string AttemptMove(string[] tokens)
    {
        PointOfInterest destination = currentPOI.Move(tokens);
        if(destination == null) return "Cannot move in that direction.";
        currentPOI = destination;
        return currentPOI.GetDescription();
    }
}
