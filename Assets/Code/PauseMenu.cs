using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool isMuted;


    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public Sprite mutedImage;
    public Sprite soundImage;
    public Button button;

    public void MuteSound()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        if (isMuted)
        {
            button.image.sprite = mutedImage;
        }
        else
        {
            button.image.sprite = soundImage;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
