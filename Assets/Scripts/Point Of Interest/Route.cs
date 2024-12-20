using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pairs a direction (<see cref="string"/>) to a destination (<see cref="PointOfInterest"/>).
/// </summary>
[System.Serializable]
public class Route
{
    [SerializeField] string direction = "forward";
    [SerializeField] PointOfInterest destination;
    //[SerializeField] CheatsIDunnoWhateverWeEndUpCallingThis requiredCheats;

    public Route(string direction, PointOfInterest destination)
    {
        this.direction = direction;
        this.destination = destination;
    }

    /// <summary>
    /// Returns the direction related to this route.
    /// </summary>
    /// <returns>The direction name.</returns>
    public string GetDirection()
    {
        return direction;
    }

    /// <summary>
    /// Returns the destination of this route.
    /// </summary>
    /// <returns>The <see cref="PointOfInterest"/> of this route.</returns>
    public PointOfInterest GetDestination()
    {
        return destination;
    }

    /// <summary>
    /// Check if the current game state facilitates travel to this location.
    /// </summary>
    /// <returns><see cref="bool"/> stating if travel is possible.</returns>
    public bool CanTravel()
    {
        /*False conditions*/

        return true;
    }
}


