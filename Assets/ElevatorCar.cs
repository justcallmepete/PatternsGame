using System.Collections;
using UnityEngine;

/*
 * This script will check if any player is in this box collider. A trigger box collider is required.
 * The next scene in the build setting will be loaded after this 
 */
[RequireComponent(typeof(BoxCollider))]
public class ElevatorCar : MonoBehaviour
{
    [Tooltip("Delay in seconds for loading next scene.")]
    public float loadSceneDelay = 3;

    private int requiredAmountOfPlayers;
    private int nPlayer = 0;

    void Awake()
    {
        requiredAmountOfPlayers = GameManager.Instance.Players.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            nPlayer++;

            if (nPlayer == requiredAmountOfPlayers)
            {
                StartCoroutine(LoadNextScene());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            nPlayer--;
        }
    }

    private IEnumerator LoadNextScene()
    {
        // Delay the load scene.
        float currentTime = 0;
        while (currentTime < loadSceneDelay)
        {
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.LoadNextScene();       
    }
}
