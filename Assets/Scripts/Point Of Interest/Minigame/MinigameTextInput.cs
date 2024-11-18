using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MinigameTextInput is an <see cref="InputParser"/> meant to be 
/// used by <see cref="PromptType"/> minigames.
/// </summary>
public class MinigameTextInput : InputParser
{
    [SerializeField] public PromptType promptMinigame;

    /// <summary>
    /// To stop the singleton.
    /// </summary>
    private void Awake()
    {
        return;
    }

    /// <summary>
    /// Because <see cref="InputParser"/> gets the POI's desc and
    /// MinigameTextInput shouldn't.
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
    /// Preprocesses input and then sends it to the
    /// <see cref="PromptType"/> <see cref="Minigame"/> to handle.
    /// </summary>
    /// <param name="input">The input the player entered in the inputField.</param>
    public override void ParseInput(string input)
    {
        promptMinigame.ReadTextInput(input.Split(' '));
    }
}
