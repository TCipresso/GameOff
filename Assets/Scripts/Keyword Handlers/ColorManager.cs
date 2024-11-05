using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (tokens.Length < 2 || tokens.Length > 3 || tokens[1].Equals("help"))
            return "Usage: color <color1> <optional: target>\nTargets:\nh: apply to only hud\nn: apply to only narrator\ni: applies to your inputs\ns: applies to descriptive texts.";

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
            for(int i = 0; i < colorStore.colors.Length; i++)
            {
                colorStore.colors[i] = targetColor;
            }
        }
        else
        {
            int target = 0;
            switch(tokens[2])
            {
                case "h":
                    target = 0;
                    break;
                case "n":
                    target = 1;
                    break;
                case "i":
                    target = 2;
                    break;
                case "s":
                    target = 3;
                    break;
                default:
                    return "Invalid color target";
            }
            colorStore.colors[target] = targetColor;
        }

        colorStore.UpdateColorables();
        
        return "Successfully updated colors.";
    }
}
