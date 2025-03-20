using UnityEngine;
using UnityEngine.UI;

public class LivePlaysController : MonoBehaviour
{
    [SerializeField] private Text _playText;
    [SerializeField] private Text _xText;
    [SerializeField] private Text _winText;
    [SerializeField] private Text[] _randomBets;
    [SerializeField] private Text[] _randomX;
    [SerializeField] private Text[] _randomWins;
    private CubeManager _cubeManager;
    private BetManager _betManager;
    private PreviousValuesManager _previousValuesManager;

    private void Start()
    {
        _betManager = GetComponent<BetManager>();
        _cubeManager = GetComponent<CubeManager>();
        _previousValuesManager = FindObjectOfType<PreviousValuesManager>();
    }

    public void UpdateTexts()
    {
        _previousValuesManager.UpdatePreviousValues(_randomBets, _randomX, _randomWins, _playText, _xText, _winText);


        for (int i = 0; i < _randomWins.Length; i++)
        {
            float randomBet = Random.Range(0.10f, 99.99f);
            _randomBets[i].text = randomBet.ToString("f2");

            _randomX[i].text = _cubeManager.targetScaleZ.ToString("f2");

            _randomWins[i].text = (randomBet * _cubeManager.targetScaleZ).ToString("f2");
        }

        float bet = _betManager.Bet;
        if (bet == 0) bet = 1;
        _playText.text = bet.ToString("f2");        
        _xText.text = _cubeManager.targetScaleZ.ToString("f2");

        if (_cubeManager.targetScaleZ > 1)
            _winText.text = (bet * _cubeManager.targetScaleZ).ToString("f2");
        else
            _winText.text = "0.00";
    }
}