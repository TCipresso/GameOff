using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Minigame is an abstract class that represents all minigames
/// in the game. Implements <see cref="ReportHit"/>
/// </summary>
public abstract class Minigame : MonoBehaviour, ReportHit
{
    protected MinigameCaller caller;
    [SerializeField] protected bool showResultOnUI = true;
    
    /// <summary>
    /// Starts the minigame.
    /// </summary>
    /// <param name="caller">The <see cref="MinigameCaller"/> starting the game.</param>
    public virtual void StartMinigame(MinigameCaller caller)
    {
        if(caller == null)
        {
            Debug.LogError($"{name} Cannot start a game with a null caller");
            return;
        }
        this.caller = caller;
        MinigameResultUI.instance.HideResults();
    }

    /// <summary>
    /// Cleans up the minigame and tells the caller the ending <see cref="MinigameStatus"/>
    /// </summary>
    public abstract void EndMinigame();

    /// <summary>
    /// Allows <see cref="Obstacle"/>s to report when they get hit.
    /// </summary>
    public abstract void ReportObstacleHit();
}
