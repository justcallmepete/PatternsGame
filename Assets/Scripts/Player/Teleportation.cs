using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will detect other players and gives the player the ability to pull the other player 
 * towards yourself or pull yourself to the other player.
 */

public class Teleportation : MonoBehaviour
{
    [Header("General settings")]
    public byte teleportationKey = 4;
    [Tooltip("Maximal pull distance")]
    public float maxDistance;
    public float channelTime = 3;

    private float currentTime = 0;
    private Controlable controlable;
    private MainPlayer mainPlayer;
    private List<GameObject> otherPlayers = new List<GameObject>();

    void Start()
    {
        mainPlayer = gameObject.GetComponent<MainPlayer>();
        controlable = gameObject.GetComponent<Controlable>();

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in allPlayers)
        {
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

        if (controlable.GetButton(teleportationKey))
        {
            // Start channelling if player is in sight and character is free
            if (IsPlayerInSight())
            {
                if (mainPlayer.IsFree())
                {
                    StartChannelling();
                    StartCoroutine(Channelling(GetClosestPlayer()));
                }
            }
        }
        else if (mainPlayer.IsChannelling())
        {
            // Stop channelling is the key is released
            StopChannelling();
        }

        // Draw debug ray
        if (mainPlayer.IsChannelling())
        {
            Vector3 target = (GetClosestPlayer().transform.position - transform.position).normalized * maxDistance;
            Debug.DrawRay(transform.position, target, Color.green);
        }
    }

    private bool IsPlayerInSight()
    {
        RaycastHit hit;

        Vector3 target = (GetClosestPlayer().transform.position - transform.position).normalized * maxDistance;
        if (Physics.Raycast(transform.position, target, out hit, maxDistance))
        {
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

    private IEnumerator Channelling(GameObject pPlayer)
    {
        // Set current time to zero
        currentTime = 0;
        // Get MainPlayer script from the other player
        MainPlayer pPlayerMainPlayer = pPlayer.GetComponent<MainPlayer>();

        while (currentTime < channelTime)
        {
            // Cancel channelling if player is not in sight, player is not channelling, other player is busy or any other button is pressed
            if (!IsPlayerInSight() || !mainPlayer.IsChannelling() || pPlayerMainPlayer.IsBusy() || controlable.CheckAnyButton(teleportationKey))
            {
                StopChannelling();
                yield break;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Start teleporting
        StartCoroutine(pPlayerMainPlayer.StartTeleport(gameObject));
        StopChannelling();
    }

    private void StopChannelling()
    {
        if (!mainPlayer.IsBusy())
        {
            mainPlayer.CurrentState = MainPlayer.State.Idle;
        }
        Debug.Log("Stop Channel");
        currentTime = 0;
    }

    private void StartChannelling()
    {
        mainPlayer.CurrentState = MainPlayer.State.Channelling;
        Debug.Log("Start Channel");
    }

    // Ratio for the channel bar
    public float GetChannelTimeRatio()
    {
        return currentTime / channelTime;
    }
}
