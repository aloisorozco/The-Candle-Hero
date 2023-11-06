using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    // Call whenever slider is moved
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);  
    }

    // Will only work once built, not in editor.
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
