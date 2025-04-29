using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Toggle musicToggle;
    public Toggle soundToggle;

    void Start()
    {
        // Load saved settings or default to true
        musicToggle.isOn = PlayerPrefs.GetInt("Music", 1) == 1;
        soundToggle.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;

        musicToggle.onValueChanged.AddListener(OnMusicToggle);
        soundToggle.onValueChanged.AddListener(OnSoundToggle);
    }

    void OnMusicToggle(bool isOn)
    {
        Debug.Log("Music is now " + (isOn ? "ON" : "OFF"));
        PlayerPrefs.SetInt("Music", isOn ? 1 : 0);
        // Add logic to start/stop background music
        // Example: AudioManager.Instance.SetMusicEnabled(isOn);
    }

    void OnSoundToggle(bool isOn)
    {
        Debug.Log("Sound is now " + (isOn ? "ON" : "OFF"));
        PlayerPrefs.SetInt("Sound", isOn ? 1 : 0);
        // Add logic to mute/unmute sound effects
        // Example: AudioManager.Instance.SetSoundEnabled(isOn);
    }
}
