using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class AutoSettingsManager : MonoBehaviour
{
    [Header("Number of Rounds")]
    public Button[] roundsButtons;
    public int selectedRounds = 10;
    public int selectedOption;

    [Header("Auto Cash Out After")]
    public Button[] cashOutButtons;
    public Button[] xReachedAdjustButtons;
    public Button[] stepsAdjustButtons;
    public Text xReachedValue, stepsValue;
    public InputField winAmountInput;
    public float xReached = 7.0f;
    public int steps = 3;
    public float winAmount = 50.0f;

    [Header("Stop Auto Game If Profit")]
    public Button[] profitButtons;
    public InputField[] profitInputs;

    public float profitIncrease = 25.0f;
    public float profitDecrease = 25.0f;
    public float profitSingleWin = 25.0f;

    [Header("Other")]
    public GameObject autoSettingsPanel;
    public Button startAutoButton;
    private CubeManager _cubeManager;
    public bool[] marks;

    void Start()
    {
        marks = new bool[] { true, false, false };

        _cubeManager = GetComponent<CubeManager>();

        for (int i = 0; i < profitButtons.Length; i++)
        {
            int index = i;
            profitButtons[i].onClick.AddListener(() => ToggleProfitOption(index));
        }

        xReachedAdjustButtons[0].onClick.AddListener(() => AdjustXReached(-0.1f));
        xReachedAdjustButtons[1].onClick.AddListener(() => AdjustXReached(0.1f));

        stepsAdjustButtons[0].onClick.AddListener(() => AdjustSteps(-1));
        stepsAdjustButtons[1].onClick.AddListener(() => AdjustSteps(1));

        ResetSettings();
    }

    public void SelectRounds(int value)
    {
        selectedRounds = value;

        foreach (var btn in roundsButtons)
        {
            btn.transform.GetChild(0).gameObject.SetActive(int.Parse(btn.GetComponentInChildren<Text>().text) == value);
        }
    }

    public void SelectOption(int index)
    {
        selectedOption = index;
        for (int i = 0; i < cashOutButtons.Length; i++)
        {
            cashOutButtons[i].transform.GetChild(0).gameObject.SetActive(i == index);
        }
    }

    public void CheckProfitSelection()
    {
        bool anySelected = false;

        foreach (var btn in profitButtons)
        {
            if (btn.transform.GetChild(0).gameObject.activeSelf)
            {
                anySelected = true;
                break;
            }
        }

        startAutoButton.interactable = anySelected;
    }

    public void ToggleProfitOption(int index)
    {
        bool isActive = profitButtons[index].transform.GetChild(0).gameObject.activeSelf;
        profitButtons[index].transform.GetChild(0).gameObject.SetActive(!isActive);

        marks[index] = !isActive;

        CheckProfitSelection();
    }

    private void AdjustXReached(float amount)
    {
        xReached = Mathf.Max(0.10f, xReached + amount);
        xReachedValue.text = xReached.ToString("F2", CultureInfo.InvariantCulture);
    }

    private void AdjustSteps(int amount)
    {
        steps = Mathf.Max(1, steps + amount);
        stepsValue.text = steps.ToString();
    }

    public void ValidateWinAmount()
    {
        if (string.IsNullOrEmpty(winAmountInput.text) || !float.TryParse(winAmountInput.text, NumberStyles.Float, CultureInfo.InvariantCulture, out winAmount) || winAmount < 0.10f)
        {
            winAmount = 50.0f;
        }
        winAmountInput.text = winAmount.ToString("F2", CultureInfo.InvariantCulture);
    }

    public void ValidateProfitInputs()
    {
        float value;

        if (string.IsNullOrEmpty(profitInputs[0].text) ||
            !float.TryParse(profitInputs[0].text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) || value < 0.10f)
        {
            profitIncrease = 25.0f;
        }
        else
        {
            profitIncrease = value;
        }

        if (string.IsNullOrEmpty(profitInputs[1].text) ||
            !float.TryParse(profitInputs[1].text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) || value < 0.10f)
        {
            profitDecrease = 25.0f;
        }
        else
        {
            profitDecrease = value;
        }

        if (string.IsNullOrEmpty(profitInputs[2].text) ||
            !float.TryParse(profitInputs[2].text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) || value < 0.10f)
        {
            profitSingleWin = 25.0f;
        }
        else
        {
            profitSingleWin = value;
        }

        profitInputs[0].text = profitIncrease.ToString("F2", CultureInfo.InvariantCulture);
        profitInputs[1].text = profitDecrease.ToString("F2", CultureInfo.InvariantCulture);
        profitInputs[2].text = profitSingleWin.ToString("F2", CultureInfo.InvariantCulture);
    }

    public void StartAuto()
    {
        autoSettingsPanel.SetActive(false);
        _cubeManager.StartGame();
        selectedRounds--;
    }

    public void ResetSettings()
    {
        SelectRounds(10);
        SelectOption(0);

        xReached = 7.0f;
        steps = 3;
        winAmount = 50.0f;
        xReachedValue.text = xReached.ToString("F2", CultureInfo.InvariantCulture);
        stepsValue.text = steps.ToString();
        winAmountInput.text = winAmount.ToString("F2", CultureInfo.InvariantCulture);

        for (int i = 0; i < profitButtons.Length; i++)
        {
            profitButtons[i].transform.GetChild(0).gameObject.SetActive(i == 0);
            profitInputs[i].text = "25.00";
        }
    }

    public void ValidateInputField(InputField inputField)
    {
        float value;

        if (string.IsNullOrEmpty(inputField.text) ||
            !float.TryParse(inputField.text, System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out value) ||
            value < 0.10f)
        {
            value = 0.10f;
        }

        inputField.text = value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
    }
}