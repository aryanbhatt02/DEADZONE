using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScripts : MonoBehaviour
{       
    public void StartNewGame()
    {

        AudioManager.instance.PlayButtonSound();

        Time.timeScale = 1f;

        SaveManager.shouldLoadGame = false;

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();               

        StartCoroutine(SceneFader.instance.StartNewGameFadeOut());
    }

    public void ExitGame()
    {        
        Application.Quit();
    }

    public void LoadGame()
    {
        AudioManager.instance.PlayButtonSound();

        SaveManager.shouldLoadGame = true;
        Time.timeScale = 1f;

        StartCoroutine(SceneFader.instance.StartNewGameFadeOut());
    }
}
