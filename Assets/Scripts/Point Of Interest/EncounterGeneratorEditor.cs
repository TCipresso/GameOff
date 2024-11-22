using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EncounterGenerator))]
public class EncounterGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EncounterGenerator generator = (EncounterGenerator)target;

        if(GUILayout.Button("Generate Encounters"))
        {
            generator.GenerateEncounters(generator.startingPOI);
        }
    }
}
