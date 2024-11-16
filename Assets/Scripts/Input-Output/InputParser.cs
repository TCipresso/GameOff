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
    [Header("UI Elements")]
    [SerializeField] protected TMP_InputField inputField;
    [SerializeField] protected GameObject textArea;
    [SerializeField] protected GameObject carrot;

    [Header("Keyword Handlers")]
    [SerializeField] ColorManager colorManager;

    [SerializeField] CheatCodeManager cheatCodeManager;
    public static InputParser instance { get; private set; }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    /// <summary>
    /// Adds a listener to input field to parse input only
    /// when return is pressed. Print the text of the starting
    /// POI.
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

        TextOutput.instance.Print(GameManager.instance.GetCurrentPOIDesc(), ColorType.STORYCOLOR);
    }

    public virtual void ActivateInput()
    {
        textArea.SetActive(true);
        carrot.SetActive(true);
    }

    public virtual void DeactivateInput()
    {
        textArea.SetActive(false);
        carrot.SetActive(false);
    }

    /// <summary>
    /// Parse input tokenizes the input inserted by the player. It
    /// first checks if it is an input related to the scene and then
    /// checks if it is an input related to a keyword. The tokens are 
    /// then passed to the correct handler.
    /// </summary>
    /// <param name="input">The input entered in the inputField</param>
    public virtual void ParseInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return;

        TextOutput.instance.Print(input, ColorType.INPUTCOLOR, OutputCarrot.USER);
        string output = "";

        string[] tokens = input.ToLower().Split(" ");
        if (GameManager.instance.IsPOIKeyword(tokens))
        {
            output = GameManager.instance.ParsePOIKeyword(tokens);
        }
        else
        {
            switch (tokens[0])
            {
                case "color":
                    output = colorManager.ReadTokens(tokens);
                    break;
                case "move":
                    output = GameManager.instance.AttemptMove(tokens);
                    break;
                case "godmode":
                    cheatCodeManager.TryActivateCheat("GodMode");
                    output = "Godmode enabled!";
                    break;
                case "noclip":
                    cheatCodeManager.TryActivateCheat("NoClip");
                    output = "No Clip enabled!";
                    break;


            }
        }

        if (!string.IsNullOrEmpty(output))
        {
            TextOutput.instance.Print(output, ColorType.STORYCOLOR);
        }
    }

}
