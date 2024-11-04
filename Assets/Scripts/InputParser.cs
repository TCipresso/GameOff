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

    private void Start()
    {
        inputField.onEndEdit.AddListener(text =>
        {
            if(Input.GetKeyDown(KeyCode.Return))
                ParseInput(text);
        });
    }

    public int ParseInput(string input)
    {
        Debug.Log(input);
        return 200;
    }
}
