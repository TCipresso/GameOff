using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// InputParser is an object to be used with text input.
/// It takes a string as an input, finds the correct input 
/// handler, and then updates the state of the game.
/// </summary>
public class InputParser : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] protected TMP_InputField inputField;
    [SerializeField] protected GameObject textArea;
    [SerializeField] protected GameObject carrot;

    [Header("Keyword Handlers")]
    [SerializeField] HelpHandler helpHandler;
    [SerializeField] ColorManager colorManager;
    [SerializeField] CheatCodeManager cheatCodeManager;

    [Header("Hint Info")]
    [TextArea(3, 10)] [SerializeField] string hintText;
    [SerializeField] float hintTime;
    Coroutine giveHint;

    public static InputParser instance { get; private set; }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        giveHint = StartCoroutine(GiveHint());
    }

    IEnumerator GiveHint()
    {
        yield return new WaitForSecondsRealtime(hintTime);
        TextOutput.instance.Print(hintText);
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

    }

    /// <summary>
    /// Activates the input area.
    /// </summary>
    public virtual void ActivateInput()
    {
        giveHint = StartCoroutine(GiveHint());
        inputField.text = "";
        textArea.SetActive(true);
        carrot.SetActive(true);
    }

    /// <summary>
    /// Deactivates the input area.
    /// </summary>
    public virtual void DeactivateInput()
    {
        StopCoroutine(giveHint);
        inputField.text = "";
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
        //When adding keywords to the InputParser itself, don't forget to update the GetKeywords in GameManager.
        if (string.IsNullOrEmpty(input)) return;

        TextOutput.instance.Print($"{input}", OutputCarrot.USER); // Prefix user input with user carrot
        string output;

        if (Combat.instance.IsCombatActive())
        {
            Combat.instance.HandlePlayerInput(input);
            return;
        }

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
                case "help":
                    output = helpHandler.ReadTokens(tokens);
                    break;
                case "inventory":
                    output = PlayerInventory.instance.ReadTokens(tokens);
                    break;
                case "godmode":
                    cheatCodeManager.TryActivateCheat("GodMode");
                    output = "Godmode enabled!";
                    break;
                case "noclip":
                    cheatCodeManager.TryActivateCheat("NoClip");
                    output = "No Clip enabled!";
                    break;
                default:
                    output = "Unknown command.";
                    break;
            }
        }

        if (!string.IsNullOrEmpty(output))
        {
            StopCoroutine(giveHint);
            if(gameObject.activeSelf) giveHint = StartCoroutine(GiveHint());
            TextOutput.instance.Print(output);
        }
    }
}
