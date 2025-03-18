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

    void Start()
    {
        alphaFader = GetComponent<AlphaFader>();
        foreach (var cube in cubes)
        {
            cube.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (cubes.Length > 0)
        {
            currentIndex = 0;
            isGameOver = false;
            StartCoroutine(GrowCube(cubes[currentIndex]));
        }
    }

    IEnumerator GrowCube(Transform cube)
    {
        cube.gameObject.SetActive(true); // Включаем куб
        float growthDuration = 0.5f;
        float elapsedTime = 0f;

        // 75% шанс получить z > 1f, 25% шанс получить z < 1f
        float targetScaleZ = (Random.value < 0.25f) ? Random.Range(0.1f, 0.99f) : Random.Range(1.01f, 10f);
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
        if (cube.localScale.z < 1f)
        {
            isGameOver = true;
            Debug.Log("Game Over!");
        }
        else
        {
            currentIndex++;
            if (currentIndex < cubes.Length)
            {
                alphaFader.StartFading();
                yield return new WaitForSeconds(3.0f);
                StartCoroutine(GrowCube(cubes[currentIndex])); // Запускаем следующий куб
            }
            else
            {
                Debug.Log("Victory! Все кубы успешно выросли.");
            }
        }
    }
}
