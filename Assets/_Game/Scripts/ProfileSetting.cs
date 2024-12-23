using UnityEngine;
using UnityEngine.UI;

public class ProfileSettings : MonoBehaviour
{
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Dropdown avatarDropdown;
    [SerializeField] private Toggle soundToggle;

    private void Start()
    {
        // Load saved player data from PlayerPrefs
        playerNameInput.text = PlayerPrefs.GetString("PlayerName", "Player");
        avatarDropdown.value = PlayerPrefs.GetInt("AvatarIndex", 0);
        soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;

        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
    }

    public void OnSaveSettings()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        PlayerPrefs.SetInt("AvatarIndex", avatarDropdown.value);
        PlayerPrefs.SetInt("SoundEnabled", soundToggle.isOn ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        // Handle sound toggling
        if (isOn)
        {
            // Enable sound
            AudioListener.volume = 1f;
        }
        else
        {
            // Mute sound
            AudioListener.volume = 0f;
        }
    }
}
