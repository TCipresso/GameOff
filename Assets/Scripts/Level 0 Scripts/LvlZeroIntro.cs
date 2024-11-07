using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Message
{
    [TextArea(3, 10)]
    public string text;
    public float clearDelay = 1f;
}

public class LvlZeroIntro : MonoBehaviour
{
    [Header("Typing Effect Settings")]
    public TextMeshProUGUI dialogueText;
    public List<Message> introMessages;
    public List<Message> mainMessages;
    public float typingSpeed = 0.05f;
    public AudioSource typingSound;
    public TMP_InputField playerInputField;

    [Header("Player Name Settings")]
    public Color playerNameColor = Color.yellow; // Set the color in the inspector

    private int currentMessageIndex = 0;
    private bool waitingForPlayerInput = false;

    private void Start()
    {
        playerInputField.gameObject.SetActive(false);
        dialogueText.text = "";

        if (introMessages.Count > 0)
        {
            StartCoroutine(TypeMessages(introMessages, StartPlayerInput));
        }

        playerInputField.onEndEdit.AddListener(delegate { OnSubmitName(); });
    }

    private void StartPlayerInput()
    {
        playerInputField.gameObject.SetActive(true);
        waitingForPlayerInput = true;
    }

    public void OnSubmitName()
    {
        if (waitingForPlayerInput && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(playerInputField.text))
        {
            PlayerPrefs.SetString("PlayerName", playerInputField.text);
            playerInputField.gameObject.SetActive(false);
            waitingForPlayerInput = false;

            dialogueText.text = "";

            StartCoroutine(TypeMessages(mainMessages, null));
        }
    }

    private IEnumerator TypeMessages(List<Message> messages, System.Action onFinish)
    {
        currentMessageIndex = 0;

        while (currentMessageIndex < messages.Count)
        {
            dialogueText.text = "";
            string processedMessage = ReplacePlayerNameWithColor(messages[currentMessageIndex].text);
            yield return StartCoroutine(TypeMessage(processedMessage));

            yield return new WaitForSecondsRealtime(messages[currentMessageIndex].clearDelay);
            dialogueText.text = "";

            currentMessageIndex++;
        }

        onFinish?.Invoke();
    }

    private string ReplacePlayerNameWithColor(string message)
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        string colorHex = ColorUtility.ToHtmlStringRGB(playerNameColor);
        return message.Replace("{PlayerName}", $"<color=#{colorHex}>{playerName}</color>");
    }

    private IEnumerator TypeMessage(string message)
    {
        int charIndex = 0;

        // Set the entire message with color tags in the dialogueText
        dialogueText.text = message;
        dialogueText.maxVisibleCharacters = 0; // Initially, no characters are visible

        // Reveal one character at a time
        while (dialogueText.maxVisibleCharacters < message.Length)
        {
            dialogueText.maxVisibleCharacters++;

            // Play typing sound every other character
            if (typingSound != null && charIndex % 2 == 0)
            {
                typingSound.Stop();
                typingSound.Play();
            }

            charIndex++;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}
