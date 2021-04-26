using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuContainer;
    public GameObject LevelSelect;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void StartStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void StartStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void SelectScene()
    {
        LevelSelect.SetActive(true);
        MainMenuContainer.SetActive(false);
    }
    

    public void RestoreMainMenu()
    {
        MainMenuContainer.SetActive(true);
        LevelSelect.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
