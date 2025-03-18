using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class BetManager : MonoBehaviour
{
    public Text balanceText; // Текст с балансом
    public InputField betInput; // Поле для ввода ставки
    public Button plusButton, minusButton; // Кнопки + и -
    public Button[] betButtons; // Кнопки 1.0, 2.0, 5.0, 10.0

    private float balance;
    private const string BALANCE_KEY = "PlayerBalance"; // Ключ для хранения баланса
    private const float minBet = 1f; // Минимальная ставка

    void Start()
    {
        betInput.text = "1.00";
        // Загружаем баланс или устанавливаем 100 по умолчанию
        balance = PlayerPrefs.GetFloat(BALANCE_KEY, 100f);
        UpdateBalanceText();

        // Ограничиваем ввод только цифрами и точкой
        betInput.onValueChanged.AddListener(ValidateInput);

        // Добавляем обработчики кнопок + и -
        plusButton.onClick.AddListener(() => ChangeBet(1f));
        minusButton.onClick.AddListener(() => ChangeBet(-1f));
    }

    private void ValidateInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return;

        float bet;
        if (!float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out bet) || bet < minBet)
        {
            betInput.text = minBet.ToString("F2", CultureInfo.InvariantCulture);
        }
        else if (bet > balance)
        {
            betInput.text = balance.ToString("F2", CultureInfo.InvariantCulture); // Если ставка больше баланса, ставим баланс
        }
    }


    public void ChangeBet(float amount)
    {
        float bet = float.Parse(betInput.text, CultureInfo.InvariantCulture);
        bet += amount;
        bet = Mathf.Clamp(bet, minBet, balance); // Не меньше 1 и не больше баланса
        betInput.text = bet.ToString("F2", CultureInfo.InvariantCulture);
    }

    public void SetBet(float amount)
    {
        betInput.text = Mathf.Clamp(amount, minBet, balance).ToString("F2", CultureInfo.InvariantCulture);
    }

    public void TryPlaceBet()
    {
        PlaceBet();
    }


    public bool PlaceBet()
    {
        if (string.IsNullOrEmpty(betInput.text)) betInput.text = minBet.ToString("F2", CultureInfo.InvariantCulture);

        float bet;
        if (!float.TryParse(betInput.text, NumberStyles.Float, CultureInfo.InvariantCulture, out bet) || bet < minBet)
        {
            Debug.Log("Ошибка: Минимальная ставка 1.00");
            return false;
        }

        if (bet > balance)
        {
            Debug.Log("Ошибка: Недостаточно монет!");
            return false;
        }

        balance -= bet;
        UpdateBalanceText();
        PlayerPrefs.SetFloat(BALANCE_KEY, balance);
        PlayerPrefs.Save();
        return true;
    }

    public void CalculateWinnings(float coeff)
    {
        float bet = float.Parse(betInput.text, CultureInfo.InvariantCulture);
        float winnings = bet * coeff;
        balance += winnings;
        UpdateBalanceText();
        PlayerPrefs.SetFloat(BALANCE_KEY, balance);
        PlayerPrefs.Save();
    }

    private void UpdateBalanceText()
    {
        balanceText.text = balance.ToString("F2", CultureInfo.InvariantCulture);
    }
}
