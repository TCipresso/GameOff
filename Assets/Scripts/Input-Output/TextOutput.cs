using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
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

public struct OutputString
{
    public string text;
    public OutputCarrot outputCarrot;
    public ColorType colorType;

    public OutputString(string text, OutputCarrot outputCarrot=OutputCarrot.SYSTEM, ColorType colorType = ColorType.HUDCOLOR)
    {
        this.text = text;
        this.outputCarrot = outputCarrot;
        this.colorType = colorType;
    }
}

/// <summary>
/// A singleton object to display output text to.
/// </summary>
public class TextOutput : MonoBehaviour
{
    public static TextOutput instance { get; private set; }
    [SerializeField] ColorStore colorStore;
    private List<OutputString> outputs = new List<OutputString>(20);
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

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
        OutputString newOutput = new OutputString(text, outputCarrot, colorType);
        if(outputs.Count == outputs.Capacity) outputs.RemoveAt(0);
        outputs.Add(newOutput);
        
        StringBuilder output = new StringBuilder();
        foreach (OutputString outputString in outputs)
        {
            Color32 outputColor = colorStore.GetColor(outputString.colorType);
            output.Append(
                $"<color=#{outputColor.r.ToString("x2")}{outputColor.g.ToString("x2")}{outputColor.b.ToString("x2")}{outputColor.a.ToString("x2")}>" +
                $"{GetCarrot(outputString.outputCarrot)} {outputString.text}</color>\n");
        }

        textMeshProUGUI.text = output.ToString();
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
