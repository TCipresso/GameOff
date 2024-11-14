using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour, ReportHit
{
    protected MinigameCaller caller;
    
    public virtual void StartMinigame(MinigameCaller caller)
    {
        /*if(caller == null)
        {
            Debug.LogError($"{name} Cannot start a game with a null caller");
            return;
        }*/
        this.caller = caller;
    }

    public abstract void EndMinigame();

    public abstract void Report();
}
