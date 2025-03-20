using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _editProfilePanel;
    [SerializeField] private GameObject _rulesPanel;

    [SerializeField] private AudioSource _soundManager;
    [SerializeField] private AudioSource _musicManager;

    [SerializeField] private GameObject _disabledSound;
    [SerializeField] private GameObject _enabledSound;
    [SerializeField] private GameObject _disabledMusic;
    [SerializeField] private GameObject _enabledMusic;

    [SerializeField] private Text[] _playerNames;
    [SerializeField] private Sprite[] _avatars;
    [SerializeField] private Image[] _playerAvatar;
    [SerializeField] private GameObject[] _imageToChoose;

    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _lose; 

    private string playerNameText;

    private void Start()
    {
        foreach (var item in _playerNames)
        {
            item.text = PlayerPrefs.GetString("SavedName", "Player");
        }

        foreach (var item in _playerAvatar)
        {
            item.sprite = _avatars[PlayerPrefs.GetInt("SavedAvatar", 0)];
        }

        _imageToChoose[PlayerPrefs.GetInt("SavedAvatar", 0)].transform.GetChild(0).gameObject.SetActive(true);

        int sound = PlayerPrefs.GetInt("SoundVolume", 1);
        if (sound == 1) EnableSounds();
        else DisableSounds();

        int music = PlayerPrefs.GetInt("MusicVolume", 1);
        if (music == 1) EnableMusic();
        else DisableMusic();
    }

    public void OpeSSettings()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void OpenEditProfile()
    {
        _editProfilePanel.SetActive(true);
    }

    public void CloseEditProfile()
    {
        _editProfilePanel.SetActive(false);
    }

    public void OpenRules()
    {
        _rulesPanel.SetActive(true);
    }

    public void CloseRules()
    {
        _rulesPanel.SetActive(false);
    }

    public void DisableSounds()
    {
        _enabledSound.SetActive(false);
        _disabledSound.SetActive(true);
        _soundManager.volume = 0;
        PlayerPrefs.SetInt("SoundVolume", 0);
    }

    public void EnableSounds()
    {
        _disabledSound.SetActive(false);
        _enabledSound.SetActive(true);
        _soundManager.volume = 1;
        PlayerPrefs.SetInt("SoundVolume", 1);
    }

    public void DisableMusic()
    {
        _enabledMusic.SetActive(false);
        _disabledMusic.SetActive(true);
        _musicManager.volume = 0;
        PlayerPrefs.SetInt("MusicVolume", 0);
    }

    public void EnableMusic()
    {
        _disabledMusic.SetActive(false);
        _enabledMusic.SetActive(true);
        _musicManager.volume = 1;
        PlayerPrefs.SetInt("MusicVolume", 1);
    }

    public void SaveName(InputField inputField)
    {
        playerNameText = inputField.text;
        PlayerPrefs.SetString("SavedName", playerNameText);
        PlayerPrefs.Save();

        foreach (var item in _playerNames)
        {
            item.text = playerNameText;
        }
    }

    public void ChoseAvatar(int index)
    {
        foreach (var item in _imageToChoose)
        {
            item.transform.GetChild(0).gameObject.SetActive(false);
        }

        _imageToChoose[index].transform.GetChild(0).gameObject.SetActive(true);

        foreach (var item in _playerAvatar)
        {
            item.sprite = _avatars[index];
        }
        PlayerPrefs.SetInt("SavedAvatar", index);
    }

    public void PlayClickSound()
    {
        _soundManager.PlayOneShot(_click);
    }

    public void PlayWinSound()
    {
        _soundManager.PlayOneShot(_win);
    }

    public void PlayLoseSound()
    {
        _soundManager.PlayOneShot(_lose);
    }
}