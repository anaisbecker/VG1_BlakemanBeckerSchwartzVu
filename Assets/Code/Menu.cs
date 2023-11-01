using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    // Outlets
    public GameObject scoreMenu;
    public TMP_Text textLevel;
    public TMP_Text[] textRankedTimes;

    void Awake()
    {
        instance = this;
        HideScoreMenu();
    }

    public void ShowScoreLevel1()
    {
        textLevel.text = "Level 1";
    }

    public void ShowScoreLevel2()
    {
        textLevel.text = "Level 2";
    }

    public void ShowScoreMenu()
    {
        ShowScoreLevel1();
        scoreMenu.SetActive(true);
    }

    public void HideScoreMenu()
    {
        scoreMenu.SetActive(false);
    }


    public void OnPlayLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayLevel2()
    {
        SceneManager.LoadScene(2);
    }
}
