using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnPlayLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayLevel2()
    {
        SceneManager.LoadScene(2);
    }
}
