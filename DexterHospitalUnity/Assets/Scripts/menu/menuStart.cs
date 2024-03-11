using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuStart : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void StartScene()
    {
        SceneManager.LoadScene("Ò»Â¥ÔÓÎï¼ä");
    }
}
