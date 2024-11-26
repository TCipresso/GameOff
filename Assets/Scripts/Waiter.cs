using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waiter is anything that wants to wait but 
/// cannot start a coroutine such as <see cref="ScriptableObject"/>.
/// The subscriber to <see cref="Wait"/>.
/// </summary>
public interface Waiter
{
    /// <summary>
    /// Behaviour for when waiting is complete.
    /// </summary>
    public void WaitComplete(); 
}
