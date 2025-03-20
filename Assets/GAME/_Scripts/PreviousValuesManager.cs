using UnityEngine;
using UnityEngine.UI;

public class PreviousValuesManager : MonoBehaviour
{
    public Text[] previousBets;
    public Text[] previousX;
    public Text[] previousWins;

    public Text previousPlayText;
    public Text previousXText;
    public Text previousWinText;

    public void UpdatePreviousValues(Text[] bets, Text[] xValues, Text[] wins, Text playText, Text xText, Text winText)
    {
        int minLength = Mathf.Min(previousBets.Length, bets.Length);

        // ✅ Заполняем доступные значения из прошлых ставок
        for (int i = 0; i < minLength; i++)
        {
            previousBets[i].text = bets[i].text;
            previousX[i].text = xValues[i].text;
            previousWins[i].text = wins[i].text;
        }

        // ✅ Если previousBets больше bets, заполняем оставшиеся случайными значениями
        for (int i = minLength; i < previousBets.Length; i++)
        {
            float randomBet = Random.Range(0.10f, 99.99f);
            previousBets[i].text = randomBet.ToString("f2");

            previousX[i].text = xText.text;

            previousWins[i].text = (randomBet * float.Parse(xText.text)).ToString("f2");
        }

        // ✅ Сохраняем предыдущие основные значения
        previousPlayText.text = playText.text;
        previousXText.text = xText.text;
        previousWinText.text = winText.text;
    }

}
