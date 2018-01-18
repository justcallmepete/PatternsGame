using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMenu : MonoBehaviour {

    // main menu
    public void PlayGame()
    {
        SceneManager.LoadScene("DemoLevel_1");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    // pup up menu
    public void LoadNextLevel()
    {
        print("test button1");
        return;
        GameManager.Instance.LoadNextScene();
    }

    public void LoadCheckpoint()
    {
        print("test button2");
        return;

        GameManager.Instance.ReloadCheckpoint(0f);
    }

    public void ReloadScene()
    {
        print("test button3");
        return;

        GameManager.Instance.ReloadScene();
    }

    public void BackToMainMenu()
    {
        print("test button4");
        return;

        GameManager.Instance.BackToMainMenu();      
    }



}
