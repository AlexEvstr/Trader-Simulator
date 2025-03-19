using UnityEngine;
using UnityEngine.UI;

public class PlayAutoSwitcher : MonoBehaviour
{
    [SerializeField] private Button PlayBtn;
    [SerializeField] private Button AutoBtn;
    [SerializeField] private GameObject _autoSettingsPanel;
    [SerializeField] private Button[] _buttonsToDisable;
    [SerializeField] private InputField _inputField;
    [SerializeField] private GameObject _playBtn;
    [SerializeField] private GameObject _cancelBtn;
    private AutoSettingsManager _autoSettingsManager;

    public int gameMode;

    private void Start()
    {
        _autoSettingsManager = GetComponent<AutoSettingsManager>();
        PlayBtn.onClick.AddListener(SwitchToPlay);
        AutoBtn.onClick.AddListener(SwitchToAuto);
        _cancelBtn.GetComponent<Button>().onClick.AddListener(CancelButton);
    }

    public void SwitchToPlay()
    {
        gameMode = 0;
        PlayBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        PlayBtn.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);

        AutoBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        AutoBtn.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.5f);
        _autoSettingsPanel.SetActive(false);

        EnableBetButtons();

        _playBtn.SetActive(true);
        _cancelBtn.SetActive(false);
    }

    private void SwitchToAuto()
    {
        gameMode = 1;
        AutoBtn.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        AutoBtn.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);

        PlayBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        PlayBtn.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.5f);

        _autoSettingsPanel.SetActive(true);

        DisableBetButtons();

        _playBtn.SetActive(false);
        _cancelBtn.SetActive(true);
        _autoSettingsManager.ResetSettings();
    }

    private void CancelButton()
    {
        SwitchToPlay();
    }

    public void DisableBetButtons()
    {
        foreach (var buttons in _buttonsToDisable)
        {
            buttons.interactable = false;
        }
        _inputField.interactable = false;
    }

    public void EnableBetButtons()
    {
        foreach (var buttons in _buttonsToDisable)
        {
            buttons.interactable = true;
        }
        _inputField.interactable = true;
    }
}