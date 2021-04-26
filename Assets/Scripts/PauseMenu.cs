using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    private void Start()
    {
        gameObject.SetActive(false);
        paused = false;
    }

    public void OnPause()
    {
        if (paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        gameObject.SetActive(true);
        paused = true;
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        gameObject.SetActive(false);
        paused = false;
        Time.timeScale = 1.0f;
    }

    public void QuitMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
        paused = false;
        Objective.Reset();
    }
}
