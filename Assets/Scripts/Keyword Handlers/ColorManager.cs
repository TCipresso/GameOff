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

    public override string ReadTokens(string[] tokens)
    {
        if (tokens.Length <= 1)
            return "Usage: color <color1> ...";

        switch(tokens[1])
        {
            case "blue":
                colorStore.narratorColor = Color.blue;
                colorStore.hudColor = Color.blue;
                break;
            default:
                return "Color not yet supported";
        }

        colorStore.UpdateColorables();
        
        return "Colors updated";
    }
}
