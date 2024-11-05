using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Enum to denote what type of > to prefix to output.
/// </summary>
public enum OutputCarrot { 
    /// <summary>
    /// No >
    /// </summary>
    NONE, 

    /// <summary>
    /// />
    /// </summary>
    USER, 

    /// <summary>
    /// >
    /// </summary>
    SYSTEM,
    
    /// <summary>
    /// ?>
    /// </summary>
    QUESTION 
}

/// <summary>
/// A singleton object to display output text to.
/// </summary>
public class TextOutput : MonoBehaviour
{
    public static TextOutput instance { get; private set; }
    [SerializeField] GameObject textPrefab;

    /// <summary>
    /// Singleton.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    /// <summary>
    /// Print an output to the screen.
    /// </summary>
    /// <param name="text">Text to output.</param>
    /// <param name="colorType">The <see cref="ColorType"/> to color the output.</param>
    /// <param name="outputCarrot">The <see cref="OutputCarrot"/> to be prefixed to the output.</param>
    public void Print(string text, ColorType colorType=ColorType.HUDCOLOR, OutputCarrot outputCarrot=OutputCarrot.SYSTEM)
    {
        GameObject textObject = Instantiate(textPrefab, transform);
        
        string outputString = GetCarrot(outputCarrot);
        outputString += text;
        
        textObject.GetComponent<TextMeshProUGUI>().text = outputString;
        textObject.GetComponent<Colorable>().colorType = colorType;
    }

    /// <summary>
    /// Gets the proper carrot depending on <see cref="OutputCarrot"/>
    /// </summary>
    /// <param name="outputCarrot">The <see cref="OutputCarrot"/> to get.</param>
    /// <returns>A string representing the <see cref="OutputCarrot"/></returns>
    private string GetCarrot(OutputCarrot outputCarrot)
    {
        switch (outputCarrot)
        {
            case OutputCarrot.NONE:
                return "";
            case OutputCarrot.USER:
                return "/>";
            default:
            case OutputCarrot.SYSTEM:
                return ">";
            case OutputCarrot.QUESTION:
                return "?>";
        }
    }
}
