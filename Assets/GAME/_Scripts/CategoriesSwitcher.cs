using System;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesSwitcher : MonoBehaviour
{
    [SerializeField] private Button[] _categoryButtons;
    [SerializeField] private GameObject[] _categoryPanels;

    [SerializeField] private Button[] _topResultsButtons;
    [SerializeField] private GameObject[] _topResultsPanels;

    [SerializeField] private Text[] _dailyDates;
    [SerializeField] private Text[] _months;

    private void Start()
    {
        DateTime now = DateTime.Now;

        foreach (var item in _dailyDates)
        {
            item.text = now.ToString("dd.MM.yy");
        }
        foreach (var item in _months)
        {
            item.text = now.ToString("MMMM");
        }
    }

    public void OpenNewCategoryPanel(int index)
    {
        foreach (var item in _categoryPanels)
        {
            item.SetActive(false);
        }
        _categoryPanels[index].SetActive(true);
        foreach (var item in _categoryButtons)
        {
            //item.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            item.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.5f);
        }
        //_categoryButtons[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        _categoryButtons[index].GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);

    }

    public void SwitchTopResults(int index)
    {
        foreach (var item in _topResultsPanels)
        {
            item.SetActive(false);
        }
        _topResultsPanels[index].SetActive(true);
        foreach (var item in _topResultsButtons)
        {
            item.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            item.GetComponentInChildren<Text>().color = new Color(1, 1, 1, 0.5f);
        }
        _topResultsButtons[index].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        _topResultsButtons[index].GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1);
    }
}