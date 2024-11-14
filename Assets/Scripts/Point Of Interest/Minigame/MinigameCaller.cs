using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MinigameCaller is a subscriber pattern and is to be used 
/// by objects that want to start <see cref="Minigame"/>s.
/// </summary>
public interface MinigameCaller
{
    /// <summary>
    /// Logic to start a <see cref="Minigame"/>
    /// </summary>
    public void StartMinigame();
    /// <summary>
    /// Handle the result from a <see cref="Minigame"/>.
    /// </summary>
    /// <param name="gameResult">The resulting <see cref="MinigameStatus"/> from the <see cref="Minigame"/> I started.</param>
    public void CompleteMinigame(MinigameStatus gameResult);
}
