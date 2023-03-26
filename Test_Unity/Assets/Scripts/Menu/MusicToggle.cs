using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MusicToggle : MonoBehaviour
{
    [SerializeField] AudioMixerGroup mixer;
    [SerializeField] Toggle toggle;
    [SerializeField] Slider slider;
    bool musicPlaying;
    float musicVolume;

    private void Start()
    {
        mixer.audioMixer.GetFloat("MusicVolume", out float musicVolume);
        mixer.audioMixer.GetFloat("MasterVolume", out float masterVolume);

        toggle.isOn = musicVolume == -10 ? true : false;
        slider.value = masterVolume;
    }

    public void ToggleMusic(bool enabled)
    {
        musicPlaying = enabled;
        mixer.audioMixer.SetFloat("MusicVolume", !musicPlaying ? -80 : -10);
    }

    public void ChangeVolume(float volume)
    {
        mixer.audioMixer.SetFloat("MasterVolume", volume);
    }
}
