using UnityEngine;
using TMPro;

public class TSLimiter : MonoBehaviour
{
    public PlayerStats playerStats;
    private TMP_InputField inputField;
    private string currentInput = "";
    private float typingDelay;
    private float typingTimer;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        if (inputField == null) return;

        inputField.onValueChanged.AddListener(OnInputChanged);
        SetTypingSpeed();
    }

    private void Update()
    {
        typingTimer += Time.deltaTime;
    }

    private void SetTypingSpeed()
    {
        if (playerStats != null)
        {
            typingDelay = 1f / playerStats.baseTypingSpeed;
            typingTimer = typingDelay;
        }
    }

    private void OnInputChanged(string newText)
    {
        if (Input.GetKey(KeyCode.Backspace) || typingTimer >= typingDelay)
        {
            currentInput = newText;
            typingTimer = 0f;
        }
        else
        {
            inputField.text = currentInput;
            inputField.caretPosition = currentInput.Length;
        }
    }
}
