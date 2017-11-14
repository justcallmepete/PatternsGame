using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * The main player class to control the state of the player and other behaviours. It searches all 
 * Player Component Interfaces and controls it by updating it in a specific order.
 */

[RequireComponent(typeof(InputComponent))]
[RequireComponent(typeof(MovementComponent))]
//[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(WhistleComponent))]
[RequireComponent(typeof(TeleportComponent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DetectionComponent))]

public class MainPlayer : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> lightStandingIn;
    [SerializeField]
    public bool IsStandingInLight { get { return (lightStandingIn.Count > 0); } }
    public enum PlayerIndex
    {
        P1, P2
    }

    // Get player index in string.
    public string getPlayerIndex()
    {
        return playerIndex.ToString();
    }

    [Header("Player settings")]
    public PlayerIndex playerIndex;

    [Header("General settings")]
    [Tooltip("The speed of changing the alpha value.")]
    public float alphaSpeed = 1f;    // How fast alpha value decreases.
    private Color color;            // Used to store color reference.
    private Material material;

    

    // Used for channelling
    private float channelTimeRatio = 0;
    public float ChannelTimeRatio { get { return channelTimeRatio; } set { channelTimeRatio = value; } }
    [HideInInspector]
    public Vector3 teleportTarget;

    public enum State
    {
        Idle,
        Busy,
        Teleported,
        Channelling,
        Dead
    }

    /*
     * 0 - X
     * 1 - A
     * 2 - B 
     * 3 - Y
     * 4 - L1
     * 5 - R1
     * 6 - L2
     * 7 - R2
     * 8 - Select
     * 9 - Start
     */
     
    [HideInInspector]
    public bool[] buttonDownList = new bool[10];
    [HideInInspector]
    public bool[] buttonList = new bool[10];
    [HideInInspector]
    public Vector3 axisDirection = Vector3.zero;
    [HideInInspector]
    public PlayerComponentInterface[] components;
    [HideInInspector]
    public Rigidbody rigidBody;
    
    private State currentState = State.Idle;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    public Inventory inventory = new Inventory();


    private void Awake()
    {
        // Cache stuff for later
        components = gameObject.GetComponents<PlayerComponentInterface>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        material = gameObject.GetComponent<MeshRenderer>().material;

        // Save current color
        color = material.color;

        // Initialize
        foreach (PlayerComponentInterface component in components)
        {
            component.MainPlayer = this;
            component.AwakeComponent();
        }

        // Sort array on ID
        Array.Sort(components, delegate (PlayerComponentInterface a, PlayerComponentInterface b)
        {
            return (a.Id).CompareTo(b.Id);
        });
    }

    private void Start()
    {
        if (getPlayerIndex() == "P1")
        {
            inventory = SaveLoadControl.Instance.updatedSavablePlayer1Data.inventory;
        }
        else if (getPlayerIndex() == "P2")
        {
            inventory = SaveLoadControl.Instance.updatedSavablePlayer2Data.inventory;
        }

        LoadData();        
    }

    private void LoadCheckpoint()
    {
        if (SaveLoadControl.Instance.isLoadingCheckpoint)
        {
            Debug.Log("Loading player checkpoint");
            if (getPlayerIndex() == "P1")
            {
                transform.position = new Vector3(SaveLoadControl.Instance.loadedCheckpoint.savedPlayer1Data.playerPosX,
                                                 SaveLoadControl.Instance.loadedCheckpoint.savedPlayer1Data.playerPosY,
                                                 SaveLoadControl.Instance.loadedCheckpoint.savedPlayer1Data.playerPosZ);
            }
            else if (getPlayerIndex() == "P2")
            {
                transform.position = new Vector3(SaveLoadControl.Instance.loadedCheckpoint.savedPlayer2Data.playerPosX,
                                                 SaveLoadControl.Instance.loadedCheckpoint.savedPlayer2Data.playerPosY,
                                                 SaveLoadControl.Instance.loadedCheckpoint.savedPlayer2Data.playerPosZ);
            }
            SaveLoadControl.Instance.isLoadingCheckpoint = false;
        }
    }

    private void LoadData()
    {
        if (SaveLoadControl.Instance.isSceneBeingLoaded)
        {
            if(getPlayerIndex() == "P1")
            {
                transform.position = new Vector3(SaveLoadControl.Instance.loadedData.savedPlayer1Data.playerPosX,
                                                 SaveLoadControl.Instance.loadedData.savedPlayer1Data.playerPosY,
                                                 SaveLoadControl.Instance.loadedData.savedPlayer1Data.playerPosZ);

                inventory = SaveLoadControl.Instance.loadedData.savedPlayer1Data.inventory;
                SaveLoadControl.Instance.isPlayer1Loaded = true;
            }
            else if(getPlayerIndex() == "P2")
            {
                transform.position = new Vector3(SaveLoadControl.Instance.loadedData.savedPlayer2Data.playerPosX,
                                                 SaveLoadControl.Instance.loadedData.savedPlayer2Data.playerPosY,
                                                 SaveLoadControl.Instance.loadedData.savedPlayer2Data.playerPosZ);

                inventory = SaveLoadControl.Instance.loadedData.savedPlayer2Data.inventory;
                SaveLoadControl.Instance.isPlayer2Loaded = true;
            }
        }
    }

    private void SaveData()
    {
        if(getPlayerIndex() == "P1")
        {
            SaveLoadControl.Instance.updatedSavablePlayer1Data.playerPosX = transform.position.x;
            SaveLoadControl.Instance.updatedSavablePlayer1Data.playerPosY = transform.position.y;
            SaveLoadControl.Instance.updatedSavablePlayer1Data.playerPosZ = transform.position.z;
            SaveLoadControl.Instance.updatedSavablePlayer1Data.inventory = inventory;
        }
        else if(getPlayerIndex() == "P2")
        {
            SaveLoadControl.Instance.updatedSavablePlayer2Data.playerPosX = transform.position.x;
            SaveLoadControl.Instance.updatedSavablePlayer2Data.playerPosY = transform.position.y;
            SaveLoadControl.Instance.updatedSavablePlayer2Data.playerPosZ = transform.position.z;
            SaveLoadControl.Instance.updatedSavablePlayer2Data.inventory = inventory;
        }
    }


    private void Update()
    {
        // Update all components
        foreach (PlayerComponentInterface component in components)
        {
            component.UpdateComponent();
        }

        SaveData();
        LoadCheckpoint();
    }

    private void FixedUpdate()
    {
        // Fixed update all components
        foreach (PlayerComponentInterface component in components)
        {
            component.FixedUpdateComponent();
        }
    }

    private void LateUpdate()
    {
        // Fixed update all components
        foreach (PlayerComponentInterface component in components)
        {
            component.LateUpdateComponent();
        }
    }

    // Check current states
    public bool IsBusy()
    {
        return CurrentState == State.Teleported || CurrentState == State.Dead;
    }

    public bool IsFree()
    {
        return CurrentState == State.Idle;
    }

    public bool IsChannelling()
    {
        return CurrentState == State.Channelling;
    }

    public bool IsTeleported()
    {
        return CurrentState == State.Teleported;
    }

    public IEnumerator AlphaFade(float pAlpha = 0)
    {
        // Alpha start value.
        float currentAlpha = material.color.a;

        if (pAlpha < currentAlpha) {
            // Loop until aplha is below zero (completely invisalbe)
            while (currentAlpha > pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha -= alphaSpeed * Time.deltaTime;
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
        else
        {
            while (currentAlpha < pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha += alphaSpeed * Time.deltaTime;
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
    }
}
