using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Singleton GameManagers contains all guards and all players in a scene. Scene is also managed in this scene.
 * This manager can be called from any other script with GameManager.Instance.
 */
public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    private GameObject[] guards;
    private GameObject[] players;

    private bool gameOver;

    public GameObject[] Guards { get { return guards; } }
    public GameObject[] Players { get { return players; } }

    private Coroutine slowMotionCoroutine;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

	void Awake () {
        _instance = this;
        guards = GameObject.FindGameObjectsWithTag("Guard");
        players = GameObject.FindGameObjectsWithTag("Player");
        gameOver = false;
    }

    public void ReloadScene()
    {
        gameOver = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReloadCheckpoint()
    {
        SaveLoadControl.Instance.LoadData(true);
    }

    public void LoadNextScene()
    {
        // Loads next scene in Build Settings
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<MainPlayer>().inventory.Keycard = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SlowMotion(float timeScale = 1, float duration = 2)
    {
        if (slowMotionCoroutine != null)
        {
            StopCoroutine(slowMotionCoroutine);
        }

        slowMotionCoroutine = StartCoroutine(DoSlowMotion(timeScale, duration));
    }

    private IEnumerator DoSlowMotion(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
}
