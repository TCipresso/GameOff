using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColorManager is a class that controlls the colors
/// of elements on the screen. It tells objects to update
/// their color.
/// </summary>
public class ColorManager : KeywordHandler
{
    [SerializeField] ColorStore colorStore;

    /// <summary>
    /// Reads the provided tokens and updates the colors of 
    /// the specified elements.
    /// </summary>
    /// <param name="tokens">Tokens from user input.</param>
    /// <returns>A string to be displayed to the user.</returns>
    public override string ReadTokens(string[] tokens)
    {
        string helpString = "Usage: color <color1> <optional: target>\nTargets:\nh: apply to only hud\nn: apply to only narrator";

        if (tokens.Length < 2 || tokens.Length > 3 || tokens[1].Equals("help"))
            return helpString;

        Color targetColor;
        switch(tokens[1])
        {
            case "blue":
                targetColor = Color.blue;
                break;
            case "green":
                targetColor = Color.green;
                break;
            case "red":
                targetColor = Color.red;
                break;
            case "white":
                targetColor = Color.white;
                break;
            case "yellow":
                targetColor = Color.yellow;
                break;
            case "cyan":
                targetColor = Color.cyan;
                break;
            case "magenta":
                targetColor = Color.magenta;
                break;
            default:
                return "Color not yet supported.";
        }

        if(tokens.Length < 3)
        {
            colorStore.hudColor = targetColor;
            colorStore.narratorColor = targetColor;
        }
        else
        {
            switch(tokens[2])
            {
                case "n":
                    colorStore.narratorColor = targetColor;
                    break;
                case "h":
                    colorStore.hudColor = targetColor;
                    break;
                default:
                    return "Invalid color target";
            }
        }

        colorStore.UpdateColorables();
        
        return "Successfully updated colors.";
    }
}
