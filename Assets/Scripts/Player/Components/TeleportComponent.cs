using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Gives the player the ability to channel for teleport. Both players must have this script to enable 
 * teleport.
 */ 

public class TeleportComponent : PlayerComponentInterface
{
    [Header("General settings")]
    public int teleportationButton;
    //[Tooltip("Maximal pull distance")]
    //public float maxDistance = 10;
    [Tooltip("Duraction of channelling")]
    public float channelTime = 2;
    [SerializeField]
    [Tooltip("Delay between activating teleport and changing position.")]
    private float teleportDelay = 1;
    [Tooltip("Outer radius of the player")]
    [SerializeField]
    private float outerRadius = 1.3f;
    private float currentTime = 0;
    private bool doTeleport = false;

    private List<GameObject> otherPlayers = new List<GameObject>();

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        teleportationButton = InputManager.Instance.GetKey("B", MainPlayer.GetPlayerIndex());

        // Set id
        id = 4;

        // Get all players in the scene
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        // Cache the players except itself
        foreach (GameObject obj in allPlayers)
        {
            if (obj != gameObject)
            {
                otherPlayers.Add(obj);
            }
        }
    }

    public override void UpdateComponent()
    {
        // If player is getting teleported
        if (MainPlayer.IsTeleported() && !doTeleport)
        {
            StartCoroutine(StartTeleport());
            doTeleport = true;
            return;
        }

        // Do nothing if there is no other player
        if (otherPlayers.Count == 0)
        {
            return;
        }

        // Do nothing if the main player is in the busy state
        if (MainPlayer.IsBusy())
        {
            return;
        }

        if (MainPlayer.buttonList[teleportationButton])
        {
            // Start channelling if player is in sight and character is free
            if (IsPlayerInSight())
            {
                if (MainPlayer.IsFree())
                {
                    StartChannelling();
                    StartCoroutine(Channelling(GetClosestPlayer()));
                }
            }
        }
        else if (MainPlayer.IsChannelling())
        {
            // Stop channelling is the key is released
            StopChannelling();
        }

        UpdateChannelTimeRatio();

        // Draw debug ray
        if (MainPlayer.IsChannelling())
        {
            Vector3 target = (GetClosestPlayer().transform.position - transform.position);
            Debug.DrawRay(transform.position, target, Color.green);
        }
    }
    private bool IsPlayerInSight()
    {
        RaycastHit hit;

        Vector3 target = GetClosestPlayer().transform.position - transform.position;
        if (Physics.Raycast(transform.position, target, out hit))
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

        // Must hold teleportkey for channelTime to activate teleport
        while (currentTime < channelTime)
        {
            // Cancel channelling if player is not in sight, player is not channelling, other player is busy or any other button is pressed
            if (!IsPlayerInSight() || !MainPlayer.IsChannelling() || 
                pPlayerMainPlayer.IsBusy() || CheckAnyButton())
            {
                StopChannelling();
                yield break;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Set teleport values for the other players
        pPlayerMainPlayer.teleportTarget = gameObject.transform.position + 
            (pPlayer.transform.position - gameObject.transform.position).normalized * outerRadius;
        pPlayerMainPlayer.CurrentState = MainPlayer.State.Teleported;
        StopChannelling();
    }

    private void StopChannelling()
    {
        if (MainPlayer.CurrentState != MainPlayer.State.Busy)
        {
            MainPlayer.CurrentState = MainPlayer.State.Idle;
        }
        currentTime = 0;
    }

    private void StartChannelling()
    {
        MainPlayer.CurrentState = MainPlayer.State.Channelling;
    }

    // Ratio for the channel bar
    public void UpdateChannelTimeRatio()
    {
        MainPlayer.ChannelTimeRatio = currentTime / channelTime;
    }
    
    // Check if any button except teleport button is pressed
    private bool CheckAnyButton()
    {
        for (int i = 0; i < MainPlayer.buttonList.Length; i++)
        {
            if (i == teleportationButton)
            {
                continue;
            }

            if (MainPlayer.buttonList[i])
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator StartTeleport()
    {
        // Change alpha value to 0
        StartCoroutine(MainPlayer.AlphaFade());

        // Wait for teleport
        yield return new WaitForSeconds(teleportDelay);

        // Set new position
        gameObject.transform.position = MainPlayer.teleportTarget;

        // Change alpha value to 1
        StartCoroutine(MainPlayer.AlphaFade(1));

        MainPlayer.CurrentState = MainPlayer.State.Idle;
        doTeleport = false;
    }
}
