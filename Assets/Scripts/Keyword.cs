using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Keyword is any word that takes specific
/// inputs to update the state of the game.
/// </summary>
public abstract class Keyword : MonoBehaviour
{
    /// <summary>
    /// Takes in a series of tokens. Each implementation of 
    /// Keyword handles their tokens differently depending 
    /// on the purpose of the keyword.
    /// </summary>
    /// <param name="tokens">An array of strings to be read by the Keyword.</param>
    /// <returns>A string stating the result of the keyword.</returns>
    public abstract string ReadTokens(string[] tokens);
}
