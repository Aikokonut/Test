using UnityEngine;
using UnityEngine.UI;

public class ProfileSettings : MonoBehaviour
{
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private Dropdown avatarDropdown;
    [SerializeField] private Toggle soundToggle;

    private void Start()
    {
        // Load saved player data from PlayerPrefs or Firebase
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
        AuthManager.Instance.SavePlayerData(playerNameInput.text, avatarDropdown.value/*, soundToggle.isOn*/);
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        // Handle sound toggling
        AudioListener.volume = isOn ? 1f : 0f;
    }
}
