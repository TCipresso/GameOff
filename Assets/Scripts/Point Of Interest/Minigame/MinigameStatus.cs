using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MinigameStatus is just a more readable way to pass the
/// info of the status of a <see cref="Minigame"/>
/// </summary>
public enum MinigameStatus
{
    LOST = -1, 
    INPROGRESS = 0, 
    WIN = 1
}
