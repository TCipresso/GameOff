using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider slider;
    [SerializeField] string mixerChannel;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip valueChangeNoise;
    private float valueCheckpoint;

    private void Awake()
    {
        slider.onValueChanged.AddListener((value) =>
        {
            UpdateAudio(value);
        });
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(mixerChannel)) slider.value = PlayerPrefs.GetFloat(mixerChannel);
        else slider.value = slider.maxValue;
        valueCheckpoint = slider.value;

        slider.onValueChanged.AddListener((value) => 
        {
            if(Mathf.Abs(valueCheckpoint - value) >= .1)
            {
                valueCheckpoint = value;
                if(valueChangeNoise != null) audioSource?.PlayOneShot(valueChangeNoise);
            }
        });
    }

    public void UpdateAudio(float value)
    {
        PlayerPrefs.SetFloat(mixerChannel, value);
        audioMixer.SetFloat(mixerChannel, DecimalToDb(value));
        Debug.Log(value);
    }

    private float DecimalToDb(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
    }

    public void RemovePerf()
    {
        PlayerPrefs.DeleteKey(mixerChannel);
    }
}
