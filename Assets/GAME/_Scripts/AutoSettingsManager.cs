using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class AutoSettingsManager : MonoBehaviour
{
    [Header("Number of Rounds")]
    public Button[] roundsButtons; // Кнопки 10, 20, 50, 100
    public int selectedRounds = 10;
    public int selectedOption;

    [Header("Auto Cash Out After")]
    public Button[] cashOutButtons; // Кнопки X reached, Steps, Win Amount
    public Button[] xReachedAdjustButtons; // Кнопки + и - для X reached
    public Button[] stepsAdjustButtons; // Кнопки + и - для Steps
    public Text xReachedValue, stepsValue; // Отображение значений
    public InputField winAmountInput; // Поле для Win Amount
    public float xReached = 7.0f;
    public int steps = 3;
    public float winAmount = 50.0f;

    [Header("Stop Auto Game If Profit")]
    public Button[] profitButtons; // 3 кнопки выбора
    public InputField[] profitInputs; // Поля ввода (3 шт.)

    [Header("Other")]
    public GameObject autoSettingsPanel; // Окно автоигры
    public Button startAutoButton;
    private CubeManager _cubeManager;

    void Start()
    {
        _cubeManager = GetComponent<CubeManager>();

        for (int i = 0; i < profitButtons.Length; i++)
        {
            int index = i;
            profitButtons[i].onClick.AddListener(() => ToggleProfitOption(index));
        }

        // Кнопки для изменения X reached
        xReachedAdjustButtons[0].onClick.AddListener(() => AdjustXReached(-0.1f));
        xReachedAdjustButtons[1].onClick.AddListener(() => AdjustXReached(0.1f));

        // Кнопки для изменения Steps
        stepsAdjustButtons[0].onClick.AddListener(() => AdjustSteps(-1));
        stepsAdjustButtons[1].onClick.AddListener(() => AdjustSteps(1));

        // Сбрасываем настройки в начале
        ResetSettings();
    }

    // Выбор кнопки по значению (например, roundsButtons)
    // Выбор количества раундов (работает как у тебя)
    public void SelectRounds(int value)
    {
        selectedRounds = value;

        foreach (var btn in roundsButtons)
        {
            btn.transform.GetChild(0).gameObject.SetActive(int.Parse(btn.GetComponentInChildren<Text>().text) == value);
        }
    }

    // Выбор Auto Cash Out After (X reached, Steps, Win Amount)
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

        // Включаем или выключаем кнопку Start Auto
        startAutoButton.interactable = anySelected;
    }

    // Выбор Stop Auto Game If Profit (можно выбрать несколько)
    public void ToggleProfitOption(int index)
    {
        bool isActive = profitButtons[index].transform.GetChild(0).gameObject.activeSelf;
        profitButtons[index].transform.GetChild(0).gameObject.SetActive(!isActive);

        // Проверяем выбор после изменения
        CheckProfitSelection();
    }


    // Изменение X reached
    private void AdjustXReached(float amount)
    {
        xReached = Mathf.Max(0.10f, xReached + amount);
        xReachedValue.text = xReached.ToString("F2", CultureInfo.InvariantCulture);
    }

    // Изменение Steps
    private void AdjustSteps(int amount)
    {
        steps = Mathf.Max(1, steps + amount);
        stepsValue.text = steps.ToString();
    }

    // Валидация Win Amount
    public void ValidateWinAmount()
    {
        if (string.IsNullOrEmpty(winAmountInput.text) || !float.TryParse(winAmountInput.text, NumberStyles.Float, CultureInfo.InvariantCulture, out winAmount) || winAmount < 0.10f)
        {
            winAmount = 0.10f;
        }
        winAmountInput.text = winAmount.ToString("F2", CultureInfo.InvariantCulture);
    }

    // Валидация полей Stop Auto Game If Profit
    public void ValidateProfitFields()
    {
        for (int i = 0; i < profitInputs.Length; i++)
        {
            float value;
            if (string.IsNullOrEmpty(profitInputs[i].text) ||
                !float.TryParse(profitInputs[i].text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) || value < 0.10f)
            {
                profitInputs[i].text = "0.10";
            }
        }
    }

    // Метод закрытия окна (Start Auto)
    public void StartAuto()
    {
        autoSettingsPanel.SetActive(false);
        _cubeManager.StartGame();
        selectedRounds--;
    }

    // Метод сброса всех настроек (Reset)
    public void ResetSettings()
    {
        // Number of Rounds
        SelectRounds(10);
        SelectOption(0);

        xReached = 7.0f;
        steps = 3;
        winAmount = 50.0f;
        xReachedValue.text = xReached.ToString("F2", CultureInfo.InvariantCulture);
        stepsValue.text = steps.ToString();
        winAmountInput.text = winAmount.ToString("F2", CultureInfo.InvariantCulture);

        // Stop Auto Game If Profit (только первая кнопка активна)
        for (int i = 0; i < profitButtons.Length; i++)
        {
            profitButtons[i].transform.GetChild(0).gameObject.SetActive(i == 0);
            profitInputs[i].text = "0.10";
        }
    }

    public void ValidateInputField(InputField inputField)
    {
        float value;

        // Если поле пустое или содержит некорректное значение — ставим 0.10f
        if (string.IsNullOrEmpty(inputField.text) ||
            !float.TryParse(inputField.text, System.Globalization.NumberStyles.Float,
                            System.Globalization.CultureInfo.InvariantCulture, out value) ||
            value < 0.10f)
        {
            value = 0.10f;
        }

        // Устанавливаем корректное значение обратно в InputField
        inputField.text = value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
    }


}
