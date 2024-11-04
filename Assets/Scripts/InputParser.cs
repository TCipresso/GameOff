using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// InputParser is an object to be used with text input.
/// It takes a string as an input, find the correct input 
/// handler, and then updates the state of the game.
/// </summary>
public class InputParser : MonoBehaviour
{
    [Header("Input Field")]
    [SerializeField] TMP_InputField inputField;

    [Header("Keyword Handlers")]
    [SerializeField] ColorManager colorManager;

    /// <summary>
    /// Adds a listener to input field to parse input only
    /// when return is pressed.
    /// </summary>
    private void Start()
    {
        inputField.onEndEdit.AddListener(text =>
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ParseInput(text);
                inputField.text = "";
            }
        });
    }

    /// <summary>
    /// Parse input tokenizes the input inserted by the player. It
    /// first checks if it is an input related to the scene and then
    /// checks if it is an input related to a keyword. The tokens are 
    /// then passed to the correct handler.
    /// </summary>
    /// <param name="input">The input entered in the inputField</param>
    /// <returns>An HTTP status code because I'm extra like that.</returns>
    public int ParseInput(string input)
    {
        if (input.Equals("")) return 400;

        //TODO: Check if the input is a string related to one of the scene's options.
        
        string[] tokens = input.ToLower().Split(" ");
        switch(tokens[0])
        {
            case "color":
                colorManager.ReadTokens(tokens);
                return 200;
        }
        
        return 404;
    }
}
