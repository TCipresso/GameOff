using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EncounterSpriteManager is an aggregate of Noncombat Sprites 
/// to allow <see cref="ScriptableObject"/>s to activate sprites.
/// </summary>
public class EncounterSpriteManager : MonoBehaviour
{
    public static EncounterSpriteManager instance {  get; private set; }
    [SerializeField] GameObject[] sprites;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    /// <summary>
    /// Turn on a sprite.
    /// </summary>
    /// <param name="i">Index of Sprite wanted.</param>
    public void ActivateSprite(int i)
    {
        if (i < 0) return;
        sprites[i].SetActive(true);
    }

    /// <summary>
    /// Turn off a sprite.
    /// </summary>
    /// <param name="i">Index of Sprite wanted.</param>
    public void DeactivateSprite(int i)
    {
        if (i < 0) return;
        sprites[i].SetActive(false);
    }
}
