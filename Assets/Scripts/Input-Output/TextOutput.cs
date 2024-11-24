using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum OutputCarrot
{
    NONE,
    USER,
    SYSTEM,
    QUESTION
}

public class TextOutput : MonoBehaviour
{
    public static TextOutput instance { get; private set; }

    [Header("TextMeshPro Settings")]
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private int charsPerSound = 5;

    private Queue<string> messageQueue = new Queue<string>();
    private bool isTyping = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void Print(string text, OutputCarrot outputCarrot = OutputCarrot.SYSTEM)
    {
        string prefixedText = $"{GetCarrot(outputCarrot)} {text}\n";
        messageQueue.Enqueue(prefixedText);
        if (!isTyping)
        {
            StartCoroutine(TypeText());
        }
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        int charCount = 0;
        while (messageQueue.Count > 0)
        {
            string text = messageQueue.Dequeue();
            foreach (char c in text)
            {
                textMeshProUGUI.text += c;
                charCount++;
                if (charCount % charsPerSound == 0)
                {
                    audioSource.PlayOneShot(typingSound);
                }
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
        }
        isTyping = false;
    }

    private string GetCarrot(OutputCarrot outputCarrot)
    {
        switch (outputCarrot)
        {
            case OutputCarrot.NONE: return "";
            case OutputCarrot.USER: return "/>";
            case OutputCarrot.QUESTION: return "?>";
            default: return ">";
        }
    }

    public void Clear()
    {
        messageQueue.Clear();
        textMeshProUGUI.text = "";
        StopAllCoroutines();
        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
