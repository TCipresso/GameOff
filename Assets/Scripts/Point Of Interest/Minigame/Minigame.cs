using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Minigame is an abstract class that represents all minigames
/// in the game. Implements <see cref="ReportHit"/>
/// </summary>
public abstract class Minigame : MonoBehaviour, ReportHit
{
    protected MinigameCaller caller;
    [SerializeField] protected bool showResultOnUI = true;
    [SerializeField] protected TimerUI timerUIElement;
    [SerializeField] protected GameObject background;
    [SerializeField] protected Colorable colorable;
    
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
        background.SetActive(true);
        MinigameResultUI.instance.HideResults();
        colorable?.UpdateColor();
    }

    /// <summary>
    /// Cleans up the minigame and tells the caller the ending <see cref="MinigameStatus"/>
    /// </summary>
    public virtual void EndMinigame()
    {
        background.SetActive(false);
    }

    /// <summary>
    /// Allows <see cref="Obstacle"/>s to report when they get hit.
    /// </summary>
    public abstract void ReportObstacleHit();
}
