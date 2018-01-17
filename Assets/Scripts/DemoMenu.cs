using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMenu : MonoBehaviour {

    // main menu
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    // pup up menu
    public void LoadNextLevel()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void LoadCheckpoint()
    {
        GameManager.Instance.ReloadCheckpoint(0);
    }

    public void ReloadScene()
    {
        GameManager.Instance.ReloadScene();
    }

    public void BackToMainMenu()
    {
        GameManager.Instance.BackToMainMenu();      
    }



}
