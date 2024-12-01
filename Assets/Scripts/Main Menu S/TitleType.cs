using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TitleMessage
{
    [TextArea(3, 10)]
    public string text;
    public float clearDelay = 1f;
}

public class TitleType : MonoBehaviour
{
    [Header("Typing Effect Settings")]
    public TextMeshProUGUI titleText;
    public List<TitleMessage> messages;
    public float typingSpeed = 0.05f;
    public AudioSource typingSound;

    private void Start()
    {
        if (messages.Count > 0)
        {
            StartCoroutine(TypeMessages(messages));
        }
    }

    private IEnumerator TypeMessages(List<TitleMessage> messages)
    {
        foreach (var message in messages)
        {
            titleText.text = "";
            yield return StartCoroutine(TypeMessage(message.text));
            yield return new WaitForSecondsRealtime(message.clearDelay);
            titleText.text = "";
        }
    }

    private IEnumerator TypeMessage(string message)
    {
        int visibleCharCount = 0;
        titleText.text = message;
        titleText.maxVisibleCharacters = 0;

        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] == '<')
            {
                while (i < message.Length && message[i] != '>')
                {
                    i++;
                }
            }
            else
            {
                visibleCharCount++;
                titleText.maxVisibleCharacters = visibleCharCount;

                if (typingSound != null)
                {
                    typingSound.PlayOneShot(typingSound.clip);
                }

                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }
    }
}
