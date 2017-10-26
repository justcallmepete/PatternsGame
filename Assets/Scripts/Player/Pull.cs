using System.Collections.Generic;
using UnityEngine;

/*
 * This script will detect other players and gives the player the ability to pull the other player 
 * towards yourself.
 */ 

public class Pull : MonoBehaviour
{
    [Header("General settings")]
    public byte pullPlayerKey = 5;
    [Tooltip("Maximal pull distance")]
    public float maxDistance;

    private Controlable controlable;
    private MainPlayer mainPlayer;
    private List<GameObject> otherPlayers = new List<GameObject>();

    void Start()
    {
        // Cache components for later use
        mainPlayer = gameObject.GetComponent<MainPlayer>();
        controlable = gameObject.GetComponent<Controlable>();

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject obj in allPlayers)
        {
            // Ignore this game object
            if (obj != gameObject)
            {
                otherPlayers.Add(obj);
            }
        }
    }

    private void Update()
    {
        // Do nothing if there is no other player
        if (otherPlayers.Count == 0)
        {
            return;
        }

        // Do nothing if the main player is in the busy state
        if (mainPlayer.IsBusy())
        {
            return;
        }

        if (controlable.GetButtonDown(pullPlayerKey))
        {
            if (IsPlayerInSight())
            {
                PullPlayer(GetClosestPlayer());
            }
        }
    }

    private bool IsPlayerInSight()
    {
        RaycastHit hit;

        Vector3 target = (GetClosestPlayer().transform.position - transform.position).normalized * maxDistance;
        // Draw debug ray
        Debug.DrawRay(transform.position, target, Color.green, 1);

        if (Physics.Raycast(transform.position, target, out hit, maxDistance))
        {
            // Return true if player is hit by the ray cast
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetClosestPlayer()
    {
        int index = 0;
        float minDistance = Vector3.Distance(otherPlayers[0].transform.position, gameObject.transform.position);

        for (int i = 1; i < otherPlayers.Count; i++)
        {
            float distance = Vector3.Distance(otherPlayers[i].transform.position, gameObject.transform.position);

            if (distance < minDistance)
            {
                index = i;
                minDistance = distance;
            }
        }

        return otherPlayers[index];
    }

    private void PullPlayer(GameObject pPlayer)
    {
        pPlayer.GetComponent<MainPlayer>().BePulled(gameObject, maxDistance);
        gameObject.GetComponent<MainPlayer>().PullPlayer(pPlayer);
    } 
}
