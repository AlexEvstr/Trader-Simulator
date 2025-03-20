using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public Transform[] cubes;
    public Text[] coeffText;
    public Text currentcoeff;
    private int currentIndex = 0;
    private bool isGameOver = false;
    private AlphaFader alphaFader;
    public GameObject _playBtn;
    public GameObject _cashOutBtn;
    private BetManager _betManager;
    public float targetScaleZ;
    public GameObject _loadCircle;
    public Text _loadText;
    private PlayAutoSwitcher _playAutoSwitcher;
    private AutoSettingsManager _autoSettingsManager;
    private LivePlaysController _livePlaysController;
    public Button CancelButton;

    void Start()
    {
        _livePlaysController = GetComponent<LivePlaysController>();
        _autoSettingsManager = GetComponent<AutoSettingsManager>();
        _playAutoSwitcher = GetComponent<PlayAutoSwitcher>();
        alphaFader = GetComponent<AlphaFader>();
        _betManager = GetComponent<BetManager>();
        DisableCubesAndTetxs();
        _livePlaysController.UpdateTexts();
        CancelButton.interactable = false;
    }

    public void DisableCubesAndTetxs()
    {
        foreach (var cube in cubes)
        {
            cube.gameObject.SetActive(false);
        }
        foreach (var item in coeffText)
        {
            item.text = "";
        }
    }

    public void EndGameAndShowWin()
    {
        _cashOutBtn.GetComponent<Button>().interactable = false;
        StopAllCoroutines();
        _betManager.CalculateWinnings(targetScaleZ);

        if (_playAutoSwitcher.gameMode == 1)
        {
            if (_autoSettingsManager.marks[0])
            {
                if (_betManager.balance - _playAutoSwitcher.LastBalance >= _autoSettingsManager.profitIncrease)
                {                    
                    _playAutoSwitcher.SwitchToPlay();
                }
            }
            
            if (_autoSettingsManager.marks[2])
            {
                if (_betManager.WinningsSum(targetScaleZ) >= _autoSettingsManager.profitSingleWin)
                {                    
                    _playAutoSwitcher.SwitchToPlay();
                }
            }
        }

        isGameOver = true;
        DisableCubesAndTetxs();
        StartCoroutine(ShowLoadCircle());
    }

    private void RestartGame()
    {
        StartCoroutine(WaitBeforeRestart());
    }

    private IEnumerator WaitBeforeRestart()
    {
        _cashOutBtn.GetComponent<Button>().interactable = false;
        _playAutoSwitcher.EnableBetButtons();
        yield return new WaitForSeconds(1.0f);
        StopAllCoroutines();
        isGameOver = true;
        DisableCubesAndTetxs();
        StartCoroutine(ShowLoadCircle());
    }

    private IEnumerator ShowLoadCircle()
    {
        CancelButton.interactable = true;
        _livePlaysController.UpdateTexts();
        _loadCircle.SetActive(true);
        _loadText.text = "3";
        yield return new WaitForSeconds(1.0f);
        _loadText.text = "2";
        yield return new WaitForSeconds(1.0f);
        _loadText.text = "1";
        yield return new WaitForSeconds(1.0f);
        _loadText.text = "0";
        _loadCircle.SetActive(false);
        CancelButton.interactable = false;

        if (_playAutoSwitcher.gameMode == 0)
        {
            _cashOutBtn.SetActive(false);
            _playBtn.SetActive(true);
        }
        else
        {
            if (_autoSettingsManager.selectedRounds > 0)
            {
                _autoSettingsManager.selectedRounds--;
                _playAutoSwitcher.DisableBetButtons();
                yield return new WaitForSeconds(0.5f);
                StartGame();
            }
            else
            {
                _playAutoSwitcher.SwitchToPlay();
                _cashOutBtn.SetActive(false);
            }
            
        }
    }

    public void StartGame()
    {
        if (cubes.Length > 0)
        {
            _betManager.TryPlaceBet();
            currentIndex = 0;
            isGameOver = false;
            StartCoroutine(GrowCube(cubes[currentIndex]));
            _playBtn.SetActive(false);
            _cashOutBtn.SetActive(true);
            _cashOutBtn.GetComponent<Button>().interactable = true;
        }
        if (_playAutoSwitcher.gameMode == 1)
        {
            _cashOutBtn.SetActive(false);
            _cashOutBtn.GetComponent<Button>().interactable = false;
        }
    }

    IEnumerator GrowCube(Transform cube)
    {
        cube.gameObject.SetActive(true);
        float growthDuration = 0.5f;
        float elapsedTime = 0f;

        targetScaleZ = (Random.value < 0.35f) ? Random.Range(0.1f, 0.99f) : Random.Range(1.01f, 10f);
        currentcoeff.text = targetScaleZ.ToString("f2") + "x";
        coeffText[currentIndex].text = targetScaleZ.ToString("f2");
        while (elapsedTime < growthDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / growthDuration);
            float newScaleZ = Mathf.Lerp(0, targetScaleZ, progress);
            cube.localScale = new Vector3(cube.localScale.x, cube.localScale.y, newScaleZ);
            yield return null;
        }
        
        if (cube.localScale.z < 1f || isGameOver == true)
        {
            isGameOver = true;
            if (_autoSettingsManager.marks[1])
            {
                if (_playAutoSwitcher.LastBalance - _betManager.balance >= _autoSettingsManager.profitDecrease)
                {
                    _playAutoSwitcher.SwitchToPlay();
                    DisableCubesAndTetxs();
                }
                else
                {
                    RestartGame();
                }
            }
            else
            {
                RestartGame();
            }
        }
        else
        {
            if (_playAutoSwitcher.gameMode == 1)
            {
                if (_autoSettingsManager.selectedOption == 0 && targetScaleZ >= _autoSettingsManager.xReached)
                {
                    yield return new WaitForSeconds(0.5f);
                    EndGameAndShowWin();
                }
                else if (_autoSettingsManager.selectedOption == 1 && currentIndex + 1 >= _autoSettingsManager.steps)
                {                    
                    yield return new WaitForSeconds(0.5f);
                    EndGameAndShowWin();
                }
                else if (_autoSettingsManager.selectedOption == 2 && _betManager.WinningsSum(targetScaleZ) >= _autoSettingsManager.winAmount)
                {                    
                    yield return new WaitForSeconds(0.5f);
                    EndGameAndShowWin();
                }
            }

            currentIndex++;
            

            if (currentIndex < cubes.Length)
            {
                alphaFader.StartFading();
                yield return new WaitForSeconds(3.0f);
                StartCoroutine(GrowCube(cubes[currentIndex]));
            }
            else
            {
                isGameOver = true;
                RestartGame();
            }
        }
    }
}