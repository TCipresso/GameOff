using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;

[CustomEditor(typeof(MinigameTester))]
public class MinigameTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MinigameTester tester = (MinigameTester)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Test Minigame"))
        {
            tester.StartMinigame();
        }
    }
}
