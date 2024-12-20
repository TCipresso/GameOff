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

    public void UpdateAudio(float value)
    {
        PlayerPrefs.SetFloat(mixerChannel, value);
        audioMixer.SetFloat(mixerChannel, DecimalToDb(value));
    }

    private float DecimalToDb(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(mixerChannel)) slider.value = PlayerPrefs.GetFloat(mixerChannel);
        else slider.value = slider.maxValue;
        valueCheckpoint = slider.value;

        slider.onValueChanged.AddListener((value) => 
        {
            PlayValueChangeAudio(value);
        });
    }

    private void PlayValueChangeAudio(float value)
    {
        if (Mathf.Abs(valueCheckpoint - value) >= .15)
        {
            valueCheckpoint = value;
            if (valueChangeNoise != null)
            {
                if ((bool)audioSource?.isPlaying) audioSource.Stop();
                audioSource?.PlayOneShot(valueChangeNoise);
            }
        }
    }

    public void RemovePerf()
    {
        PlayerPrefs.DeleteKey(mixerChannel);
    }
}
