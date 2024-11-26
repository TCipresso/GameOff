using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// HelpHandler is a <see cref="KeywordHandler"/> that collects
/// the current keywords and sets them up for the player.
/// </summary>
public class HelpHandler : KeywordHandler
{
    public override string ReadTokens(string[] tokens)
    {
        List<string> keywords = GameManager.instance.GetKeywords();
        StringBuilder response = new StringBuilder("Available Keywords:\n");
        foreach (string keyword in keywords)
        {
            response.Append($"-{keyword}\n");
        }
        response.Append("For keywords that have more than one argument, enter the keyword to see its usage.");

        return response.ToString();
    }
}
