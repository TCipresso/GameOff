using UnityEngine;
using UnityEngine.UI;

public class DmgIndicator : MonoBehaviour
{
    public static DmgIndicator instance;

    [Header("Color Settings")]
    public Color baseColor; // Color when damage is at its base
    public Color modColor;  // Color when damage is modified

    [Header("Image Reference")]
    public Image indicatorImage; // Reference to the Image component

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate DmgIndicator detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //UpdateIndicatorColor();
    }

    /// <summary>
    /// Updates the color of the Image component based on the player's damage stat.
    /// </summary>
    public void UpdateIndicatorColor()
    {
        if (PlayerStats.instance == null || indicatorImage == null) return;

        // Check if the player's damage is equal to the base damage
        if (PlayerStats.instance.dmg == PlayerStats.instance.baseDmg)
        {
            indicatorImage.color = baseColor; // Set to base color
        }
        else
        {
            indicatorImage.color = modColor; // Set to modified color
        }
    }
}
