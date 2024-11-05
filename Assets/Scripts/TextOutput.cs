using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum OutputCarrot { NONE, USER, SYSTEM, QUESTION }

public class TextOutput : MonoBehaviour
{
    public static TextOutput instance { get; private set; }
    [SerializeField] GameObject textPrefab;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    public void Print(string text, ColorType colorType=ColorType.HUDColor, OutputCarrot outputCarrot=OutputCarrot.SYSTEM)
    {
        GameObject textObject = Instantiate(textPrefab, transform);
        
        string outputString;
        switch(outputCarrot)
        {
            case OutputCarrot.NONE:
                outputString = "";
                break;
            case OutputCarrot.USER:
                outputString = "/>";
                break;
            default: 
            case OutputCarrot.SYSTEM:
                outputString = ">";
                break;
            case OutputCarrot.QUESTION:
                outputString = "?>";
                break;
        }
        outputString += text;
        
        textObject.GetComponent<TextMeshProUGUI>().text = outputString;
        textObject.GetComponent<Colorable>().colorType = colorType;
    }
}
