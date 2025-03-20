using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class BetManager : MonoBehaviour
{
    public Text balanceText;
    public InputField betInput;
    public Button plusButton, minusButton;
    public Button[] betButtons;

    public float balance;
    private const string BALANCE_KEY = "PlayerBalance";
    private const float minBet = 1f;
    public float Bet = 1.0f;

    public Text _winText;

    void Start()
    {
        betInput.text = "1.00";
        balance = PlayerPrefs.GetFloat(BALANCE_KEY, 100f);
        UpdateBalanceText();

        betInput.onValueChanged.AddListener(ValidateInput);

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
            betInput.text = balance.ToString("F2", CultureInfo.InvariantCulture);
        }
        Bet = bet;
    }


    public void ChangeBet(float amount)
    {
        float bet = float.Parse(betInput.text, CultureInfo.InvariantCulture);
        bet += amount;
        bet = Mathf.Clamp(bet, minBet, balance);
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
        _winText.text = winnings.ToString("f2");
    }

    public float WinningsSum(float coeff)
    {
        float bet = float.Parse(betInput.text, CultureInfo.InvariantCulture);
        float winnings = bet * coeff;
        return winnings;

    }

    private void UpdateBalanceText()
    {
        balanceText.text = balance.ToString("F2", CultureInfo.InvariantCulture);
    }
}