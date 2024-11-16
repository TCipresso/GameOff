using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTextInput : InputParser
{
    [SerializeField] public PromptType promptMinigame;

    private void Awake()
    {
        return;
    }

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

    public override void ActivateInput()
    {
        inputField.text = "";
        base.ActivateInput();
    }

    public override void DeactivateInput()
    {
        inputField.text = "";
        base.DeactivateInput();
    }

    public override void ParseInput(string input)
    {
        promptMinigame.ReadTextInput(input);
    }
}
