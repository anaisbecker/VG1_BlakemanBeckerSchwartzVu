using System;
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
        UpdateScoreboard(1);
    }

    public void ShowScoreLevel2()
    {
        UpdateScoreboard(2);
    }

    private void UpdateScoreboard(int level)
    {
        textLevel.text = "Level " + level;

        // Load Scores from storage
        List<string> scores = LoadScores(level);

        // Display Scores
        for (int i = 0; i < textRankedTimes.Length; i++)
        {
            if (scores[i] != "0")
            {
                // Parse scores from float
                TimeSpan time = TimeSpan.FromSeconds(float.Parse(scores[i]));
                textRankedTimes[i].text = time.ToString("mm':'ss'.'ff");
            }
            else
            {
                textRankedTimes[i].text = "-";
            }
        }
    }

    private List<string> LoadScores(int level)
    {
        // Load Default scores
        if (!PlayerPrefs.HasKey("Level_" + level))
        {
            List<string> initialScores = new List<string>();
            for (int i = 0; i < textRankedTimes.Length; i++)
            {
                initialScores.Add("0");
            }
            return initialScores;
        }

        string scoreString = PlayerPrefs.GetString("Level_" + level);
        List<string> scores = new List<string>(scoreString.Split(','));

        // Sort ascending, ignoring default 0 scores
        scores.Sort((a, b) =>
        {
            if (a == "0" && b == "0")
            {
                return 0; // Both are 0, no change in order
            }
            else if (a == "0")
            {
                return 1; // x is 0, so y comes before x
            }
            else if (b == "0")
            {
                return -1; // y is 0, so x comes before y
            }
            return float.Parse(a).CompareTo(float.Parse(b));

        });


        return scores;
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
