using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Singleton GameManagers contains all guards and all players in a scene
 */
public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    private GameObject[] guards;
    private GameObject[] players;

    public GameObject[] Guards { get { return guards; } }
    public GameObject[] Players { get { return players; } }

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
    }

    public void ReloadScene()
    {

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
