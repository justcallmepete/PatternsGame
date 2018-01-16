using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyTools.SoundManager;

/*
 * Gives the player the ability to channel for teleport. Both players must have this script to enable 
 * teleport.
 */ 

public class TeleportComponent : PlayerComponentInterface
{
    public AudioClip teleportInSFX;
    public AudioClip teleportOutSFX;
    public AudioClip chargeSFX;

    private int chargeSFXId;

    [Header("General settings")]
    public string teleportButton = "B";
    private int teleportationKey;
    //[Tooltip("Maximal pull distance")]
    //public float maxDistance = 10;
    [Tooltip("Duraction of channelling")]
    [SerializeField]
    float channelTime = 2.8f;

    [Tooltip("Delay between activating teleport and changing position.")]
    private float teleportDelay = 0.6f;
    [Tooltip("Outer radius of the player")]
    private float outerRadius = 2.3f;
    private float currentTime = 0;
    private bool doTeleport = false;

    private List<GameObject> otherPlayers = new List<GameObject>();

    public override void AwakeComponent()
    {
        base.AwakeComponent();

        teleportationKey = InputManager.Instance.GetKey(teleportButton, MainPlayer.GetPlayerIndex());

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

        if (MainPlayer.buttonList[teleportationKey] )
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
        PlayChargeSFX();
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
                StopChargeSFX();
                // print(!IsPlayerInSight() +","+  !MainPlayer.IsChannelling() + "," + pPlayerMainPlayer.IsBusy() + "," + CheckAnyButton());
                StopChannelling();
                yield break;
            }
            currentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Set teleport values for the other players
        //pPlayerMainPlayer.teleportTarget = gameObject.transform.position + 
        //    (pPlayer.transform.position - gameObject.transform.position).normalized * outerRadius;
        //pPlayerMainPlayer.CurrentState = MainPlayer.State.Teleported;
        StopChannelling();
        MainPlayer.teleportTarget = pPlayer.transform.position +
            (-pPlayer.transform.position + gameObject.transform.position).normalized * outerRadius;
        MainPlayer.CurrentState = MainPlayer.State.Teleported;
        UpdateChannelTimeRatio();
        print(currentTime);
    }

    private void StopChannelling()
    {
        MainPlayer.animator.SetBool("teleport", false);
        if (MainPlayer.CurrentState != MainPlayer.State.Busy)
        {
            MainPlayer.CurrentState = MainPlayer.State.Idle;
            GameObject.Destroy(fxTeleport);
        }
        currentTime = 0;
    }

    public GameObject teleportEffect;
    GameObject fxTeleport;

    private void StartChannelling()
    {
        // teleport effect
        fxTeleport = Instantiate(teleportEffect, transform.position, transform.rotation) as GameObject;

        MainPlayer.animator.SetBool("teleport", true);
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
            if (i == teleportationKey)
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
        PlayTeleportInSFX();
        // Change alpha value to 0
        StartCoroutine(MainPlayer.AlphaFade());

        // Start teleport effect on new location
        fxTeleport = Instantiate(teleportEffect, MainPlayer.teleportTarget, transform.rotation);

        // Dissepear on current location
        SkinnedMeshRenderer[] body = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer i in body)
        {
            i.enabled = false;
        }

        // Delay in teleport
        yield return new WaitForSeconds(teleportDelay);

        // Appear in new location
        foreach (SkinnedMeshRenderer i in body)
        {
            i.enabled = true;
        }

        // Set new position
        gameObject.transform.position = MainPlayer.teleportTarget;

        // Change alpha value to 1
        StartCoroutine(MainPlayer.AlphaFade(1));

        MainPlayer.CurrentState = MainPlayer.State.Idle;
        doTeleport = false;
        PlayTeleportOutSFX();
    }

    public void PlayTeleportInSFX()
    {
        int id = SoundManager.PlaySound(teleportInSFX, 0.4f, false, gameObject.transform);
        Audio open = SoundManager.GetAudio(id);
        open.audioSource.rolloffMode = AudioRolloffMode.Custom;

        open.Set3DDistances(2f, 15f);
        open.audioSource.spatialBlend = 1f;
    }

    public void PlayTeleportOutSFX()
    {
        int id = SoundManager.PlaySound(teleportInSFX, 0.4f, false, gameObject.transform);
        Audio open = SoundManager.GetAudio(id);
        open.audioSource.rolloffMode = AudioRolloffMode.Custom;

        open.Set3DDistances(2f, 15f);
        open.audioSource.spatialBlend = 1f;
    }

    public void PlayChargeSFX()
    {
        Audio sound = SoundManager.GetAudio(chargeSFX);

        if (sound == null)
        {
            SoundManager.PlaySound(chargeSFX, 0.4f, false, gameObject.transform);

            sound = SoundManager.GetAudio(chargeSFX);
            sound.Set3DSettings();
            return;
        }

        sound.Play();
        sound.Set3DSettings();
    }

    public void StopChargeSFX()
    {
        SoundManager.GetAudio(chargeSFXId).Stop();
    }
}
