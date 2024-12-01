using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioSlider))]
public class AudioSliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioSlider slider = (AudioSlider)target;
        base.OnInspectorGUI();

        if(GUILayout.Button("Clear Player Pref"))
        {
            slider.RemovePerf();
        }
    }
}
