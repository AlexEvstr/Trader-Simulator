using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LivePlaysController : MonoBehaviour
{
    [SerializeField] private Text _playText;
    [SerializeField] private Text _xText;
    [SerializeField] private Text _winText;
    private CubeManager _cubeManager;
    private BetManager _betManager;

    private void Start()
    {
        _betManager = GetComponent<BetManager>();
        _cubeManager = GetComponent<CubeManager>();
    }

    public void UpdateTexts()
    {
        _playText.text = _betManager.Bet.ToString("f2");
        _xText.text = _cubeManager.targetScaleZ.ToString("f2");
        if (_cubeManager.targetScaleZ > 1)
            _winText.text = _betManager.WinningsSum(_cubeManager.targetScaleZ).ToString("f2");
        else
            _winText.text = "0";
    }
}
