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
        if (tokens.Length < 2 || tokens.Length > 3 || tokens[1].Equals("help"))
            return "Usage: color <color1> <optional: target>\nTargets:\nh: apply to only hud\nn: apply to only narrator\ni: applies to your inputs\ns: applies to descriptive texts.";

        Color targetColor;
        try {
            targetColor = DetermineColor(tokens[1]);
        } catch (System.InvalidOperationException e) {
            return e.Message;
        }

        if (tokens.Length < 3) //No target.
        {
            for (int i = 0; i < colorStore.colors.Length; i++)
            {
                colorStore.colors[i] = targetColor;
            }
        }
        else //Targeted.
        {
            int target;
            try {
                target = DetermineTarget(tokens[2]);
            } catch (System.InvalidOperationException e) {
                return e.Message;
            }
            colorStore.colors[target] = targetColor;
        }

        colorStore.UpdateColorables();
        return "Successfully updated colors.";
    }

    /// <summary>
    /// Determines the color that is represented by the string.
    /// </summary>
    /// <param name="color">The name of the color wanted.</param>
    /// <returns>The <see cref="Color"/> represented by the string.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when a color is not supported.</exception>
    private Color DetermineColor(string color)
    {
        switch (color)
        {
            case "blue":
                return Color.blue;
            case "green":
                return Color.green;
            case "red":
                return Color.red;
            case "white":
                return Color.white;
            case "yellow":
                return Color.yellow;
            case "cyan":
                return Color.cyan;
            case "magenta":
                return Color.magenta;
            default:
                throw new System.InvalidOperationException("Provided color is not supported.");
        }
    }

    /// <summary>
    /// Determines the target that is represented by the string.
    /// </summary>
    /// <param name="target">The representation of target wanted.</param>
    /// <returns>The corresponding color index of the target.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when a target is not supported.</exception>
    private int DetermineTarget(string target)
    {
        switch (target)
        {
            case "h":
                return 0;
            case "n":
                return 1;
            case "i":
                return 2;
            case "s":
                return 3;
            default:
                throw new System.InvalidOperationException("Provide target is not supported.");
        }
    }
}
