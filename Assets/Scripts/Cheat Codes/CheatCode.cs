using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Cheat Code")]
public class CheatCode : ScriptableObject
{
    public string cheatName;
    public string[] alternateSpellings;
    public int maxUses;
    public GameObject MiniGame;
}
