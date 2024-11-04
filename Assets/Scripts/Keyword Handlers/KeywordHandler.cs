using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A KeywordHanlder handles a specific keyword and 
/// updates the state of the game according to the 
/// keyword.
/// </summary>
//Abstract because you cannot serialize interfaces. Allows you to make a list of keyword handlers using the editor if needed.
public abstract class KeywordHandler : MonoBehaviour
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
