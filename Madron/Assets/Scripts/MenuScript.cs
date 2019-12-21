using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public static bool IsGamePaused = false;

    public GameObject PauseMenuUI;
    public GameObject GameInfoUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameInfoUI.SetActive(true);

        Time.timeScale = 1f;

        IsGamePaused = false;
    }

    public void Restart()
    {
        Resume();
        GameManager.instance.Restart();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene("GameProcess");
    }

    public void Quit()
    {
        Application.Quit(0);
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameInfoUI.SetActive(false);

        Time.timeScale = 0f;

        IsGamePaused = true;
    }
}
