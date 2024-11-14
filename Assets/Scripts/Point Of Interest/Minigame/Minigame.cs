using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    private MinigameCaller caller;
    
    public virtual void StartMinigame(MinigameCaller caller)
    {
        this.caller = caller;
    }

    public abstract void EndMinigame();
}
