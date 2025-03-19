using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public Transform[] cubes; // Массив кубов
    public Text[] coeffText; // Массив кубов
    public Text currentcoeff; // Массив кубов
    private int currentIndex = 0; // Текущий куб
    private bool isGameOver = false; // Флаг Game Over
    private AlphaFader alphaFader;
    public GameObject _playBtn;
    public GameObject _cashOutBtn;
    private BetManager _betManager;
    private float targetScaleZ;
    public GameObject _loadCircle;
    public Text _loadText;
    private PlayAutoSwitcher _playAutoSwitcher;
    private AutoSettingsManager _autoSettingsManager;

    void Start()
    {
        _autoSettingsManager = GetComponent<AutoSettingsManager>();
        _playAutoSwitcher = GetComponent<PlayAutoSwitcher>();
        alphaFader = GetComponent<AlphaFader>();
        _betManager = GetComponent<BetManager>();
        foreach (var cube in cubes)
        {
            cube.gameObject.SetActive(false);
        }
    }

    public void EndGameAndShowWin()
    {
        _cashOutBtn.GetComponent<Button>().interactable = false;
        StopAllCoroutines();
        _betManager.CalculateWinnings(targetScaleZ);
        isGameOver = true;
        foreach (var cube in cubes)
        {
            cube.gameObject.SetActive(false);
        }
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
        foreach (var cube in cubes)
        {
            cube.gameObject.SetActive(false);
        }
        foreach (var item in coeffText)
        {
            item.text = "";
        }
        StartCoroutine(ShowLoadCircle());
    }

    private IEnumerator ShowLoadCircle()
    {
        _loadCircle.SetActive(true);
        _loadText.text = "3";
        yield return new WaitForSeconds(1.0f);
        _loadText.text = "2";
        yield return new WaitForSeconds(1.0f);
        _loadText.text = "1";
        yield return new WaitForSeconds(1.0f);
        _loadText.text = "0";
        _loadCircle.SetActive(false);


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
    }

    IEnumerator GrowCube(Transform cube)
    {
        cube.gameObject.SetActive(true); // Включаем куб
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
        
        // Проверяем, прошел ли куб порог в 1f
        if (cube.localScale.z < 1f || isGameOver == true)
        {
            isGameOver = true;
            RestartGame();
        }
        else
        {
            if (_playAutoSwitcher.gameMode == 1)
            {
                if (_autoSettingsManager.selectedOption == 0 && targetScaleZ >= _autoSettingsManager.xReached)
                {
                    Debug.Log($"targetX: {targetScaleZ}/xReached: {_autoSettingsManager.xReached}");
                    yield return new WaitForSeconds(0.5f);
                    EndGameAndShowWin();
                }
                else if (_autoSettingsManager.selectedOption == 1 && currentIndex + 1 >= _autoSettingsManager.steps)
                {
                    Debug.Log($"currentIndex: {currentIndex}/steps: {_autoSettingsManager.steps}");
                    yield return new WaitForSeconds(0.5f);
                    EndGameAndShowWin();
                }
                else if (_autoSettingsManager.selectedOption == 2 && _betManager.WinningsSum(targetScaleZ) >= _autoSettingsManager.winAmount)
                {
                    Debug.Log($"winnings: {_betManager.WinningsSum(targetScaleZ)}/winAmount: {_autoSettingsManager.winAmount}");
                    yield return new WaitForSeconds(0.5f);
                    EndGameAndShowWin();
                }
            }

            currentIndex++;
            

            if (currentIndex < cubes.Length)
            {
                alphaFader.StartFading();
                yield return new WaitForSeconds(3.0f);
                StartCoroutine(GrowCube(cubes[currentIndex])); // Запускаем следующий куб
            }
            else
            {
                isGameOver = true;
                RestartGame();
            }
        }
    }
}
